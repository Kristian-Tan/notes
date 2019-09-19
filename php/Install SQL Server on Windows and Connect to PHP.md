# install XAMPP bundle (php7 + apache + mysql)

# install Microsoft SQL Server (download from Microsoft's official website)
    - very light version: Microsoft SQL Server LocalDB (free)
    - light: Microsoft SQL Server Express (free)
    - normal: Microsoft SQL Server (trial)
note: Windows 7 will only work with Microsoft SQL Server 14

# start a database app (for LocalDB only)
```
    cd C:\Program Files\Microsoft SQL Server\120\Tools\Binn\
    SqlLocalDB create LocalDBApp1 // create new db app
    SqlLocalDB start LocalDBApp1 // start the db app
    SqlLocalDB info LocalDBApp1 // view info, take note of "Instance pipe name"
```

# install Microsoft SQL Server Management Studio (download free from Microsoft's official website)
    connect to L:ocalDB: copy paste "Instance pipe name" to server name, select "Windows authentication"

# download php driver to connect to Microsoft SQL Server
- first, check which driver version to download
https://docs.microsoft.com/en-us/sql/connect/php/system-requirements-for-the-php-sql-driver
- download the driver with version above
https://docs.microsoft.com/en-us/sql/connect/php/step-1-configure-development-environment-for-php-development
https://docs.microsoft.com/en-us/sql/connect/php/download-drivers-php-sql-server

- execute the downloaded installer, then point to php extension path (ex: C:\xampp\php\ext)

- change php.ini (ex: C:\xampp\php\php.ini)
add this line: "extension=php_sqlsrv_71_ts_x86.dll"

explanation: 
    - 71 means for php7 version 7.1 (other options: 71, 72)
    - ts means thread safe version (other options: nts, ts)
    - x86 means the architecture (other options: x86, x64)

# restart apache



# troubleshooting: module not loaded
- detection: run phpinfo(), then check if sqlsrv module listed or not
- detection: run php from command line, then check if any error message displayed
- solution: might be running a wrong version of the driver, change php.ini where sqlsrv driver is loaded
    -- check if the php version is right
    -- if ts driver failed, try nts
    -- if x64 driver failed, try x86