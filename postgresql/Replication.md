# PostgreSQL Replication

- turn off postgresql service (like mysql lock table) in both master and slave
- in master, ```postgresql.conf```:
```bash
listen_address='*' # listen to all (like listen to 0.0.0.0)
wal_level=replica # note: level 'minimal','replica'/'hot_standby'/'archive','logical'
checkpoint_segments=8 # note: deprecated in latest postgresql
archive_mode=on # enable archiving WAL (write ahead log)
archive_command='cp %p /var/lib/postgres/archive/%f' # shell command to execute to archive completed WAL file segment; $p replaced with path of file to archive, %f replaced with destination file name; note that directory must exist and owned by 'postgres' user (and group)
# archive_command='cp -i %p /var/lib/postgres/archive/%f < /dev/null' # or use this to make cp command never overwrite existing file (and ignore error)
max_wal_senders=3 # maximum number of process to send WAL from master to slave
wal_keep_segments=32 # maximum number of retained WAL segment (each segment=16mb)
synchronous_commit=local # local -> async/lazy replication, on -> sync/eager replication (transaction have to wait until all slave finished)
```
- in master, configure ```pg_hba.conf``` to allow slave user to connect to master server, 
    - ex: add line ```host replication all 192.168.1.113/32 trust```
- in slave, copy master's database: 
```bash
pg_basebackup -h 192.168.1.112 -D /var/lib/postgres/data -U replica # copy/dump/backup master's database files to slave's data directory, this will copy all database
```
- in slave, ```recovery.conf``` (inside postgresql data directory) OR in ```postgresql.conf``` (for latest version of postgresql):
```bash
standby_mode=on # declare as slave; it means the slave server is read only (trying to update/insert/delete/transaction/create/lock/etc. will throw error)
hot_standby=on # declare as slave; it means the slave server is read only (trying to update/insert/delete/transaction/create/lock/etc. will throw error)
primary_conninfo='host=... port=5432 user=... password=...' # define connection to master
restore_command='cp /var/lib/postgres/archive/%f %p' # command to execute to retrieve an archived segment of the WAL file, opposite of archive_command
promote_trigger_file='/tmp/postgres.5432.trigger' # optional, may also be called 'trigger_file' in older version, if this file exist will turn off standby mode (to promote slave to master when doing failover), to promote manually: 'pg_ctl promote'
```
- create empty file ```standby.signal``` inside postgre data directory on slave server
- check if replication work (use '\x' to print one line per column):
```sql
select * from pg_stat_replication; -- in master
select * from pg_stat_wal_receiver; -- in slave
```
- benchmark OLTP (online transaction processing) with sysbench:
```bash
sysbench --db-driver=pgsql --oltp-table-size=500000 --oltp-tables-count=4 --threads=1 --pgsql-host=192.168.1.112 --pgsql-port=5432 --pgsql-user=postgres --pgsql-password=123 --pgsql-db=sbtest /usr/share/sysbench/tests/include/oltp_legacy/parallel_prepare.lua run # insert test/prepare data
sysbench --db-driver=pgsql --report-interval=2 --oltp-table-size=500000 --oltp-tables-count=4 --threads=16 --time=30 --pgsql-host=192.168.1.112 --pgsql-port=5432 --pgsql-user=postgres --pgsql-password=123 --pgsql-db=sbtest /usr/share/sysbench/tests/include/oltp_legacy/oltp.lua run # read-write benchmark
```     

## Postgres WAL (write-ahead log):
- log terhadap apa yang terjadi pada file database
- gunakan ```pg_waldump``` untuk melihat log tersebut, umumnya tersimpan di ```/var/lib/postgresql/data/pg_wal/000...```
        
