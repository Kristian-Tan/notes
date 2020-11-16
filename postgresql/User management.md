# PostgreSQL User Management

- user is equal with role; as in a role have zero/one/many privileges (so does user)
- role can be a member/child of other role (ex: ```GRANT parentrolehere TO childrolehere```)
- user is a term of role with LOGIN privilege
- privileges: 
    - LOGIN (can be used to login, a.k.a. user), 
    - SUPERUSER (bypass all check except login privilege check), 
    - CREATEDB, 
    - CREATEROLE, 
    - REPLICATION
- deleting role need to remove/reassign owned items (ex: database) 
```sql
REASSIGN OWNED BY doomed_role TO successor_role; 
DROP OWNED BY doomed_role; 
DROP ROLE doomed_role;
``` 

## Example:
- [console] createuser -U postgres -P -c 5 --replication newusernamehere
    - flag -U postgres = do this operation as ```postgres```
    - flag -P = prompt password for new user
    - flag -c 5 = connection limit max 5 (default unlimited)
    - flag --replication = grant replication privilege
- [phppgadmin] select server name in tree (left panel, ex: "localhost"), select roles tab, select "create role" (in phppgadmin, role=user)
