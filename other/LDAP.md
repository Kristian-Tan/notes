LDAP = lightweight directory access protocol
- spesialisasi untuk read berkali-kali data yang jarang di-update (cth: daftar user, addressbook)
Attribute
- seperti relational database column / relational database entity attribute
- cth:
    mail: admin@example.com
-
Entry
- seperti relational database row / relational database record
- kumpulan banyak atribut, dibawah suatu nama
- cth:
```
    dn: sn=Ellingwood,ou=people,dc=digitalocean,dc=com
    objectclass: person
    sn: Ellingwood
    cn: Justin Ellingwood
```
-
STRUKTUR DATA:
Data Information Tree (DIT)
- seperti directory tree untuk mengelompokkan data (membedakan apakah ini suatu entry yang berisi informasi "people", atau "inventory" atau apa)
- cth:
    dn: sn=Ellingwood,ou=people,dc=digitalocean,dc=com
- di contoh itu terlihat bahwa bentuk treenya: com -> digitalocean -> people
ObjectClass
- seperti sebuah class di OOP, atau seperti entity di RDBMS
- memiliki banyak attribute
- cth definition:
```
    objectclass ( 2.5.6.6 NAME 'person' DESC 'RFC2256: a person' SUP top STRUCTURAL
        MUST ( sn $ cn )
        MAY ( userPassword $ telephoneNumber $ seeAlso $ description ) )
```
- arti: ObjectClass person harus memilik attribute sn dan cn, attribute lain seperti userPassword, telephoneNumber, seeAlso dan description dianggap optional
- nomor-nomor (seperti di contoh: 2.5.6.6) adalah OID (object identifier)
Attribute
- seperti properti dari ObjectClass di OOP, atau seperti column/attribute suatu tabel di RDBMS
- cth definition:
```
    attributetype ( 2.5.4.41 NAME 'name'
            DESC 'RFC4519: common supertype of name attributes'
            EQUALITY caseIgnoreMatch
            SUBSTR caseIgnoreSubstringsMatch
            SYNTAX 1.3.6.1.4.1.1466.115.121.1.15{32768} )
    attributetype ( 2.5.4.4 NAME ( 'sn' 'surname' )
            DESC 'RFC2256: last (family) name(s) for which the entity is known by' SUP name )
    attributetype ( 2.5.4.4 NAME ( 'cn' 'commonName' )
            DESC 'RFC4519: common name(s) for which the entity is known by' SUP name )
```
-
Schema
- kumpulan ObjectClass definition dan Attribute definition
Singkatan:
- dn: distinguished name (seperti primary key di RDBMS)
- cn: common name
- sn: surname (nama keluarga)
- dc: domain components
- rdn: relative distinguished name
Contoh Domain Components:
- catatan: semakin di depan (di kiri) suatu properti, maka semakin spesifik, sebaliknya semakin di belakang (di kanan) suatu properti, maka semakin luas
    - cth: ou=people,dc=ubaya,dc=ac,dc=id
    - artinya id (root) -> ac (branch) -> ubaya (branch) -> people (leaf)
- dc: domain components
    - cth: dc=ubaya,dc=ac,dc=id
- l: location, c: country
    - cth: l=new_york,c=us
- ou: organizational unit, o: organization
    - cth: ou=marketing,o=Example_Co
-
Resource Locator
- Distinguished Name (dn) dan Relative Distinguished Name (rdn) digunakan untuk identifikasi entry
- dn seperti absolute path
    - cth: dn: uid=jsmith1,ou=People,dc=example,dc=com
    - anggap saja url: http://example.com/People/jsmith1
- rdn seperti relative path
    - cth: dn: uid=jsmith1
    - anggap saja url relatif: ./jsmith1
-
Inheritance
- seperti konsep Inheritance di OOP
- satu ObjectClass bisa inherit multiple parent ObjectClass
- keyword: SUP NamaParentObjectClass STRUCTURAL/AUXILIARY
- tiap ObjectClass harus inherit sesuatu, yang paling atas: class 'top'
- cth:
```
    objectclass ( 2.5.6.7 NAME 'person' SUP top STRUCTURAL
    objectclass ( 2.5.6.7 NAME 'organizationalPerson' SUP person STRUCTURAL
    objectclass ( 2.5.6.7 NAME 'inetOrgPerson' SUP organizationalPerson STRUCTURAL
```
- arti: inetOrgPerson -> organizationalPerson -> person -> top
Variasi Protocol LDAP:
- ldap://: (standar)
- ldaps://: (LDAP over SSL/TLS, deprecated) Normal LDAP traffic is not encrypted. STARTTLS encryption is recommended instead
- ldapi://: (LDAP over IPC / internal socket, secure)