## Boot Linux
steps:
1. identify disk partition that needs to be loaded
2. locate vmlinuz and initrd.img

example:
```
grub> set root=(hd0,1)
grub> linux /boot/vmlinuz-3.13.0-29-generic root=/dev/sda1
grub> initrd /boot/initrd.img-3.13.0-29-generic
grub> boot
```


## Boot EFI
steps:
1. identify disk partition that needs to be loaded
2. locate .efi file

example:
```
grub> set root=(hd0,1)
grub> chainloader /efi/boot/BOOTX64.EFI
grub> boot
```
