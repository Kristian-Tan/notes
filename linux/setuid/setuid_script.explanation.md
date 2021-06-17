# setuid script
- run commands as the user who owns the file
- use case: file fix_permission_script.sh contains ```chmod g+w /home/kristian/hardcoded/path/to/file```, owned by user 'kristian', but other user might also want to execute that command (and it's not acceptable for other user to be given sudo privilege)
- shell script cannot be run with setuid mode: https://unix.stackexchange.com/questions/364/allow-setuid-on-shell-scripts
- the workaround is to compile a program that calls the script

## file explanation
- file setuid_script.sh
```
#!/bin/bash
echo "running as: '`id`'" # print/show which user id executed the script
echo "effective id: '`id -u`'"
chmod g+w /home/kristian/hardcoded/path/to/file # do the specified command
```
- file: setuid_script.c
```
    #include <stdio.h>
    #include <stdlib.h>
    #include <sys/types.h>
    #include <unistd.h>

    int main()
    {
        clearenv(); // clear environment variable
        setuid(1005); // setuid to target user's uid (run `id` in terminal to show your uid OR see /etc/passwd file)
        setreuid(1005, 1005); // set real and effective uid
        system("/home/kristian/setuid_script.sh"); // run the specified script
        return 0;
    }
```
- the wrapper can then be compiled with setuid_script.build.sh
```
cd /home/kristian # change directory to where the setuid_script is
gcc setuid_script.c -o setuid_script # compile the binary wrapper with gcc
chmod 4755 setuid_script # chmod to give the binary wrapper rwsr-xr-x (user can read-write-execute, group and other can read-execute, and setuid bit on so other user can execute it as kristian)
chmod 4755 setuid_script.sh
chmod 0744 setuid_script.build.sh
chmod 0744 setuid_script.c
```

## security consideration
- /home/kristian/setuid_script.sh should not be editable by other user (must set proper file permission), otherwise any user can run command as user kristian by editing file /home/kristian/setuid_script.sh
- path to the wrapped shell script MUST be absolute path, otherwise any user can copy the binary wrapper to his own home directory and create a .sh file with same name and run arbitrary command
- other common attack vector includes injecting environment variable (e.g.: PATH), so ```clearenv()``` might be necessarry

