1. for rooted Android
- ON ANDROID: run terminal emulator, run commands:
```
su
setprop service.adb.tcp.port 5555
stop adbd
start adbd
```
- ON COMPUTER: run terminal/cmd, run commands:
```
adb kill-server
adb connect 192.168.1.13:5555
adb devices
```

- ON ANDROID: to stop device from listening to tcp (and back to listening to usb only)
```
su
setprop service.adb.tcp.port -1
stop adbd
start adbd
```

2. for non-rooted Android
- HARDWARE: connect device and computer with USB cable
- ON COMPUTER: run terminal/cmd, run commands:
```
adb tcpip 5555
adb connect 192.168.1.13:5555
```
- HARDWARE: disconnect USB cable