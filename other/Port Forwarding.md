#NGROK
https://ngrok.com
- needs login/register
- needs to download client program in target host
- https://dashboard.ngrok.com/get-started

#SERVEO
https://serveo.net/
- no login, no client program
- require ssh client program (can use git bash or cygwin in windows)

## USAGE
```
ssh -R 80:localhost:[LOCAL_PORT] serveo.net
```
ex: ssh -R 80:localhost:3000 serveo.net // forward port 3000 to internet
### request particular subdomain:
```
ssh -R incubo.serveo.net:80:localhost:8888 serveo.net
```

### use your own domain
- register for free subdomain in freedns.afraid.com
- make A record to point to 159.89.214.31
- make/generate an ssh key
- get the key's fingerprint with command ```ssh-keygen -l```
- make TXT record "authkeyfp=SHA256:nOkcoaG8ymwaUkyNYy3Fn9Vh7vVWuNQ/vCKlXPQSGzc"
- port forward it with command:
```
ssh -R kristian.us.to:80:localhost:80 serveo.net // assuming the subdomain is kristian.us.to
```
