# SSHFS

## About
Filesystem over SSH (secure shell). Content of remote server (usually linux server) can be browsed through local computer like directory/drive

## How (Linux)
- install `sshfs` package (with apt/pacman/yum/etc.)
- edit `/etc/fuse.conf`, uncomment `user_allow_other` (to enable non-root user to use sshfs)
- create mount point (ex: `/home/kristian/mnt/ip99`)
- run with command: `sshfs -o allow_other,IdentityFile=/home/kristian/.ssh/id_rsa kristian@192.168.1.99:/ /home/kristian/mnt/ip99`
- explanation:
    - specify to use ssh key stored in windows drive = ```/home/kristian/.ssh/id_rsa```
    - remote ssh user = ```kristian```
    - remote ssh address = ```192.168.1.99```
    - mount remote ssh filesystem at root = ```:/```
    - mount it to mount point = ```/home/kristian/mnt/ip99```
- frequently used options:
    - allow_other = allow non-root user to access mounted filesystem
    - IdentityFile = ssh key used to login to remote ssh server

## How (Windows): WSL
WSL (Windows Subsystem for Linux) is a way to run Linux in Windows, it is advertised to reach near native performance (although it still have some stability issue)
- get latest update to windows 10 (google it or go to link)
    - run `winver` (should be newer or equal to 20H2)
    - https://www.microsoft.com/en-id/windows/get-windows-10
    - download windows 10 updater from official microsoft site
- install wsl (google it, seems like only need to run some powershell command)
    - https://docs.microsoft.com/en-us/windows/wsl/install-win10
- install distro (ex: ubuntu20.04) from Microsoft Store (open from start -> type 'store' -> type 'wsl' OR 'ubuntu' OR 'suse')
- follow installation on linux above
- open windows explorer, type ```\\wsl$``` in address bar / path
- navigate to your mount point

## How (Windows): sshfs-win
- install winfsp https://github.com/billziss-gh/winfsp/releases
- install sshfs-win https://github.com/billziss-gh/sshfs-win/releases
- make sure to install **stable** version, not **beta** or **testing** (last known good version is `sshfs-win-3.5.20024-x64` and `winfsp-1.8.20304`)
- go to my computer / this PC (in explorer)
- right click my computer / this PC, then "map network drive"
- select drive, fill folder with: `\\sshfs\[LOCUSER=]REMUSER@HOST[!PORT][\PATH]`
- sshfs can be replaced with `sshfs.r` to make it mounted at root
- example: `\\sshfs.r\kristian@192.168.1.99!22\` (mount sshfs to root, username kristian, ssh server at 192.168.1.99 on port 22)
- known issue: does not respect ssh user's umask

## How (Windows): SiriKali (recommended)
SiriKali is multiplatform frontend GUI for various secure filesystem (one of them is sshfs). It can use linux's native sshfs as backend and use sshfs-win on windows as backend
- install winfsp and sshfs-win (see above)
- install sirikali https://github.com/mhogomchungu/sirikali/releases
- open sirikali
- create volume -> sshfs
- input:
    - remote ssh server address: `kristian@192.168.1.99:/`
    - ssh port number: `22`
    - volume type: not changeable `sshfs`
    - mount point path: `Z:`
    - mount options: `create_file_umask=0000,create_dir_umask=0000,umask=0000,idmap=user,StrictHostKeyChecking=no,UseNetworkDrive=yes,reconnect,ServerAliveInterval=15,ServerAliveCountMax=3`
        - explanation: mount options can be `key=value` or just `flag`, separated by comma
        - default options: `create_file_umask=0000,create_dir_umask=0000,umask=0000,idmap=user,StrictHostKeyChecking=no`
            - option `create_file_umask` and `create_dir_umask` and `umask` set file mode's umask, this needs to be set in sshfs since sshfs-win DOES NOT respect UNIX umask (will make file permission become rwx------)
            - option `idmap=user` will translate the file uid of remote filesystem to uid of local user
            - option `StrictHostKeyChecking=no` will make key fingerprint relaxed (always trust server's key)
        - set to use network drive: `UseNetworkDrive=yes`
        - automatically reconnect when connection failed (will timeout after 1 minute): `reconnect,ServerAliveInterval=15,ServerAliveCountMax=3`
    - check `create network drive`
    - may check `auto mount volume` (if checked, will mount on startup, and will mount when clicking favorites -> mount all)
- click "add" button
- close window, back to sirikali main menu, click "favorites" button, select newly created sshfs volume to mount it
recommendation: save password so user don't need to input it every mount
- favorites -> manage favorites
- tab manage keys in wallet
- radio button Windows DPAPI
- set master password (can be empty)
- select "volume path" from dropdown (example: `Sshfs kristian@192.168.1.99:/`)
- enter your UNIX account password
- click add volume to selected wallet
- next time before mount, the password field will already have value

## How (Linux): SiriKali
SiriKali can also be used for GUI frontend on linux if the user don't want to use command line
- install sirikali by downloading from https://github.com/mhogomchungu/sirikali/releases or from PPA (debian based) or from AUR (arch based)
- see usage above

## How (macOS)
- install brew (go to https://brew.sh and then copy paste to terminal)
- run `brew cask`
- run `brew install --cask osxfuse`
- run `brew install sshfs`
- run sshfs from terminal like on linux (see above)

