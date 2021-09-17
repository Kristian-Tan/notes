# QEMU/KVM Cloud-Init

## About
- cloud-init is a way to configure VM disk image (e.g.: ```qcow2```) without modifying the disk itself (so a cloud provider can have 1 master cloud-init image, then just copy it when user wants new VM)
- components is: 'cloud-init' VM disk image, 'cloud-init disk' (often called config.img/seed.img/init.img)
- after VM image boots, it runs 'cloud-init' systemd target, which reads 'cloud-init disk', then configure machine based on that configuration (e.g.: configure ssh key, add users)

### Components

#### Cloud-init VM Image
- the 'cloud-init VM image' should be a virtual disk (e.g.: qcow2) attached to the virtual machine (e.g.: via ```-hda init.img``` or ```-drive if=virtio,format=qcow2,file=cloudimg-amd64-disk-kvm.img```)
- must be mounted on as first disk
- this virtual disk contains installed OS, also installed cloud-init package

#### Cloud-init Disk
- the 'cloud-init disk' should be a virtual raw CD-ROM attached to the virtual machine (e.g.: via ```-cdrom init.img``` or ```-drive if=virtio,format=raw,file=tmp.img```)
- must be mounted on as second (and last) disk
- this raw CD-ROM image is created with cloud-image-utils (or lower layer tools like ```mkisofs``` or ```xorriso```)
- must contain these files:
  - meta-data
```yml
instance-id: 95256f88-600c-4914-b9d6-e70b7e375aeb
hostname: 95256f88-600c-4914-b9d6-e70b7e375aeb
local-hostname: 95256f88-600c-4914-b9d6-e70b7e375aeb
```
  - user-data
```yml
#cloud-config
users:
- name: kristian
  ssh-authorized-keys:
  - ssh-rsa ...
  sudo: ['ALL=(ALL) NOPASSWD:ALL']
  groups: sudo
  shell: /bin/bash
timezone: Asia/Jakarta
```
  - example: https://cloudinit.readthedocs.io/en/latest/topics/examples.html
- generating 'cloud-init disk'
  - mkisofs: 
    - ```mkisofs -o outputimage.img -V cidata -r -J --quiet mydir/```
    - mydir/ must contain meta-data and user-data
  - xorriso
    - ```xorriso -as genisoimage -output outputimage.iso -volid CIDATA -joliet -rock mydir/```
  - cloud-image-utils
    - ```cloud-localds -v --network-config=network_config_static.cfg outputimage.iso cloud_init.cfg```
    - where ```cloud_init.cfg``` and ```network_config_static.cfg``` is specific yml formatted config file
    - need to install package ```cloud-image-utils```

## Example
- download cloud image VM disk
  - https://docs.openstack.org/image-guide/obtain-images.html
  - ubuntu: https://cloud-images.ubuntu.com/ example: https://cloud-images.ubuntu.com/focal/current/focal-server-cloudimg-amd64-disk-kvm.img
  - arch: https://mirror.pkgbuild.com/images/latest/
- create configuration file (see above meta-data and user-data)
- generate 'cloud-init disk' (see above mkisofs)
- run qemu:
```bash
qemu-system-x86_64 -enable-kvm \
 -smp 4,cores=2 -m 2G \
 -nographic \
 -device virtio-net-pci,netdev=net00 \
 -netdev id=net00,type=user,hostfwd=tcp::2222-:22 \
 -device virtio-net-pci,netdev=net01,mac='52:54:00:12:34:7A' \
 -netdev id=net01,type=bridge,br=bridge0 \
 -drive if=virtio,format=qcow2,file=cloudimg-amd64-disk-kvm.img \
 -drive if=virtio,format=raw,file=init.img
```
- qemu option explanation:
  - ```-nographic```
    - do not use graphical window output (print output in terminal)
    - without this some VM will show message 'boot without initrd' and stuck there
    - only use this for image that boots without grub / without bootloader (e.g.: ubuntu cloud and newer)
  - ```-device virtio-net-pci,netdev=net00 -netdev id=net00,type=user,hostfwd=tcp::2222-:22```
    - add device using 'virtio-net-pci' driver, using 'net00' as backend
    - which is defined as user mode network backend that do TCP forwarding from guest:2222 to hypervisor:22
    - this enables us to ssh to cloud VM with ```ssh user@127.0.0.1 -p 2222 -o StrictHostKeyChecking=no```, otherwise, you need to configure dhcp server
  - ```-device virtio-net-pci,netdev=net01,mac='52:54:00:12:34:7A' -netdev id=net01,type=bridge,br=bridge0```
    - add virtual network interface to bridged network with the hypervisor (see notes about QEMU/KVM networking)

