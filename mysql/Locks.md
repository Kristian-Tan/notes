# MySQL table locks:

- READ LOCK: 
    - "ada process yang sedang membaca tabel ini; sebelum process tersebut selesai membaca, jangan diubah dulu tabelnya", 
    - session pemilik lock bisa read, tidak bisa write; 
    - session lain bisa read, operasi write masuk antrian

- WRITE LOCK: 
    - "ada process yang sedang mengubah tabel ini; sebelum process tersebut selesai menulis, tabel ini jangan dibaca/ditulisi dulu (tunggu process yang sedang mengubah tadi selesai)", 
    - session pemilik lock bisa read/write; 
    - session lain operasi read/write masuk antrian 
