# MySQL replication:

## Types
- classic 1 master many slave configuration
- master-master configuration
- mysql group replication (tidak support mariaDB, hanya untuk mysql)
    - menggunakan protocol Paxos untuk mencapai kesepakatan diantara banyak server
    - https://www.cs.rutgers.edu/~pxk/417/notes/paxos.html
    - https://en.wikipedia.org/wiki/Paxos_(computer_science)
- innoDB cluster
- galera cluster (mariaDB)

## MySQL replication: classic 1 master many slave
- in master configuration file (restart mysql service after changing):
```bash
    bind-address = 192.168.1.112
    server-id = 1
    log-bin = /var/log/mysql/mysql-bin.log
    binlog-do-db = replicated_db
```
- in master mysql console:
```sql
    GRANT REPLICATION SLAVE, REPLICATION CLIENT ON *.* TO 'slave_user'@'%' IDENTIFIED BY 'password'; FLUSH PRIVILEGES;
    FLUSH TABLES WITH READ LOCK;
    -- do mysqldump here, then import the dump in slave server
    SHOW MASTER STATUS; -- note binary log file name and log position, then do mysqldump
    UNLOCK TABLES;
```
- in slave configuration file (restart mysql service after changing):
```bash
    bind-address = 192.168.1.113
    server-id = 2
    relay-log = /var/log/mysql/mysql-relay-bin.log
    log-bin = /var/log/mysql/mysql-bin.log
    binlog-do-db = replicated_db
```
- in slave mysql console:
```sql
    -- import mysqldump from server
    CHANGE MASTER TO 
        MASTER_HOST='192.168.1.112',
        MASTER_USER='slave_user', 
        MASTER_PASSWORD='password', 
        MASTER_LOG_FILE='mysql-bin.000001', 
        MASTER_LOG_POS=107;
    START SLAVE;
    SHOW SLAVE STATUS \G;
```
- terkadang terjadi masalah pada query yang dijalankan di slave sehingga stuck (berhenti replikasi), solusinya bisa drop slave lalu setting ulang replikasi, atau skip eksekusi query yang bermasalah tadi (akan menyebabkan data di master dan slave berbeda) dengan query: ```STOP SLAVE; SET GLOBAL SQL_SLAVE_SKIP_COUNTER=1; START SLAVE;```

## Troubleshooting mysql berhenti replikasi https://www.ryadel.com/en/replication-stops-working-analysis-resync-mysql-replication/
- replikasi mysql terjadi berdasarkan logging di master, dimana jika ada operasi write ke master, maka master akan menyimpan query yang menyebabkan perubahan tersebut (contohnya INSERT/UPDATE/DELETE), kemudian slave akan membaca log tersebut dan diaplikasikan ke database slave
- apabila data di slave dan master berbeda belum tentu menyebabkan replikasi berhenti (bisa silent error), contohnya apabila ada update kolom 'nama' di slave, tetapi jika perubahan tersebut menyebabkan error (misal duplicate primary key) maka replikasi berhenti
- apabila jaringan master dan slave terputus, slave bisa otomatis melanjutkan sinkronisasi ketika koneksi tersambung kembali
 
## MySQL binary log:
- log terhadap apa yang terjadi pada database, contohnya: terjadi query insert/update/delete (statement based logging), terjadi perubahan pada baris tertentu (row based logging)
- diperlukan untuk replikasi master-slave (untuk memastikan perubahan di master juga dilakukan terhadap slave)
- set parameter ```bin-log``` ke nama file / path target binary log untuk menyalakan fitur binary logging
- gunakan ```mysqlbinlog``` untuk melihat log tersebut

