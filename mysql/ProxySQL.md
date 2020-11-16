# ProxySQL

## Overview
- open source program for load balancing / proxy for MySQL
- cannot do failover (set new master when old master goes down), but will not send query to dead server
- usually used together with galera cluster / perconaDB
- open port 6032 (mysql protocol) for administration, open port 6033 (mysql protocol) for application (ex: php) to do query on mysql_servers behind proxysql

## Administration
- use systemctl to start/stop/restart proxysql service
- command to open proxysql admin: ```mysql -u admin -padmin -h 127.0.0.1 -P6032```
- configuration saved in 3 location:
    - disk = persistent storage with sqlite in /var/lib/proxysql/proxysql.db
    - memory = stored in RAM, where admin make changes
    - runtime = currently used
    - example: move from memory to runtime ```LOAD MYSQL USERS TO RUNTIME;```, move from memory to disk ```SAVE MYSQL VARIABLES TO DISK;```
- reset all configuration: stop service, delete sqlite file, then start service

## Experiment: 
- setup 1 master 1 slave on VM then benchmark with sysbench:
- setup hostgroup 1 for master (writer), 2 for master and slave (reader and writer) 
```sql
    INSERT INTO mysql_replication_hostgroups (writer_hostgroup, reader_hostgroup) VALUES (1,2);
    INSERT INTO mysql_servers (hostgroup_id, hostname, port) VALUES (1, '192.168.1.112', 3306);
    INSERT INTO mysql_servers (hostgroup_id, hostname, port) VALUES (2, '192.168.1.112', 3306);
    INSERT INTO mysql_servers (hostgroup_id, hostname, port) VALUES (2, '192.168.1.113', 3306);
    LOAD MYSQL SERVERS TO RUNTIME; SAVE MYSQL SERVERS TO DISK;
```
- setup mysql user to default to hostgroup 2 (reader)
```sql
    INSERT INTO mysql_users (username, password, active, default_hostgroup) VALUES ('root','123',1,2);
    LOAD MYSQL USERS TO RUNTIME; SAVE MYSQL USERS TO DISK;
```
- check if proxysql can connect to mysql backends
```sql
    UPDATE global_variables SET variable_value='root' WHERE variable_name='mysql-monitor_username';
    UPDATE global_variables SET variable_value='123' WHERE variable_name='mysql-monitor_password';
    LOAD MYSQL VARIABLES TO RUNTIME; SAVE MYSQL VARIABLES TO DISK;
    SELECT * FROM mysql_server_read_only_log;
```
- add query rule to always redirect write to hostgroup 1 (writer)
```sql
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(1,1,1,'INSERT',1,'CASELESS');
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(2,1,1,'UPDATE',1,'CASELESS');
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(3,1,1,'DELETE',1,'CASELESS');
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(4,1,1,'CREATE',1,'CASELESS');
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(5,1,1,'ALTER',1,'CASELESS');
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(6,1,1,'DROP',1,'CASELESS');
    LOAD MYSQL QUERY RULES TO RUNTIME; SAVE MYSQL QUERY RULES TO DISK;
```
- result: all query served by hostgroup 2 (reader), except query that contains INSERT/UPDATE/DELETE/CREATE/ALTER/DROP served by hostgroup 1 (writer)
- do benchmark
```bash
    sysbench --db-driver=mysql --mysql-user=root --mysql-password=123 --mysql-host=192.168.1.112 --mysql-port=6033 --mysql-db=replicated_db --range_size=1000000 --table_size=500000 --tables=4 --threads=4 --events=0 --time=60 --rand-type=uniform /usr/share/sysbench/oltp_read_only.lua prepare
```
- then run same command with replacing "prepare" with "run"
- another alternative for setting up rules is to make user's default hostgroup to writer, then add these rules to redirect SELECT (read) to reader
```sql
    INSERT INTO mysql_users (username, password, active, default_hostgroup) VALUES ('root','123',1,1);
    LOAD MYSQL USERS TO RUNTIME; SAVE MYSQL USERS TO DISK;
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(1,1,1,'^SELECT.*FOR UPDATE',1,'CASELESS');
    INSERT INTO mysql_query_rules (rule_id,destination_hostgroup,active,match_digest,apply,re_modifiers) VALUES(2,2,1,'^SELECT',1,'CASELESS');
    -- because SELECT ... FOR UPDATE will read then place lock on matching rows (read first but with write intent)
```
