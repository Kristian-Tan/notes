### html
```
<a href="kristian:hello+world!">click here</a>
```


### linux
https://unix.stackexchange.com/questions/497146/create-a-custom-url-protocol-handler

```
echo "[Desktop Entry]
Type=Application
Name=Kristian Scheme Handler
Exec=/home/kristian/showargs.sh %u
StartupNotify=false
MimeType=x-scheme-handler/kristian;
" > ~/.local/share/applications/kristian-scheme-handler.desktop

xdg-mime default kristian-scheme-handler.desktop x-scheme-handler/kristian

echo 'notify-send $1' > /home/kristian/showargs.sh
chmod a+x /home/kristian/showargs.sh
```

result: will show "kristian:hello+world!" in linux notification



### windows:
https://stackoverflow.com/questions/80650/how-do-i-register-a-custom-url-protocol-in-windows


create and execute file.reg
```
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\kristian]
"URL Protocol"=""
[HKEY_CLASSES_ROOT\kristian\shell]
[HKEY_CLASSES_ROOT\kristian\shell\open]
[HKEY_CLASSES_ROOT\kristian\shell\open\command] 
@="\"C:\\bin\\ConsoleApp1.exe\" \"%1\""
```

it will be passed as first command line argument to the cli app
