# SMTP Mail Trap/Mock

- for helping development, sometimes we need to send email directly to SMTP server (usually with PHPMailer)
- one way to dump/view the email is to use `tcpdump` (too low level) or with MailTrap.io (too high level, or maybe just not having internet connection to internet)
- other way (in linux box) is to use `postfix`'s `smtp-sink`

## Installation
- install postfix, e.g.: `apt install postfix`
- run smtp-sink, e.g.: `smtp-sink -v -d %d.%H.%M.%S -u kristian -R /home/kristian/ 0.0.0.0:1025 10`
    - `-v` = verbose (optional)
    - `-u kristian` = run as user kristian
    - `-R /home/kristian` = write captured mail to /home/kristian
    - `-d %d.%H.%M.%S` = write email to file in filename format day.hour.minute.second
    - `0.0.0.0:1025 10` = bind to 0.0.0.0 (all interface/all IP address), port 1025 (allow non-root), set backlog to 10 (should be enough for development)

## Usage Example
#### command:

```bash
mkdir /var/www/html/1
cd /var/www/html/1
/usr/bin/php7.2 /usr/bin/composer require phpmailer/phpmailer
echo "..." > index.php
/usr/bin/php7.2 index.php
```

#### content of index.php (taken and modified from https://stackoverflow.com/a/23905054 )

```php
<?php
require 'vendor/autoload.php';
$mail = new \PHPMailer\PHPMailer\PHPMailer;

$mail->isSMTP();
$mail->Host = 'localhost:1025';

$mail->From = 'from@example.com';
$mail->FromName = 'Mailer';
$mail->addAddress('joe@example.net', 'Joe User');
$mail->addAddress('ellen@example.com');
$mail->addReplyTo('info@example.com', 'Information');
$mail->addCC('cc@example.com');
$mail->addBCC('bcc@example.com');

$mail->WordWrap = 50;
$mail->isHTML(true);

$mail->Subject = 'Here is the subject';
$mail->Body    = 'This is the HTML message body <b>in bold!</b>';
$mail->AltBody = 'This is the body in plain text for non-HTML mail clients';

if(!$mail->send()) {
    echo 'Message could not be sent.';
    echo 'Mailer Error: ' . $mail->ErrorInfo;
} else {
    echo 'Message has been sent';
}
```

#### example output:

```
cat ~/31.04.27.536f637299 
X-Client-Addr: 192.168.1.144
X-Client-Proto: ESMTP
X-Helo-Args: php7-dev
X-Mail-Args: <from@example.com>
X-Rcpt-Args: <joe@example.net>
X-Rcpt-Args: <ellen@example.com>
X-Rcpt-Args: <cc@example.com>
X-Rcpt-Args: <bcc@example.com>
Received: from php7-dev ([192.168.1.144])
        by smtp-sink (smtp-sink) with ESMTP id 6f637299;
        Wed, 31 Aug 2022 04:27:53 +0000 (UTC)
Date: Wed, 31 Aug 2022 11:27:53 +0700
To: Joe User <joe@example.net>, ellen@example.com
From: Mailer <from@example.com>
Cc: cc@example.com
Reply-To: Information <info@example.com>
Subject: Here is the subject
Message-ID: <H4LZBUqyIo8cWjLp3rfudflU5ZMtnf6x84tn4Tf9i8@php7-dev>
X-Mailer: PHPMailer 6.6.4 (https://github.com/PHPMailer/PHPMailer)
MIME-Version: 1.0
Content-Type: multipart/alternative;
 boundary="b1_H4LZBUqyIo8cWjLp3rfudflU5ZMtnf6x84tn4Tf9i8"
Content-Transfer-Encoding: 8bit

This is a multi-part message in MIME format.

--b1_H4LZBUqyIo8cWjLp3rfudflU5ZMtnf6x84tn4Tf9i8
Content-Type: text/plain; charset=us-ascii

This is the body in plain text for non-HTML mail
clients

--b1_H4LZBUqyIo8cWjLp3rfudflU5ZMtnf6x84tn4Tf9i8
Content-Type: text/html; charset=us-ascii

This is the HTML message body <b>in bold!</b>


--b1_H4LZBUqyIo8cWjLp3rfudflU5ZMtnf6x84tn4Tf9i8--
```

#### example stdout:

```
kristian@php7-dev:/home/kristian$ smtp-sink -v -d %d.%H.%M.%S -u kristian -R /home/kristian/ 0.0.0.0:1025 10
smtp-sink: name_mask: all
smtp-sink: warning: protocols: disabling IPv6 name/address support: Address family not supported by protocol
smtp-sink: trying... [0.0.0.0]:1025
smtp-sink: connect (AF_INET 192.168.1.144)
smtp-sink: vstream_tweak_tcp: TCP_MAXSEG 32768
smtp-sink: fd=5: stream buffer size old=0 new=65536
smtp-sink: smtp_stream_setup: maxtime=100 enable_deadline=0
smtp-sink: EHLO php7-dev
smtp-sink: MAIL FROM:<from@example.com>
smtp-sink: RCPT TO:<joe@example.net>
smtp-sink: RCPT TO:<ellen@example.com>
smtp-sink: RCPT TO:<cc@example.com>
smtp-sink: RCPT TO:<bcc@example.com>
smtp-sink: DATA
smtp-sink: .
smtp-sink: QUIT
smtp-sink: disconnect
```
