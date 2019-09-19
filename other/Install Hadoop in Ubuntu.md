1. Install java/jdk (for experiment: jdk 1.8.0_144), add to path (check with java -version)
2. Download hadoop binaries at http://www.apache.org/dyn/closer.cgi/hadoop/common/
3. Extract to /usr/local/hadoop/hadoop-3.2.0
4. Add to path: /usr/local/hadoop/hadoop-3.2.0/bin (nano /etc/environment)
5. Add your own ssh public key as trusted: cp ~/.ssh/id_rsa.pub ~/.ssh/authorized_keys
6. format namenode (for HDFS), from HADOOP_HOME, run: bin/hdfs namenode -format
7. start all hadoop services, from HADOOP_HOME, run: sbin/start-all.sh
8. Open dashboard: localhost:8088