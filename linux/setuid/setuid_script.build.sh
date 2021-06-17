cd /home/kristian
gcc setuid_script.c -o setuid_script
chmod 4755 setuid_script
chmod 4755 setuid_script.sh
chmod 0744 setuid_script.build.sh
chmod 0744 setuid_script.c