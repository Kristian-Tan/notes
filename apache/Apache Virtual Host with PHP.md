- open httpd-vhosts.conf
- add this entry:
```
<VirtualHost *:80>
	ServerAdmin webmaster@localhost
	DocumentRoot "C:/path/to/your/htdocs/"
	ServerName dev.localhost
	ServerAlias www.dev.localhost
	ErrorLog "logs/dev.localhost-error.log"
	CustomLog "logs/dev.localhost-access.log" combined

	<Directory "C:/path/to/your/htdocs/">
		Options Indexes FollowSymLinks
		AllowOverride All
		Order allow,deny
		Allow from all
		Require all granted
		
		AddHandler application/x-httpd-php .php
		AddType application/x-httpd-php .php
		<FilesMatch "\.php$">
			SetHandler application/x-httpd-php
		</FilesMatch>
		<FilesMatch "\.phps$">
			SetHandler application/x-httpd-php-source
		</FilesMatch>
	</Directory>
	
	AddHandler application/x-httpd-php .php
	AddType application/x-httpd-php .php
	<FilesMatch "\.php$">
		SetHandler application/x-httpd-php
	</FilesMatch>
	<FilesMatch "\.phps$">
		SetHandler application/x-httpd-php-source
	</FilesMatch>
</VirtualHost>
```
