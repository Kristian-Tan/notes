# UEFI boot method: 
reading .efi file to boot (maybe like PS2's .elf file?)



## ESP partition:
- use FAT32 filesystem
- have GUID partition ID = C12A7328-F81F-11D2-BA4B-00A0C93EC93B (EFI) => https://en.wikipedia.org/wiki/GUID_Partition_Table
- usually called "EFI System Partition (ESP)" 
- usually mounted on /boot/efi/
- usually created by Windows as 100MB partition at start of disk (hence /dev/sda1 on linux)

if this partition is not exist already, create from ```cfdisk```/```fdisk```, then ```mkfs.fat -F32 /dev/sda1``` (make FAT32 filesystem)



## firmware boot options / boot entry
boot entry can be added / edited by efibootmgr (Linux) or bcdedit (Windows)

boot entry will be shown on BIOS setting under "boot priority",
this boot entry will be read by BIOS, then tried from first available (ex: first priority is to boot from USB, then from HDD)

show boot entry priority:
```efibootmgr``` or ```efibootmgr -v```

change boot entry priority order:
```efibootmgr -o 0000,0001,0002,0003```
(change the 4 hexadecimal as needed)

adding grub to boot entry:
```mount /dev/sda1 /boot/efi/``` >>> assuming your EFI boot partition (ESP) is on /dev/sda1
```grub-install /dev/sda --target=x86_64-efi --efi-directory=/boot/efi/``` >>> if failed, try adding "--no-nvram" or "--removable" or "--force" or "--force-extra-removable"
```ls /boot/efi/EFI/``` >>> find where grub-install saved it's .efi file, usually called "grubx64.efi"
```efibootmgr --create --disk /dev/sda --part 1 -L <label> -l \EFI\<label>\grubx64.efi``` >>> label is the directory found in previous step, make sure the path is actually accessbile (can be found), ```-p 1``` means on partition 1 (equals to /dev/sda1)



## fixing error:
start job is running for /dev/disk/by-uuid dependency failed for /boot/efi
>>> remove the entry from /etc/fstab

other way to edit EFI partition:
- The EFI setup utility -- Most EFIs provide setup utilities that you can access by hitting a special key at boot time (Esc, Del, or a function key, typically; but what key it is varies from one system to another). These often, but not always, provide a way to adjust the boot order. If your firmware provides such an option, you should be able to use it to move GRUB to the top position. (GRUB is likely to be called ubuntu, given that you installed it from that distribution.)
- An EFI shell -- You can use the bcfg command in an EFI version 2 shell, as described on the Arch Linux wiki (https://wiki.archlinux.org/title/Unified_Extensible_Firmware_Interface#UEFI_Shell). If your system isn't already set up with an easy-to-access shell, this approach is likely to be harder to use than the others, but it is OS-agnostic.
- EasyUEFI -- The third-party Windows EasyUEFI (https://www.easyuefi.com/index-us.html) paid program is likely to be the easiest way to do what you want. You can click the ubuntu entry in EasyUEFI's list and move it to the top.
- bcdedit -- The Windows bcdedit command can alter the NVRAM-based boot order. Specifically, opening an Administrator Command Prompt window and typing ```bcdedit /set "{bootmgr}" path \EFI\ubuntu\shimx64.efi``` (optionally followed by ```bcdedit /set "{bootmgr}" description "ubuntu"``` to keep the description sensible) should do the trick.
- efibootmgr -- This Linux tool can adjust the boot order. Begin by typing sudo efibootmgr alone to see the options. Note the number (Boot####) associated with the ubuntu entry, and the current boot order (on the BootOrder line). You can then enter a new boot order with the ubuntu entry at the top by using the -o option. For instance, if the current boot order is 0000,0003,0007,0004 and ubuntu is 0007, you'd type sudo efibootmgr -o 0007,0000,0003,0004 to adjust the boot order.
- refind-mkdefault -- This script comes with rEFInd, and it's a way to automate the preceding procedure. If you're not using rEFInd, you'd need to download the script here and make it executable (chmod a+x refind-mkdefault). You'd then run it as sudo ./refind-mkdefault -L ubuntu or sudo ./refind-mkdefault -L shimx64 to make GRUB the default boot entry.




## about configuring grub:
files used to generate grub config:
- /etc/default/grub 
- /etc/grub.d/ (directory)
files generated: /boot/grub/grub.cfg

## acronym:
- EFI = Extensible Firmware Interface
- UEFI = Unified Extensible Firmware Interface
- FAT = file allocation table
- FS = filesystem
- GUID = globally unique identifiers
- GPT = GUID partition table
- sdX = SCSI disk X
- BIOS = basic input/output service

## definitions:
- BIOS and UEFI = BIOS is old way to boot computer, EFI is newer (address BIOS weakness such as: unable to boot from storage larger than 2TB, maximum 4 partitions per disk)
- BIOS = perform POST (power on self-test), then basic I/O, then boot (from disk/MBR, network/PXE)
- secure boot = UEFI feature to make sure operating system boot not tampered
- UEFI legacy mode / UEFI BIOS support = UEFI to emulate BIOS boot (read from MBR instead of ESP)
- MBR = first 512 bytes of the disk, describe disk partition, describe how to start loading the operating system, MBR disk contains maximum four primary partitions, one of primary partitions set active/bootable, 
- MBR bootloader = used to load OS, bootloader size approximately 440 bytes, bootloader will find active partition and execute Volume Boot Record (the first sector of that partition), which will continue the process of loading the operating system.


## experiment: booting into UEFI shell
- on arch, install package: edk2-shell "pacman -S edk2-shell"
- on arch, list where the files are stored: "pacman -Ql edk2-shell" ==>> found out that it's stored on /usr/share/edk2-shell/x64/Shell_Full.efi
- on arch, mount ESP from /dev/sda1 to /boot/efi: "mount /dev/sda1 /boot/efi"
- on arch, copy edk2-shell for x64 architecture to /boot/efi/shellx64.efi: "cp /usr/share/edk2-shell/x64/Shell_Full.efi /boot/efi/shellx64.efi"
- reboot, then launch BIOS/UEFI setting, then select "Launch EFI Shell from filesystem device"

## about GRUB:
commands:
- insmod = load module (insert module)
- chainloader = load specified EFI
- to boot linux:
```
grub> set root=(hd0,1)
grub> linux /boot/vmlinuz-3.13.0-29-generic root=/dev/sda1
grub> initrd /boot/initrd.img-3.13.0-29-generic
grub> boot
```
- to boot efi file: (using chainloader, usually also used to boot windows)
```
grub> set root=(hd0,1)
grub> chainloader /efi/boot/BOOTX64.EFI
grub> boot
```


## config files:
- /etc/default/grub = KEY=value pair setting
- /etc/grub.d/* = setting for generating grub.cfg
- /boot/grub/grub.cfg = generated grub config, do not edit directly, generated by grub-mkconfig -o /boot/grub/grub.cfg
- configuration example: save last booted menuentry, then on next boot automatically highlight that menu
```
# in /etc/default/grub
GRUB_SAVEDEFAULT=true
GRUB_DEFAULT=saved
# inside menuentry
menuentry "my linux" {
  savedefault
  linux /vmlinuz
  initrd /initramfs
}
```

## installing systemd-boot bootloader (alongside grub)
- https://wiki.archlinux.org/title/Systemd-boot
- on arch, install systemd (should already be installed), then systemd-boot should already be in ```/usr/lib/systemd/boot/efi/systemd-bootx64.efi```
- copy it to ESP
- create configuration inside ESP:
  - loader/loader.conf https://man.archlinux.org/man/loader.conf.5
    ```
    default  myos.conf
    timeout  300
    console-mode max
    editor   yes
    ```
  - loader/entries/myos.conf https://systemd.io/BOOT_LOADER_SPECIFICATION/#type-1-boot-loader-specification-entries
    ```
    title myos
    efi /EFI/path/to/vmlinuz
    options initrd=/EFI/path/to/intel-ucode.img initrd=/EFI/path/to/initramfs.img root="UUID=22e0XXXX-5cXX-4bXX-bbXX-b5XXXXXXXX" rw udev.log_priority=3
    ```
  - loader options can be taken from grub's config too
- add boot option: ```efibootmgr -c -d /dev/nvme0n1 -p 1 -L SystemdBoot -l '\EFI\SystemdBoot\systemd-bootx64.efi'```
- file layout of ESP should be as such (assuming dual boot with windows)
```
/boot/efi
├── EFI
│   ├── Boot
│   │   └── bootx64.efi <<< default boot
│   ├── Grub
│   │   └── grubx64.efi <<< grub bootloader
│   ├── SystemdBoot
│   │   ├── systemd-bootx64.efi <<< systemd-boot bootloader
│   │   ├── initramfs-5.4-x86_64.img <<< linux's initramfs/initrd, must be copied to ESP because systemd-boot cannot read the root filesystem
│   │   ├── intel-ucode.img <<< also copy intel/amd-ucode here, must be copied to ESP because systemd-boot cannot read the root filesystem
│   │   └── vmlinuz-5.4-x86_64 <<< kernel, must be copied to ESP because systemd-boot cannot read the root filesystem
│   └── Microsoft
│       ├── Boot
│       │   ├── BCD
│       │   ├── BCD.LOG
│       │   ├── BCD.LOG1
│       │   ├── BCD.LOG2
│       │   ├── bg-BG
│       │   │   ├── bootmgfw.efi.mui
│       │   │   └── bootmgr.efi.mui
│       │   ├── bootmgfw.efi <<< windows' bootloader
│       │   ├── bootmgr.efi
│       │   ├── BOOTSTAT.DAT
│       │   ├── boot.stl
│       └   └───── ... REST OMITTED ...
├── loader
│   ├── entries
│   │   └── myos.conf <<< systemd-boot boot entry file, each operating system that you want to be selectable should be made into a new file here
│   ├── loader.conf <<< systemd-boot main config file
│   └── random-seed
├── NvVars
├── shellx64.efi <<< uefi shell, efi file from package edk2-shell
└── System Volume Information
```
