# phpseclib: RSA Asymmetric Encryption

## Source
- https://github.com/phpseclib/phpseclib
- http://phpseclib.sourceforge.net/

## Installation
- composer ```composer require phpseclib/phpseclib:~2.0```
- clone github, then ```composer install```
- download from sourceforge
- download my copy ```phpseclib.zip```

## Key Generation
- todo

## Encryption
- encrypt with public key
- decrypt with private key
- this is like sending message in a sealed letter, only the receiver can see the message but anyone can send message
- encrypt
```php
require_once("vendor/autoload.php");
define('CRYPT_RSA_PKCS15_COMPAT', true); // phpseclib use PKCS#1 v2.1 whereas OpenSSL implemenents PKCS#1 v1.5, use this line to make sure the ciphertext can be read by openSSH
$rsa = new phpseclib\Crypt\RSA();
$rsa->setEncryptionMode(phpseclib\Crypt\RSA::ENCRYPTION_PKCS1); // set encryption method to PKCS1 (common, less secure)
$rsa->loadKey(file_get_contents("id_rsa.pub")); // public key file path, encoded in openSSH format, usually starts with "ssh-rsa", then base64 encoded key
$plaintext = "hello world";
$ciphertext = $rsa->encrypt($plaintext);
$ciphertext = base64_encode($ciphertext);
```
- decrypt
```php
require_once("vendor/autoload.php");
$rsa = new phpseclib\Crypt\RSA();
$rsa->setEncryptionMode(phpseclib\Crypt\RSA::ENCRYPTION_PKCS1); // set encryption method to PKCS1 (common, less secure)
$rsa->loadKey(file_get_contents("id_rsa")); // private key file path, encoded in openSSH format, usually starts with "-----BEGIN RSA PRIVATE KEY-----", then base64 encoded key
$ciphertext = base64_decode("KtT2rBdOHDwj4h6oqFdQlx3WyjFR3IJF123ignQdbOo72dvOfPtsAdLS2Hgaz+OsawnoZwWKZskkQSUGHnM0OXkt+WHT71oDZyv0qo0AfjIJe6t+NNylc3Q2eEb7WBYQ5uLP6NU5IpQWo1oc9l+acIuS5Jrbqd3adXp1r0gucAj1JkyKtzUd/q7XhA/LyuzscD+uYFQwkF65oP3b/RgEMbI1B8IhxQu6zmeFzhy0Bfrv7huhrdYUfGrQ6qTlCNovYKYrvnuLrnB158HwLmWaoBGKxlOp/692W/ddJoNzOLhVSAxoTAEoSj8Gpa0qGYQHWQeqk7+EcotWMQmWNfYc1A==");
$decrypted_plaintext = $rsa->decrypt($ciphertext);
```

## Signing
- encrypt with private key
- decrypt with public key
- this is like sending physically signed message, the receiver can be sure that the sender can only be the signer
- signing
```php
require_once("vendor/autoload.php");
$rsa = new phpseclib\Crypt\RSA();
$rsa->setSignatureMode(phpseclib\Crypt\RSA::SIGNATURE_PKCS1); // set encryption method to PKCS1 (common, less secure)
$rsa->loadKey(file_get_contents("id_rsa")); // private key file path, encoded in openSSH format, usually starts with "-----BEGIN RSA PRIVATE KEY-----", then base64 encoded key
$plaintext = "sign this";
$signature = $rsa->sign($plaintext);
$signature = base64_encode($signature);
```
- verify signature
```php
require_once("vendor/autoload.php");
$rsa = new phpseclib\Crypt\RSA();
$rsa->setSignatureMode(phpseclib\Crypt\RSA::SIGNATURE_PKCS1); // set encryption method to PKCS1 (common, less secure)
$rsa->loadKey(file_get_contents("id_rsa.pub")); // public key file path, encoded in openSSH format, usually starts with "ssh-rsa", then base64 encoded key
$plaintext = "sign this";
$signature = "ZPvxxP9n/tfIx+E2LNANgtmMaZZqFk7CeidVl8ASLHnfNpXyhzPepSePdt+N3vG1em/AWEeQ4cN12hOeY/SCqUSF/lSW8mSbPdFNGoVpkPT3RTkmWvXZHp4ASKt3i9j/nmcyDKHyLMI5BsFxmecSHFPIWjcPPzPuKmJG39aH3q1HBESr7wdMdxsX5cQkOGLgSB5uPCun0yiGkGxxy+6s0Dy6bTUR3tvNiFQSV811C6rrPlZF6sBTX4ZNnrQ7kZcyaPTUr4exMcwvrR/e/dUZng6VKofOevMoTMFzgYYaSBVOA12UmYO4EZMFWW1PFH51iq/xEpXuaBQ5x5NwGZa0Ag==";
$signature = base64_decode($signature);
$is_authentic = $rsa->verify($plaintext, $signature); // return true on successful verification
```

