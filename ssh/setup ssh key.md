# Setup SSH Key

- open terminal (e.g.: linux bash/zsh/fish terminal, windows git bash/mingw64/cygwin, not command prompt, not powershell)
- generate a SSH private-public key pair
```
    $ ssh-keygen -t rsa -b 4096 -C "any label here"
```
- when prompted where to save key, press enter, when prompted to add passphrase, press enter (let it be saved to default location with no passphrase)
- run ssh-agent
```
    $ eval $(ssh-agent -s)
```
- add your SSH private key to ssh-agent
```
    $ ssh-add ~/.ssh/id_rsa
```
- see SSH public key
```
    $ cat ~/.ssh/id_rsa.pub
```

