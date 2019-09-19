1. install xampp
2. add php directory to PATH
3. test if php correctly installed: run command
```php -v```
4. install composer (go to https://getcomposer.org/download/ , use .exe installer or php command)
5. add composer directory to PATH
6. create composer.bat file (see other note) that points to composer.phar
7. test if composer correctly installed: run command
```composer -V```
8. there are 2 ways to install laravel: global environment vs per project basis (see below)



# global environment:
1. open cmd, run command:
```composer global require "laravel/installer"```
2. see where laravel installer is installed (example: C:\Users\username_here\AppData\Roaming\Composer\vendor\laravel\installer\)
3. add composer/vendor/bin to PATH
4. create laravel.bat file (see other note) that point to composer/vendor/laravel/installer/laravel
5. test if laravel installer correctly installed: run command
```laravel -V```
6. to create new laravel project, run command:
```laravel new project_name_here```


# per project basis:
1. open cmd, cd to directory to create project in
2. to create new laravel project, run command:
```composer create-project laravel/laravel project_name_here "5.1.*"```