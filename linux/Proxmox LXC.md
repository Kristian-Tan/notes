# Proxmox LXC

## About
- LXC is system container (like docker container, but instead of just one app, LXC is for complete system/OS)
- in term of performance and isolation, LXC is middle ground between full virtualization (e.g.: QEMU/KVM) and containerization (e.g.: docker)

## Converting Proxmox Container to LXC
- must check OS compatibility (check file: `/usr/share/perl5/PVE/LXC/Setup/(distro).pm` )
- unprivileged container is preferred if possible (e.g.: need mknod, permission problem)
- some feature flags may be needed (e.g.: FUSE, nesting)
- reference: https://github.com/my5t3ry/machine-to-proxmox-lxc-ct-converter/blob/master/convert.sh
- ssh host roles:
    - proxmox hypervisor (called "proxmox")
    - guest OS that will be converted (called "guest")
    - copier that can connect via ssh to guest OS and copy its whole filesystem, proxmox host can also play this role (called "copier")
- run this command on proxmox host:
```bash
collectFS() {
    tar -czvvf - -C / \
      --exclude="sys" \
      --exclude="dev" \
      --exclude="run" \
      --exclude="proc" \
      --exclude="*.log" \
      --exclude="*.log*" \
      --exclude="*.gz" \
      --exclude="swap.img" \
      --exclude="swapfile" \
  .
}
```
- make sure copier can ssh to guest OS as root
- then copy the filesystem image from proxmox host (via copier if needed):
```bash
# without copier
ssh root@guest "$(typeset -f collectFS); collectFS" > /var/lib/vz/template/cache/my_custom_filesystem.tar.gz

# via copier
ssh -J kristian@copier root@guest "$(typeset -f collectFS); collectFS" > /var/lib/vz/template/cache/my_custom_filesystem.tar.gz
```
- use the newly copied filesystem as LXC template (can create from web GUI)

#### Diagnose Failed to Start
- start LXC manually via:
```bash
lxc-start -n 102 -F -l DEBUG -o /tmp/my_lxc.log
```

#### Example Problem: Missing Basic FHS Directory
- sometime, because we add to ignore /sys /dev/ /run /proc, the template filesystem did not contain those directory, not even empty ones
- to check if basic FHS directory exists, mount the filesystem first:
```bash
pct mount 102
mount # shows that mount point is in /var/lib/lxc/102/rootfs
cd /var/lib/lxc/102/rootfs
mkdir sys
mkdir dev
mkdir run
mkdir proc
cd ~
pct nmount 102
```

#### Configurations:
- network (static/dynamic IPv4/IPv6, default gateway/route, nameserver/dns, vlan tagging)
- consider if guest OS ssh keys need to be regenerated or use older ones (can backup then restore `/etc/ssh` directory content)

#### Known Problem:
- when assigning 2 interface with different IP address, both MAC address replied to ARP request (might cause problem with some network appliance)
- incompatibility with guest OS ubuntu older than 12.04 or debian older than 4, cannot use unmanaged (not debugged yet)
- ...
