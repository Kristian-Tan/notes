# pg_hba.conf

- https://www.postgresql.org/docs/9.1/auth-pg-hba-conf.html
- default location: ```/var/lib/postgres/data/pg_hba.conf``` (inside postgres data directory)
- control which host allowed to connect, which postgre user they can use, what db they can access, what auth method
- when using command line interface (ex: psql, pg\_dump, pg\_basebackup) 
    - flag "-U username", if not specified: default to UNIX username
    - flag "-h hostname", if not specified: default to localhost
    - flag "-w" (no password)/"-W" (force input password), if not specified: 
        - default to global variable PGPASSWORD or 
        - from ~/.pgpass (file format: ```host:port:db:user:pass```, separated by newline)
    - flag "-d databasename"
    - flag "-p portnumber", if not specified: default to 5432



## postgresql allow remote psql/terminal access:
- edit file ```/var/lib/postgres/data/postgresql.conf``` (or file pointed by ```SHOW config_file``` query)
- change listen_addresses to '\*'
- edit file ```/var/lib/postgres/data/pg_hba.conf``` (or hba file pointed by postgresql.conf)
- add this line: ```host all all 0.0.0.0/0 trust``` (insecure: allow all host to login into psql terminal without password)
- restart postgresql service ```systemctl restart postgresql```
- connect remotely: ```psql -h 192.168.1.112 -U root -d mydb```
- simple commands: 
    - \l = SHOW DATABASES, 
    - \dt = SHOW TABLES, 
    - \d tablename = DESC tablename, 
    - \c dbname = USE dbname 
