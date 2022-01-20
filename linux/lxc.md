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

## TO DO:
- without root
- mount
- creating your own lxc template (e.g. via debootstrap)
