# RSYSLOG-CENTRALIZED

- experiment to create centralized log server with rsyslog
- the 'clients' (hosts that will send its logs) can be new machines with rsyslogd and systemd-journald; but should also be old machines (2008's) with sysklogd

## Setup
- 192.168.17.239 = centralized log server (running ubuntu 20.04 with rsyslog)
- 192.168.17.238 = ubuntu 20.04 (with rsyslog 8.2001.0 and systemd-journald)
- 192.168.17.244 = ubuntu 18.04 (with rsyslog 8.32.0 and systemd-journald)
- 192.168.17.245 = ubuntu 8.04 (with sysklogd 1.5-1ubuntu1 / syslogd 1.5.0)
- 192.168.17.246 = ubuntu 10.04 (with rsyslog 4.2.0)
- 192.168.17.247 = ubuntu 10.04 (with rsyslog 4.2.0)
- 192.168.17.248 = debian 7 wheezy (with rsyslog 5.8.11-3+deb7u2)

## Configuring Log Server
- on 192.168.17.239
- edit /etc/rsyslog.conf
```
$template RemoteLogs,"/var/log/loghost/%FROMHOST-IP%/%syslogfacility-text%.%syslogseverity-text%.log"
if $FROMHOST-IP=='192.168.17.244' then ?RemoteLogs
& stop
if $FROMHOST-IP=='192.168.17.248' then ?RemoteLogs
& stop
```
- uncomment tcp and udp ports (note that tcp and udp can run on same port number, e.g.: 514)
```
# provides UDP syslog reception
module(load="imudp")
input(type="imudp" port="514")

# provides TCP syslog reception
module(load="imtcp")
input(type="imtcp" port="514")
```
- setup logrotate:
```
/var/log/loghost/*/*.log {
  daily
  rotate 5
  missingok
  notifempty
  compress
  delaycompress
  create 644 syslog syslog
  sharedscripts
  postrotate
    /usr/lib/rsyslog/rsyslog-rotate
  endscript
}
```

## Syslog (on client) with rsyslog
- if client is using rsyslog (e.g.: 192.168.17.238, 192.168.17.244, 192.168.17.246, 192.168.17.247, 192.168.17.248), then sending log via tcp is possible:
- add line below in /etc/rsyslog.conf
```
*.* @@192.168.17.239:514
```

## Syslog (on client) with systemd-journald
- edit /etc/systemd/journald.conf, uncomment
```
ForwardToSyslog=yes
```

## Syslog (on client) with sysklogd
- if client is using sysklogd (e.g.: 192.168.17.245), then it can only send via udp
- add line below in /etc/rsyslog.conf
```
*.* @192.168.17.239
```
- edit /etc/services
```
syslog    514/tcp
syslog    514/udp
```
- note that ```@@=tcp, @=udp```

## Apache (on client)
- apache does not use syslog facility but manages its own log
- if forwarded to syslog (via piping to logger), then how long a request is served (e.g.: how long php is running) can be seen:
```
2022-02-02T13:31:31+07:00 localhost apache2_custom: 192.168.25.12 - - [02/Feb/2022:13:29:45 +0700] "GET /bench.php HTTP/1.1" 200 1511 "https://192.168.17.244/" "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36"
```
- time 13:31 = request finished (rsyslog get it from logger), time 13:29 = request started (when apache custom log is written)
- configure in ```/etc/apache2/apache.conf``` and ```/etc/apache2/sites-enabled/*``` below:
```
ErrorLog "| /usr/bin/logger -t apache2_error -p local3.err"
CustomLog "| /usr/bin/logger -t apache2_custom -p local3.info" combined
```

- to automate changing all ErrorLog and CustomLog in apache2 config files (e.g.: when there are too many vhost in sites-enabled)
```
# replace all, add new line ErrorLog
sed -i 's#\(.*\)ErrorLog\(.*\)#\1ErrorLog\2\n\1ErrorLog "| /usr/bin/logger -t apache2_error -p local3.err"\2#' /etc/apache2/sites-enabled/*

# comment out old ErrorLog line
sed -i 's;ErrorLog /var/log;#ErrorLog /var/log;g' /etc/apache2/sites-enabled/*

# replace all, add new line CustomLog
sed -i 's#\(.*\)CustomLog\(.*\)#\1CustomLog\2\n\1CustomLog "| /usr/bin/logger -t apache2_custom -p local3.info"\2#' /etc/apache2/sites-enabled/*

# comment out old CustomLog line
sed -i 's;CustomLog /var/log;#CustomLog /var/log;g' /etc/apache2/sites-enabled/*

# fix wrong sed above for ErrorLog
sed -i 's;ErrorLog "| /usr/bin/logger -t apache2_error -p local3.err".*;ErrorLog "| /usr/bin/logger -t apache2_error -p local3.err";g' /etc/apache2/sites-enabled/*

# fix wrong sed above for CustomLog
sed -i 's;CustomLog "| /usr/bin/logger -t apache2_custom -p local3.info" .* combined;CustomLog "| /usr/bin/logger -t apache2_custom -p local3.info" combined;g' /etc/apache2/sites-enabled/*
```
