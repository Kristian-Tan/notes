# LXC / Linux Container / LXD

## About
- executing command inside ```cgroup``` isolation, also ```chroot``` to container's rootfs
- default command to be executed first is ```/sbin/init``` inside rootfs (can be overridden by setting ```lxc.init.cmd``` config)
- (maybe) LXC booting only do the init process, unlike full virtualization that do BIOS, bootloader, kernel, then init
- (maybe) LXC will be faster than full virtualization since it doesn't run its own kernel, driver for its virtualized disk, hardware, etc

## Basic operation
- install: ```apt install lxc```
- create: ```lxc-create -t download -n NAME```
- start: ```lxc-start -n NAME```
- access terminal: ```lxc-attach -n NAME``` or ```lxc-console -n NAME``` (attach gets root immediately without login, console gets virtual console like logging in to real server)
- show container status: ```lxc-info -n NAME```
- list containers: ```lxc-ls -f```
- stop: ```lxc-stop -n NAME```
- delete: ```lxc-destroy -n NAME```

## Configuration
- list configuration keys: ```lxc-config -l```
- view configuration value: ```lxc-config KEY```
- example: ```lxc-config lxc.lxcpath``` returns ```/var/lib/lxc```
- file ```/var/lib/lxc/NAME/config``` contains config specific for container named ```NAME``` (also contains path to its rootfs, example: ```/var/lib/lxc/NAME/rootfs/```)
- some configuration to limit resource usage:
  - ```lxc.cgroup.memory.limit_in_bytes = 2560M```
  - ```limits.cpu = 2```

## Storage
- storage can be ```'dir'``` (directory), ```'lvm'``` (logical volume management, stored to specified LV), ```'loop'``` (probably to an ISO or QCOW image), ```'btrfs'``` (as BTRFS subvolume), ```'zfs'```, ```'rbd'``` (ceph)
- 

## Network configuration
- edit config file in ```/var/lib/lxc/NAME/config```
```
lxc.net.0.type = veth
lxc.net.0.flags = up
lxc.net.0.link = bridge0
lxc.net.0.name = eth0
lxc.net.0.hwaddr = 52:54:00:12:34:60
lxc.net.0.ipv4 = 192.168.1.152/24
lxc.net.0.ipv4.gateway = 192.168.1.101
```
- setup bridged network as in QEMU/KVM tutorial, add DHCP
- TODO: how to setup static ip address

## Mount
- use config ```lxc.mount.entry``` (fill with fstab like syntax), e.g.: ```/SRC/DIR /var/lib/lxc/NAME/rootfs/mnt none bind 0 0```
- do not have uid/gid mapping

## Proxmox Managed LXC
- controlling status/start/stop with command `pct list` / `pct start <id>` / `pct stop <id>`
- disk storage (e.g.: raw disk image) location: `/var/lib/vz/images/<id>/*`
- config (e.g.: network, num of cpu/mem/swap, disk images) location: `/etc/pve/lxc/<id>.conf`

#### Migrating Proxmox QEMU/KVM to Proxmox LXC:
- command: https://github.com/my5t3ry/machine-to-proxmox-lxc-ct-converter/blob/master/convert.sh
```bash
collectFS() 
{
    tar -czvvf - -C / \
    --exclude="sys" \
    --exclude="dev" \
    --exclude="run" \
    --exclude="proc" \
    --exclude="*.log" \
    --exclude="*.log*" \
    --exclude="swap.img" \
    .
}
ssh root@your-qemu-kvm-virtual-machine "$(typeset -f collectFS); collectFS" > /var/lib/vz/template/cache/your-template-name.tar.gz
```
  - then, create new LXC container from web interface, use the newly created template
  - this process require free space 3x disk size of QEMU/KVM disk (1x for source disk being copied, 1x for temporary template.tar.gz file, 1x for newly created LXC disk)
- when lxc not booting correctly, debug the init process by this command: `lxc-start -n <id> -F --logfile log.log -l debug`
- quirks
  - to use some kernel functions (e.g.: FUSE when mounting SSHFS, NFS, running docker containers inside LXC) might need to enable feature flags in options > features
  - privileged vs unprivileged containers:
    - QEMU/KVM is more secure than privileged LXC container (e.g.: kernel panic inside container will not affect hypervisor), but LXC container is faster
    - privileged container's init process runs as uid=0 (root); while unprivileged container's init process runs as non-root (e.g. uid=10000)
    - some feature might be available in privileged container only (especially when migrating from QEMU/KVM virtual machine)
    - converting privileged to unprivileged or other way around: must backup, then restore from that backup (cannot just change config, because all file's metadata inside vm disk must be re-mapped to/from uid=0/root), this process involves copying all files in vm disk (might be slow/long running; might need free space 3x size of vm disk)
  - not all linux distro supported: see supported distro in `/usr/share/perl5/PVE/LXC/Setup/` and `/usr/share/lxc/config/`
    - supported distros are: alpine, arch, centOS, debian, devuan, fedora, gentoo, SUSE, ubuntu
    - may choose unmanaged distro, but most likely not working
    - different proxmox version might support different distro version (e.g.: PVE.7.1-12 supports for ubuntu only for ubuntu 12.04 to 22.04)
    - debian is generally more widely supported than ubuntu
  - disk image must be RAW or LVM or bind mount (cannot use QCOW2)
  - user's systemd unit file might not start (reason unknown)
  - edge case: before migrating to lxc, systemd unit file `ssh.service` was changed to make it run on port 443 instead of 22, after migrated to lxc, the listening port becomes 22 because LXC reads `ssh.socket` systemd unit file instead of `ssh.service`

## TO DO:
- without root
- mount
- creating your own lxc template (e.g. via debootstrap)
