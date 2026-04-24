# PROXMOX ACL

ref: https://pve.proxmox.com/wiki/User_Management

## Concepts
- user = individual users that can log into the system, e.g.: `john_doe`
- group = grouping of users (one group can contain multiple users, one user can have multiple group, so M:N / many-to-many of user and group), e.g.: `sysadmins`
- privilege = right to perform an action, e.g.: `VM.Allocate`, `Sys.Modify`
- role = grouping of privilege, e.g.: `Administrator`, `PVEAuditor` (these examples are predefined roles)

## user
- can be authenticated with several mechanism:
    - PAM: using local linux user password (like root user)
    - LDAP: ...
    - OpenIDConnect: ...
- create: `pveum user add user1@pam`

## group
- to group users, privilege should be given to group instead of user
- create: `pveum group add group1`
- set a user to group: `pveum user modify user1@pam -group group1,group2`

## role
- role is group of privilege
- assign role to group: `pveum acl modify / -group group1 -role Administrator`
