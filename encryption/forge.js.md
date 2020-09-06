# forge.js: RSA Asymmetric Encryption

## Source
- https://github.com/digitalbazaar/forge

## Installation
- npm: install ```npm install node-forge``` then require module ```var forge = require('node-forge');```
- cdn: jsDelivr ```<script src="https://cdn.jsdelivr.net/npm/node-forge@0.7.0/dist/forge.min.js"></script>``` unpkg ```<script src="https://unpkg.com/node-forge@0.7.0/dist/forge.min.js"></script>```
- download my copy ```forge.min.js```

## Key Generation
```js
var keypair = forge.pki.rsa.generateKeyPair({bits: 2048, e: 0x10001});
openSSHstringPrivateKey = forge.ssh.privateKeyToOpenSSH(keypair.privateKey, ""); // privateKey, password (empty string=no password)
openSSHstringPublicKey = forge.ssh.publicKeyToOpenSSH(keypair.publicKey, "my key generated with forge.js"); // publicKey, comment
console.log(openSSHstringPrivateKey); // string: -----BEGIN RSA PRIVATE KEY----- ...
console.log(openSSHstringPublicKey); // string: ssh-rsa ...
pemStringPrivateKey = forge.pki.privateKeyToPem(keypair.privateKey); // pem format
pemStringPublicKey = forge.pki.publicKeyToPem(keypair.publicKey); // pem format
console.log(openSSHstringPrivateKey); // string: -----BEGIN RSA PRIVATE KEY----- ...
console.log(openSSHstringPublicKey); // string: -----END PUBLIC KEY----- ...
```

## Encryption
- encrypt with public key
- decrypt with private key
- this is like sending message in a sealed letter, only the receiver can see the message but anyone can send message
- encrypt
```js
var keypair = {
    privateKey: forge.pki.privateKeyFromPem(pemStringPrivateKey),
    publicKey: forge.pki.publicKeyFromPem(pemStringPublicKey),
};
var encrypted = keypair.publicKey.encrypt("this plaintext will be encrypted"); // (defaults to forge.pki.rsaES PKCS#1 v1.5)
encrypted = btoa(encrypted); // encode to base64 (btoa stands for binary to ascii)
```
- decrypt
```js
var keypair = {
    privateKey: forge.pki.privateKeyFromPem(pemStringPrivateKey),
    publicKey: forge.pki.publicKeyFromPem(pemStringPublicKey),
};
var encrypted = atob("UaHTMEKRUo6RhHqbeVjxr9r5gM0M1JtpoZCe6JOoBt81zJWXDpLhQnNFTgNIaYcOj5KzGGD4Yx5uYeLlDIOOwYdy1Tq2VDQqAzRBCjIpvQgcXNPL+3RhDH3zicQlv9PWZ3UmKZ/sS9BadikwR9bTZ8VI4/+M7Hqwii21JLcaTxUhZlJjY/oaeu83ayOc+suxxgo1+w4Gprdy09HgkdFF+aiCDvWv2f7MgChwfG+vYu78U4tsDW2R5QPVxDVsFR6Z6Nqq6N6phXmSlBb2rqNqTZxrWfhS9EFL5oH8jcSFB6iNGZsqppJorTm9gUWZ0llajscCy6Zq5t1qYuPyvpj89g==");
var decrypted_plaintext = keypair.privateKey.decrypt(encrypted); // (defaults to forge.pki.rsaES PKCS#1 v1.5)
```

## Signing
- encrypt with private key
- decrypt with public key
- this is like sending physically signed message, the receiver can be sure that the sender can only be the signer
- signing
```js
var keypair = {
    privateKey: forge.pki.privateKeyFromPem(pemStringPrivateKey),
    publicKey: forge.pki.publicKeyFromPem(pemStringPublicKey),
};
var plaintext = "sign this";
var myhash = forge.md.sha1.create();
myhash.update(plaintext, "utf8"); // hash plaintext
var signature = keypair.privateKey.sign(myhash); // return string (but in binary format)
signature = btoa(signature); // encode to base64 (btoa stands for binary to ascii)
console.log(signature); // ZPvxxP9n/tfIx+E2LNANgtmMaZZqFk7CeidVl8ASLHnfNpXyhzPepSePdt+N3vG1em/AWEeQ4cN12hOeY/SCqUSF/lSW8mSbPdFNGoVpkPT3RTkmWvXZHp4ASKt3i9j/nmcyDKHyLMI5BsFxmecSHFPIWjcPPzPuKmJG39aH3q1HBESr7wdMdxsX5cQkOGLgSB5uPCun0yiGkGxxy+6s0Dy6bTUR3tvNiFQSV811C6rrPlZF6sBTX4ZNnrQ7kZcyaPTUr4exMcwvrR/e/dUZng6VKofOevMoTMFzgYYaSBVOA12UmYO4EZMFWW1PFH51iq/xEpXuaBQ5x5NwGZa0Ag==
```
- verify
```js
var keypair = {
    privateKey: forge.pki.privateKeyFromPem(pemStringPrivateKey),
    publicKey: forge.pki.publicKeyFromPem(pemStringPublicKey),
};
var plaintext = "sign this";
var signature = atob("ZPvxxP9n/tfIx+E2LNANgtmMaZZqFk7CeidVl8ASLHnfNpXyhzPepSePdt+N3vG1em/AWEeQ4cN12hOeY/SCqUSF/lSW8mSbPdFNGoVpkPT3RTkmWvXZHp4ASKt3i9j/nmcyDKHyLMI5BsFxmecSHFPIWjcPPzPuKmJG39aH3q1HBESr7wdMdxsX5cQkOGLgSB5uPCun0yiGkGxxy+6s0Dy6bTUR3tvNiFQSV811C6rrPlZF6sBTX4ZNnrQ7kZcyaPTUr4exMcwvrR/e/dUZng6VKofOevMoTMFzgYYaSBVOA12UmYO4EZMFWW1PFH51iq/xEpXuaBQ5x5NwGZa0Ag=="); // base64 decode (atob stands for ascii to binary)
var myhash = forge.md.sha1.create();
myhash.update(plaintext, "utf8"); // hash challenge plaintext
var verified = keypair.publicKey.verify(myhash.digest().bytes(), signature); // return bool
```
