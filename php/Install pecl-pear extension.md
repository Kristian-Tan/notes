# Install PECL/PEAR Extension
- install php (from package manager or compile from source)
    - make sure you have 'php' and 'phpize' executable in PATH (check with command: `which php` and `which phpize`)
- install pear
    - `wget http://pear.php.net/go-pear.phar`
    - `php go-pear.phar`
    - you will be asked (interactive terminal) on where to install
    - follow instruction in interactive terminal
    - add the installation ./bin directory to PATH
    - check if installation successful: `which pear` and `which pecl`
- install `autoconf` (use package manager)
- install package: `pecl install xdebug`
- include installed extension in php.ini
- use proxy: `pear config-set http_proxy http://localhost:3128`

## Example
- xdebug
    - latest package: `pecl install xdebug`
    - certain version: `pecl install xdebug-3.1.6` (for example for php-7.4)
- memcached
    - install dependency: `apt install libmemcached-dev`
    - 