# KeePass Password Manager

## About
- offline and open source password manager
- password is saved in an encrypted file (called password database)
- the file format is open, so there are various KeePass clients on various platform/OS developed by various different open source project
- to synchronize password database, mounted/networked filesystem can be used (e.g.: SFTP, WebDAV, SMB/Samba, Google Drive, One Drive, DropBox)

## Features
- offline: password database is your own responsibility (does not depend on third party provider)
- self hosted: no monthly fee, payment plan, etc
- brute force resistant: transforming master key can be configured to need X loop (one attempt can take 1s, preventing millions of guess per second brute force)
- encryption can use key file (both as 2fa or as sole key): allow to open only on specific computer that have the key, the key can also be changed

## Recommended Setup:
- SFTP on a linux box you control or rent
- allow ssh login to the linux box ONLY with ssh key / pubkey: on all your owned devices, add them to authorized_keys
- enable keyfile: copy to all your owned devices
- to synchronize database file: use fuse sshfs on linux, use sirikali sshfs on windows, use Keepass2Android sftp capability
- backup key periodically by setting up a cron job on the sftpd linux box, e.g.: git on the directory then use ```git add . ; git commit -m `date` ``` as cron command
- auto mount your sftp on computer startup

## Recommended Clients

### Recommended Client: KeePassXC (Windows/Linux/MacOS)
- https://keepassxc.org/
- have browser integration (download add-on/extension on respective browser's add-on/extension store)
- have auto-type (enter password automatically by detecting window title): so it can be used to auto type password in ssh client (e.g.: linux console/bash/git bash)
- have feature to check password in 'have i been pwned' service

### Recommended Client: Keepass2Android (Android)
- https://play.google.com/store/apps/details?id=keepass2android.keepass2android
- can be used as android's autofill service https://developer.android.com/guide/topics/text/autofill-services
    - can be used in browser: identify which password/account should be filled by browser's url
    - can be used in other application: identify which password/account should be filled by application's package name
- can stay opened in background, if cleared from recent application, will ask for quick unlock (enter last 3 character of password)

### iPhone Client: none tried yet
- none tried yet
