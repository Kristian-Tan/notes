# Resource Monitoring Tools

## General Tools
- [CLI] [CPU] [MEM] [PROC] top / htop
- [CLI] [CPU] [MEM] [PROC] [NET] [DISK] nmon

## Database Monitoring
- [CLI] [MYSQL] mytop
    - from official repository, perl script to monitor mysql resource usage
    - usage: ```mytop -u root -p```
    - show help inside mytop: type ```?```
    - requires ```perl```, ```perl-dbd-mysql```; might need to edit ```/usr/bin/mytop```, replace text "MariaDB" to "mysql" if error occured
    - to test test, run query: ```SELECT SLEEP(10)```
- [CLI] [POSTGRES] pg_activity
    - https://github.com/dalibo/pg_activity
    - usage: ```sudo -u postgres pg_activity -U postgres```
    - python script (available in AUR) to monitor postgresql usage (note: need to ```pip install psycopg2-binary```)
    - to test test, run query: ```SELECT pg_sleep(10)```
