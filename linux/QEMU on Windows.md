# QEMU on Windows

- download QEMU installer from https://qemu.weilnetz.de/w64/ then add to PATH
- enable windows feature from control panel, check "Hyper-V" / "Windows Virtualization Platform" / "Windows Virtual Machine"
- install HAXM from https://software.intel.com/en-us/articles/intel-hardware-accelerated-execution-manager-intel-haxm or https://github.com/intel/haxm
- use by adding ```-accel hax``` or ```--enable-hax```

## Cloud Init
- download cdrtools from http://freshmeat.sourceforge.net/projects/cdrecord/ or https://opensourcepack.blogspot.com/p/cdrtools.html
- add the 64bit binary to PATH or copy ```mkisofs.exe``` to current directory
- creating init disk can be done as in linux: ```mkisofs -o init.img -V cidata -r -J init``` (can also try ```./mkisofs.exe``` if failed)

## Networking

### References
- https://dev.to/whaleshark271/using-qemu-on-windows-10-home-edition-4062
- https://www.qemu.org/2017/11/22/haxm-usage-windows/
