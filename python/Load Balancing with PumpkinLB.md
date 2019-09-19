1. install python2 atau python3 (sebaiknya salah satu saja)
2. add lokasi python.exe ke PATH, add lokasi Python/Scripts ke PATH
3. install dengan perintah
```pip install PumpkinLB```
4. dari command prompt, cd ke Python/Scripts
5. buat file config.cfg di Python/Scripts berisi:
```
[mappings]
localhost:80=192.168.1.3:80,192.168.1.4:80,192.168.1.5:80
```
6. execute perintah python PumpkinLB.py config.cfg


NOTE: format config.cfg adalah loadbalancer_addr:loadbalancer_port=worker1_addr:worker1_port,worker2_addr:worker2_port,worker3_addr:worker3_port,...
- jadi di contoh tersebut, asumsikan ada jaringan sebagai berikut:
- 192.168.1.2 = laptop anda (jadi load balancer, tidak menjalankan xampp/apache)
- 192.168.1.3-5 = laptop worker (hanya menjalankan xampp/apache di port 80, berisi web statis tanpa database)
- nanti jika ada yang mau akses web tersebut, dia buka http://192.168.1.2, request tersebut akan diputar ke 192.168.1.3-5 secara round robin

troubleshooting:
- X is not recognized as an internal or external command, operable program or batch file. ==>> belum install program yang dibutuhkan, atau belum tambah PATH
- python: can't open file 'pumpkinlb.py': [Errno 2] No such file or directory ==>> belum cd ke lokasi Python/Scripts atau belum pip install PythonLB
- Traceback ... ImportError: No module named pumpkinlb ==>> belum pip install PythonLB, atau versi python yang diinstall lewat pip tadi beda (misalnya menginstall python2 dan python3 sekaligus)