example case: file called composer.phar, located in c:/xampp/composer

1. add the file's directory to PATH variable
2. do not add the file's extension to PATHEXT variable
3. create a new file with the same name with .php file, but with .bat extension
4. edit the .bat file, change its content to

```
@ECHO OFF
setlocal DISABLEDELAYEDEXPANSION
SET BIN_TARGET=%~dp0/php_or_phar_file_name_here
php "%BIN_TARGET%" %*
```

example: file name = c:/xampp/composer/composer.phar

```
@ECHO OFF
setlocal DISABLEDELAYEDEXPANSION
SET BIN_TARGET=%~dp0/composer.phar
php "%BIN_TARGET%" %*
```