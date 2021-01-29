# SSH Tunnelling

## Concept

### Remote (flag -R)
- connections from an SSH server are forwarded, via the SSH client, to a destination socket
- command: ```ssh -R srcaddr:srcport:destaddr:dstport user@host```
- example: ```ssh -R 192.168.1.112:3031:web.server.com:80 user@192.168.1.112```
    - a remote server ```user@192.168.1.112``` will listen to port 3031,
    - the listening socket is bound to ```192.168.1.112:3031```, that means that if the incoming connection don't have TCP destination address of 192.168.1.112 then it will be dropped
    - when connection made to ```192.168.1.112:3031```, it will be sent to SSH client, and then the SSH client will send it to ```web.server.com:80```
- if srcaddr is not defined, then remote server will be listening to all interface (listen to ```0.0.0.0```)

### Local (flag -L)
- connections from an SSH client are forwarded, via the SSH server, to a destination socket
- command: ```ssh -L srcaddr:srcport:destaddr:dstport user@host```
- example: ```ssh -L localhost:8000:web.server.com:80 user@192.168.1.113```
    - the local computer (SSH client) will listen to port 8000
    - the listening socket is bound to ```localhost:8000```, that means that if the incoming connection don't have TCP destination address of localhost then it will be dropped
    - when connection made to ```localhost:8000```, it will be sent to SSH server, and then the SSH server will send it to ```web.server.com:80```
- if srcaddr is not defined, then local computer will be listening to all interface (listen to ```0.0.0.0```)

## Use case 1: remote server without internet connection
- a remote server (accessable with SSH and HTTP) ```user-devel@devel.local.lan``` is being used as development server
- but the server cannot access internet (any traffic to/from WAN/internet is rejected/dropped by firewall)
- local computer (desktop/laptop) ```user-local@localhost``` can access internet (WAN) and the remote server
- the remote development server needs to make request to internet (WAN) to test a fancy API (HTTP based API on port 80) ```http://fancy.api.server.com```
- tunnelling strategy 1:
    - in local computer, listen to port 3001, make anything sent to that port to be forwarded to API server's port
    - ```ssh -L 3001:fancy.api.server.com:80 localhost```
    - in remote server, listen to port 3031, make anything sent to that port to be forwarded to SSH client's (local computer's) port 3001
    - ```ssh -R 3031:localhost:3001 user@devel.local.lan```
- tunnelling strategy 2:
    - in remote server, listen to port 3031, tunnel that port to to SSH client's (local computer's), set destination to API server's port
    - ```ssh -R 3031:fancy.api.server.com:80 user@devel.local.lan```

## Use case 2: forwarding local port
- a web server (accessable with SSH, visible from WAN/internet) ```web.example.com``` is serving web page to internet
- a development server (accessable with HTTP) ```devel.local.lan``` needs to serve it's development web page through the internet (ex: for demo)
- local computer (desktop/laptop) can access both remote and web server
- tunnelling strategy:
    - in remote server, listen to port 80, make anything sent to that port to be forwarded to SSH client's (local computer's) port 80
    - ```ssh -L 80:devel.local.lan:80 web.example.com```

## References
- https://www.techrepublic.com/article/how-to-use-local-and-remote-ssh-port-forwarding/
- https://www.ssh.com/ssh/tunneling/example