## Networking
- some images (e.g.: ubuntu) tries to autoconfigure its network via DHCP DISCOVER when it's started up, while others (e.g.: arch) does not, see my other notes about 'QEMU/KVM Networking' to setup DHCP on VM hypervisor
- some ubuntu images cannot be connected via ssh when ```network-config``` file exist in 'cloud-init disk' via TCP forwarding (e.g.: ```ssh -p2222 localhost``` from host)
  - this is because the guest cloud VM only accept ssh connection bind to their network address (cannot be from 0.0.0.0 or localhost:2222 portmapped)
  - to connect, use ```ssh 192.168.1.14``` instead (since the port is bound to an IP address now)
  - but some other images (e.g.: debian) can still accept ssh via portmapped/forwarded port localhost:2222
- to set-up networking automatically in cloud-init VM, create file ```network-config``` in 'cloud init disk' with format:
  - network-config
```yml
version: 2
ethernets:
    ens4:
        dhcp4: false
        dhcp6: false
        addresses:
          - 192.168.1.14/24
        gateway4: 192.168.1.101
        nameservers:
          addresses:
            - 8.8.8.8
            - 8.8.4.4
```
  - https://cloudinit.readthedocs.io/en/latest/topics/network-config.html
  - network interface on QEMU/KVM usually starts at ens3 (ethernet at PCIE slot 3), but if slot 3 is used for port mapping / forwarding then the bridge interface is usually at ens4, e.g.:
```bash
qemu-system-x86_64 -enable-kvm \
 -smp 4,cores=2 -m 2G \
 -nographic \
 -device virtio-net-pci,netdev=net00 \
 -netdev id=net00,type=user,hostfwd=tcp::2222-:22 \ # this one will become ens3 \
 -device virtio-net-pci,netdev=net01,mac='52:54:00:12:34:7A' \ # this one will become ens3 \
 -netdev id=net01,type=bridge,br=bridge0 \ # this one will become ens4 \
 -drive if=virtio,format=qcow2,file=cloudimg-amd64-disk-kvm.img \ # this one will become ens4 \
 -drive if=virtio,format=raw,file=init.img
```

## Resizing Image
- some cloud-init image can automatically increase its own filesystem on boot when it detected the virtual disk size has increased (e.g.: ubuntu cloud image, ubuntu server installation)
- if image cannot be resized automatically, use these (for ext4):
- condition:
  - old size 20G, new size 40G (+20G)
  - partition table:
    - /dev/sda1 512MB ESP
    - /dev/sda2 19.5G ext4 root file system
- resize virtual disk = from hypervisor: ```qemu-img resize mydisk.qcow2 +20G```
- resize partition table = from vm: ```cfdisk /dev/sda``` then resize partition table for /dev/sda2 (when asked for new size, just answer the default as the default is biggest size possible / use all free space after the partition)
- then restart vm (to load new partition table)
- resize file system = from vm: ```resize2fs /dev/sda2```

## NOTE
- sometime if the 'cloud-init disk' content have been changed, the guest cloud image just don't reload / re-read its config; try to change it's meta-data (instance-id, hostname, local-hostname)
