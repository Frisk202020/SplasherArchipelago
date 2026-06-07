use archipelago_rs::{Client, ConnectionOptions};
use serde::Deserialize;

#[derive(Deserialize, Debug)]
#[allow(dead_code)]
struct SlotData {
    splashers_goal: usize,
    death_link: usize
}

#[tokio::main]
async fn main() {
    let url = "ws://localhost:8080";
    let name = "Frisk";
    let game = Some("Splasher");

    let connection = Client::<SlotData>::connect(
        url, name, game, 
        ConnectionOptions::new()
    ).await.unwrap();

    println!("{:?}", connection.slot_data());

    loop {

    }
}
