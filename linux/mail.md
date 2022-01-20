
MUA = mail user agent (like browser in http, e.g.: mail/mailx)
MTA = mail transfer agent (e.g.: sendmail, postfix, exim and qmail), program running as a system service which collects messages sent by other local accounts as well as messages received from the network, sent from remote user accounts; also read outbox (designated directory in filesystem), then send with SMTP to remote MTA if destination is remote

## alias
- file : /etc/aliases
- compile to /etc/newaliases.db with `newaliases` or `sendmail -bi`
- format: `<alias>: <destination>`, destination can be:
  - user
  - user@external.domain
  - /path/to/file/to/append
  - |/pipe/mail/content/to/cmd
- user's `~/.forward` can also be used (only have destination)

## SMTP protocol:
- HELO server.domain
  - client initiate connection by sending this
- MAIL FROM: from@my.domain
- RCPT TO: to@ur.domain
- DATA
- RSET
- VRFY
  - confirm that specified user and mailbox exist (often turned off because it will lead to scanning/fingerprinting)
- NOOP
  - do nothing (stay alive)
- QUIT
- EHLO
  - extended hello (to specify to use extended smtp command, like below)
- AUTH {LOGIN/PLAIN/CRAM-MD5}
  - AUTH PLAIN base64encodeduserandpassconcatenated
  - AUTH LOGIN (server send base64('username'), client send base64($username), server send base64('password'), client send base64($password) )
  - AUTH CRAM-MD5 (server send base64($random_challenge), client send base64($username+' '+hmac(secret=$password, msg=$random_challenge) ) )
- STARTTLS
- HELP

## example : sending mail from LAN to LAN

- mail -> sendmail -> local MTA (postfix) -> recipient MTA / inbox (postfix in second server)
- machine1=local237, ip 192.168.1.237, have user kristian
- machine2=local254, ip 192.168.1.254, have user kristian

### both machine: debian11 
```
apt install postfix
apt install bsd-mailx
```

### setup postfix as follow (edit if already exist, add line if not exist),
below is for local237 (change 237 to 254 for second host)
- /etc/postfix/main.cf (reload/restart service after changes)
```
# set my host name to local237, and my network address to 192.168.1.0/24 (since it's on class C local network)
myhostname = local237
mynetworks = 192.168.1.0/24

# normally postfix will query DNS for MX record, but since we do not have such DNS record for local237 dan local254 we're using transport(5) file, content of file explained later
transport_maps = hash:/etc/postfix/transport

# my origin set to content of /etc/mailname file, content of file explained later
myorigin = /etc/mailname

# set my destination to computer's host name (e.g. your bash prompt 'user@myhostname ~$: ...')
mydestination = myhostname, 192.168.1.237, localhost.localdomain, localhost, local237

# set relay to none
relayhost =

# set to support all interfaces and all protocols (permissive because only for testing)
inet_interfaces = all
inet_protocols = all
```

- /etc/mailname
```
192.168.1.237
```

- /etc/postfix/transport (run `postmap /etc/postfix/transport` after changes to regenerate `/etc/postfix/transport.db`)
below is for local237 (change 254 to 237 for second host)
  - this file should contain DESTINATION server (not current server), so in local237 it should contain local254's address and vice versa
  - this file is referred so we don't need to set DNS record (if DNS MX record is set then it's not required)
```
local254 smtp:[192.168.1.254]
```

- /etc/hosts
```
127.0.0.1       localhost
192.168.1.237   local237
192.168.1.254   local254
```

- verify that service already runs in port 25 (e.g.: use `ss -ltn` or `lsof -i -P -n`)


### testing sending email
- via telnet/netcat to smtp server port 25
  - run `telnet local237 25` or `telnet local254 25`
  - type `HELO local254`
  - type `MAIL FROM: kristian@local237`
  - type `RCPT TO: kristian@local254`
  - type `DATA`
  - type the message body (start with 'Subject: ...', then 2 line break, then message body)
  - terminate the message with server's end data character (usually `<CR><LF>.<CR><LF>`, which means `(enter)(dot)(enter)`)
  - type `QUIT`
  - server's response should always be code 2xx (ok), if it's 5xx (error), check how to debug below
- via sendmail -vf
  - make file `Subject: examplesubject \n\n text email content \n`
  - run `cat file.txt | sendmail -vf 'kristian@local237' 'kristian@local254' ` (from 237 to 254, run it in 237)
  - run `mail` in both machine, in sender machine you should get 'delivery report' email, while in destination machine you should get the email
- via bsd-mails
  - run `mail -r kristian@local237 kristian@local254` (from 237 to 254, run it in 237)
  - type subject and message, then ctrl-d (EOF)

### how to debug if all fails
- look for logs in `journalctl -xe` (or use syslog if on older OS)
- or look for logs in `/var/log/mail.*`, e.g. `cat /var/log/mail.* | sort` or `tail -n0 -f /var/log/mail.*`
- to increase verbosity, edit `/etc/postfix/master.cf`, change 'smtpd' to 'smtpd -v' (verbose)
- maybe need to edit `/etc/postfix/master.cf`, change chroot to n (no)
- see outbox queue with `mailq` (or `sendmail -bp`)


## example : sending email via external smtp server

- mail -> sendmail -> external smtp server -> recipient MTA / inbox
- in this case using mailtrap
- /etc/postfix/main.cf
```
relayhost = [smtp.mailtrap.io]:2525
smtp_sasl_auth_enable = yes
smtp_sasl_mechanism_filter = plain
smtp_sasl_security_options = noanonymous
smtp_sasl_password_maps = hash:/etc/postfix/sasl_passwd
```

- /etc/postfix/sasl_passwd (create account at mailtrap first), (run `postmap /etc/postfix/sasl_passwd` after changes to regenerate `/etc/postfix/sasl_passwd.db`)
```
smtp.mailtrap.io yoursecretusername:yoursecretpassword
```

- restart postfix and send email
