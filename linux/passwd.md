# Linux Password Format

- in /etc/passwd, 2nd field contains 'x' means that password is stored in /etc/shadow
- in /etc/shadow, 2nd field contains salted-hashed password, example: ```$6$kHcprDro7O64GeQJ$h/M5on3C.rnAu7xt3uROjTVtZgvCMQ/hXsDae0lU7nd/3h4IXvRPmmidxrxnn0yBtfR5iRF0Xu7JnE0yrt5DY0```
- format: ```$hash_algo_number$salt$hashed_password```
  - $hash_algo_number can be 1 (md5), 5 (sha256), 6 (sha512), see: ```openssl passwd --help```
  - $salt and $hashed_password is base64 encoded
- to verify:
  ```bash
  openssl passwd -$hash_algo_number -salt $salt $plaintext_password
  # output: $complete_hashed_password
  ```
- or the $plaintext_password can be omitted (so it's not recorded in shell history)
- example:
  - password is ```guest```, stored as ```$6$kHcprDro7O64GeQJ$h/M5on3C.rnAu7xt3uROjTVtZgvCMQ/hXsDae0lU7nd/3h4IXvRPmmidxrxnn0yBtfR5iRF0Xu7JnE0yrt5DY0```
    ```bash
    openssl passwd -6 -salt kHcprDro7O64GeQJ guest
    # output: $6$kHcprDro7O64GeQJ$h/M5on3C.rnAu7xt3uROjTVtZgvCMQ/hXsDae0lU7nd/3h4IXvRPmmidxrxnn0yBtfR5iRF0Xu7JnE0yrt5DY0
    ```
  - to compare if they're exact match, sed command can be used:
    ```bash
    openssl passwd -$hash_algo_number -salt $salt $plaintext_password | sed 's;$complete_hashed_password;;'
    # if output is empty line, then the password is a match
    ```
  - the sed command will try to replace $complete_hashed_password with empty line (note that sed's default delimiter '/' is swapped with ';' delimiter since $complete_hashed_password contains '/' character as part of base64 encoding)
  - example:
    ```bash
    openssl passwd -6 -salt kHcprDro7O64GeQJ guest | sed 's;$6$kHcprDro7O64GeQJ$h/M5on3C.rnAu7xt3uROjTVtZgvCMQ/hXsDae0lU7nd/3h4IXvRPmmidxrxnn0yBtfR5iRF0Xu7JnE0yrt5DY0;;'
    # if output is empty line, then the password is a match
    ```
