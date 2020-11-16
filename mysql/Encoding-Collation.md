# MySQL encoding and collation:


## character set vs collation:
- encoding/character set: bagaimana encode huruf/karakter jadi bytes (cth: ascii, utf8)
- collation: bagaimana sorting huruf/karakter?? contoh collation yang ber-suffix \_ci artinya case insensitive (ketika string comparison pada WHERE/LIKE misalnya), sedangkan \_cs artinya case sensitive dan \_bin artinya binary

## utf8 vs utf8mb4
- mysql punya utf8 (encoding utf8 dengan maksimal 3byte) dan utf8mb4 (encoding utf8 dengan maksimal 4byte)
- mysql punya limit sebuah index tidak boleh lebih panjang dari 767byte, karena itu jika ada primary key yang menggunakan varchar utf8mb4 harus dibatasi 191 karakter (191char x 4byte/char = 764byte), sedangkan jika menggunakan utf8 (maksimal 3byte) primary key harus dibatasi 255 karakter (255char x 3byte/char = 765byte)

## why each schema, each table and each column have their own character set and collation:
- when creating new column and charset/collation on column level is not set, then it will inherit charset/collation from table
- when creating new table and charset/collation on table level is not set, then it will inherit charset/collation from schema
- there is no "dynamic" inheritance at "run time", inheritance only done when the table or column is created
     
