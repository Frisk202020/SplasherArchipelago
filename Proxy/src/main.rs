use std::net::SocketAddr;
use tokio::net::{TcpListener, TcpStream};
use native_tls::{TlsConnector, Protocol};
use std::sync::Arc;

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let mut args = std::env::args();
    args.next();

    let (remote_host, remote_port) = (args.next().unwrap(), args.next().unwrap()); 
    let local_addr: SocketAddr = "127.0.0.1:8080".parse()?;
    
    // In production, you can pass the real target port from the game dynamically
    let remote_target = format!("{}:{}", remote_host, remote_port);

    // Configure the Outbound Client to use modern TLS 1.3 to Archipelago
    let connector = TlsConnector::builder()
        .min_protocol_version(Some(Protocol::Tlsv13)) 
        .build()?;
    let connector = tokio_native_tls::TlsConnector::from(connector);
    let connector = Arc::new(connector);

    let listener = TcpListener::bind(&local_addr).await?;
    eprintln!("[Proxy] Active on {}, targeting {remote_target}", local_addr);

    loop {
        // Accept plain, unencrypted TCP stream from the Unity Game
        let (mut client_stream, _) = listener.accept().await?;
        let connector_clone = connector.clone();
        let target_address = remote_target.clone();
        let host_name = remote_host.to_string();

        eprintln!("[Proxy] Game connected. Opening secure pipeline to Archipelago...");
        match TcpStream::connect(&target_address).await {
            Ok(server_tcp) => {
                match connector_clone.connect(&host_name, server_tcp).await {
                    Ok(mut secure_server_stream) => {
                        eprintln!("[Proxy] TLS 1.3 Handshake complete. Splicing streams.");

                        let _ = tokio::io::copy_bidirectional(
                            &mut client_stream, 
                            &mut secure_server_stream
                        ).await;
                        
                        eprintln!("[Proxy] Pipeline closed.");
                    }
                    Err(e) => eprintln!("[Error][Proxy] Internet TLS Handshake Failed: {}", e),
                }
            }
            Err(e) => eprintln!("[Error][Proxy] Failed to physically connect to Archipelago server: {}", e),
        }
    }
}