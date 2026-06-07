use std::net::SocketAddr;
use rustls::{ClientConfig, RootCertStore, pki_types::ServerName};
use tokio::net::{TcpListener, TcpStream};
use tokio_rustls::TlsConnector;
use std::sync::Arc;

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let mut args = std::env::args();
    args.next();

    let (remote_host, remote_port) = (args.next().unwrap(), args.next().unwrap()); 
    let local_addr: SocketAddr = "127.0.0.1:8080".parse()?;    
    let remote_target = format!("{}:{}", remote_host, remote_port);
    let remote_host = ServerName::try_from(remote_host).unwrap();

    let store = RootCertStore::from_iter(
        webpki_roots::TLS_SERVER_ROOTS.iter().cloned()
    );

    let config  = ClientConfig::builder()
        .with_root_certificates(store).with_no_client_auth();

    let connector = TlsConnector::from(Arc::new(config));
    let listener = TcpListener::bind(&local_addr).await?;
    eprintln!("[Proxy] Active on {}, targeting {remote_target}", local_addr);

    loop {
        // Accept plain, unencrypted TCP stream from the Unity Game
        let (mut client_stream, _) = listener.accept().await?;
        let connector_clone = connector.clone();
        let target_address = remote_target.clone();
        let host_name = remote_host.clone();

        eprintln!("[Proxy] Game connected. Opening secure pipeline to Archipelago...");
        match TcpStream::connect(&target_address).await {
            Ok(server_tcp) => {
                match connector_clone.connect(host_name, server_tcp).await {
                    Ok(mut secure_server_stream) => {
                        eprintln!("[Proxy] TLS Handshake complete. Splicing streams.");

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