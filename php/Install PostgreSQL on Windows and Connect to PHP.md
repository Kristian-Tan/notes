# install XAMPP bundle (php7 + apache + mysql)

# download postgresql installer:
https://www.labkey.org/Documentation/wiki-page.view?name=installPostgreSQLWindows
http://www.enterprisedb.com/products-services-training/pgdownload#windows

# install the downloaded installer, you will be asked a password, take note of the password

# select default port 5432, default username 'postgres'

# configure php to connect to postgre:
- open xampp/php/php.ini, uncomment (remove ';' from the front) these lines:
```
extension=php_pdo_pgsql.dll
extension=php_pgsql.dll
```

# download phpPgAdmin
- for php lower than version 5 : cannot use phpPgAdmin, update your php version
- for php older than version 7 : http://phppgadmin.sourceforge.net/doku.php?id=download
- for php version 7 : http://soft.ge/phppgadmin-and-php7/
- then extract your downloaded phpPgAdmin to /xampp/htdocs/phpPgAdmin

- configure phpPgAdmin, open /phpPgAdmin/conf/config.inc.php
- configure server address and port number (optional)
```php
	$conf['servers'][0]['host'] = '127.0.0.1';
	$conf['servers'][0]['port'] = 5432;
```
- configure SSL if supported by server, set 'disable' if not supported, 'allow' if supported
```php
	$conf['servers'][0]['sslmode'] = 'disable';
```
- configure path (because it's created to work on linux)
```php
	$conf['servers'][0]['pg_dump_path'] = 'C:\\Program Files\\PostgreSQL\\9.6\\bin\\pg_dump.exe';
	$conf['servers'][0]['pg_dumpall_path'] = 'C:\\Program Files\\PostgreSQL\\9.6\\bin\\pg_dumpall.exe';
```
- configure security
```php
	$conf['extra_login_security'] = false;
```
- try login in 127.0.0.1/phppgadmin
-- user: postgres
-- pass: <your installation password>




NOTE:

- phpPgAdmin is outdated, use PgAdmin 4 instead (it's a desktop app that will create it's own webserver with better interface and integration)

- if an error occurred "PHP Startup error: Unable to load dynamic library php_pgsql.dll not founf" or "not a valid win32 application"
1. maybe pgsql binary directory have not been registered to PATH environment variables yet
solution: add pgsql bin directory to PATH environment variables
2. maybe xampp/apache/php is running as 32bit application while postgresql is running as 64bit application (or the other way around)
solution if xampp/apache/php is 32bit and postgresql is 64bit: download 32bit postgresql binaries from postgresql official website (as .zip bin package), then extract all file in bin with extension .dll to Windows directory (C:\Windows) and to Apache binary directory (C:\xampp\apache\bin)