# LetsEncrypt GetSSL

Sources:
- https://github.com/srvrco/getssl
- https://letsencrypt.org/docs/client-options/#clients-bash

## Installation
- install dnsutils for dig ```apt install dnsutils```
- change user to root
- change directory to installation directory
- download script via curl
- chmod to only allow root to execute
- example:
```bash
sudo su
cd /opt/
curl --silent https://raw.githubusercontent.com/srvrco/getssl/latest/getssl > getssl
chmod 700 getssl
ln -s /opt/getssl /usr/bin/getssl
```

## Create GetSSL Configuration
- only do this step once (after installation)
- will create ```~/.getssl/``` directory AND setup ```~/.getssl/yourdomain.com/``` directory
```bash
getssl -c yourdomain.com
```

## Create Initial Certificate
- create acme-challenge directory (writable by root), this directory should be exposed to web via http://yourdomain.com/.well-known/acme-challenge
  - example: ```mkdir /var/www/youromain.com/web/.well-known/acme-challenge```
- edit ```~/.getssl/getssl.cfg```, change server to real one (not staging one), just uncomment ```CA``` part
- edit ```~/.getssl/yourdomain.com/getssl.cfg```, 
  - comment ```SANS```
  - add ```ACL=('/var/www/youromain.com/web/.well-known/acme-challenge')```
- example output:
```
creating domain csr - /root/.getssl/yourdomain.com/yourdomain.com.csr
Registering account
Registered
Verify each domain
Verifying yourdomain.com
copying challenge token to /var/www/yourdomain.com/web/.well-known/acme-challenge/XXX
sending request to ACME server saying we're ready for challenge
checking if challenge is complete
Pending
checking if challenge is complete
Pending
checking if challenge is complete
Pending
checking if challenge is complete
Pending
checking if challenge is complete
Verified yourdomain.com
Verification completed, obtaining certificate.
Requesting Finalize Link
Requesting Order Link
Requesting certificate
Certificate saved in /root/.getssl/yourdomain.com/yourdomain.com.crt
```

## Schedule Auto Renewal
- add in cron: ```0 0 1 * * /opt/getssl -d -a -u >> /root/log_cron_getssl```
- see log every 1st date of month, at 00:00 in ```/root/log_cron_getssl```

## Rate Limiter of LetsEncrypt
https://letsencrypt.org/docs/rate-limits/
- 50 per week per registered domain
- 300 new orders per account per 3 hours
- max 100 domain name per certificate
- 5 duplicate certificate per week (for renewal)
- Failed Validation: 5 failures per account, per hostname, per hour
