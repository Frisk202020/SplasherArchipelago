# Splasher Archipelago's Proxy

Splasher runs on .NET Framework 3.5, which is a deprecated runtime which, crucially, does not support **latest internet security protocols**. For instance, when the client tries to connect to a secured server (e.g. most servers on the internet, including `archipelago.gg`), it fails with a `connexion timeout` because the TLS handshake is refused.

## Implementation

### Explained strategy

The easiest solution is use a third-party program (launched automatically by the client), which does support latest protocols (TLS 1.3) as a gateway towards the end-server. Specifically :

- The proxy accepts unencrypted traffic from the client (over TCP), which is secure because all traffic happens on the local machine.
- The proxy encrypts inbound traffic towards the end-server, and decrypts outbound traffic towards the client.
- The proxy does not read / analyze received traffic, it is only a gateway between the client and the end-server.

This has the added benefit of reducing the network overhead of the client, since by using the proxy, the client will only communicate with the local proxy, which will then handle (heavier) network traffic. If an error occurs, it is reflected to the client and handled as usual.

### Implementation choices

We decided to go with Rust for multiple reasons :

- for coding convenience (language preference)
- for shipping cleanness : only the final executable needs to be shipped (no runtime)
- for performance 

Though the current implementation is working, it is not optimized :

- We may be able to reduce dependencies
- **The file is stable only on windows**

Future versions should get rid of `native-tls` for `rustls`.