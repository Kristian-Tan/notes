# QEMU/KVM Networking

## Default: user mode networking
- source: https://wiki.qemu.org/index.php/Documentation/Networking
- when no networking configuration passed in command line, QEMU will default to user mode networking
- guest is not directly accessible from the host or the external network (as if the guest is behind NAT)
- virtual machine guest will have ```ens3``` or ```eth0``` as its NIC
- virtual machine guest will get IP address of 10.0.2.15/24 (from DHCP)
- gateway is 10.0.2.2 (also localhost of host OS)
- DNS is 10.0.2.3

## TUN/TAP Bridged Networking
- source: https://serverfault.com/questions/152363/bridging-wlan0-to-eth0
- assume host connected with wifi on NIC wlo1, ip address = 172.16.0.226/24
- define a subnet to be used by virtual machine guests (lets call it virtual subnet), ex = 192.168.1.0/24
- define ip address for host and guests inside virtual subnet, ex = host 192.168.1.101/24, guest 1 = 192.168.1.102/24, guest 2 = 192.168.1.103/24
- create network bridge for traffic between virtual machines: 
```bash
sudo ip link add name bridge0 type bridge
sudo ip link set bridge0 up
```
- set ip address on host's bridge interface inside virtual subnet: ```sudo ip addr add 192.168.1.101/24 dev bridge0```
- set routing/nat/masquerade from bridge0 to internet/WAN
    - enable ipv4 forwarding: edit file ```/proc/sys/net/ipv4/ip_forward```, set content to ```1```
    - set iptables:
    ```bash
    iptables --flush # flush / delete all rule
    iptables --table nat --flush # delete all rule inside NAT table
    iptables --delete-chain # delete optional user defined chain
    iptables --zero # zero the packet and byte counters in all chains
    iptables --policy INPUT ACCEPT # set policy for chain INPUT to ACCEPT
    iptables --policy OUTPUT ACCEPT # set policy for chain OUTPUT to ACCEPT
    iptables --policy FORWARD ACCEPT # set policy for chain FORWARD to ACCEPT
    iptables --table nat --append POSTROUTING --out-interface wlo1 --source 192.168.1.0/24 --jump MASQUERADE # add rule in table "nat", rule "postrouting", set out interface to wlan0, match only if source from 192.168.1.0/24, then do masquerade
    ```
- run first guest with qemu: 
```bash
    qemu-system-x86_64 -enable-kvm \
    -vga std \
    -smp 8,cores=4 -m 4G \
    -boot d -cdrom myiso-x86_64.iso \
    -net nic,macaddr='52:54:00:12:34:59' -net bridge,br=bridge0 \
    -name "First guest" $*
```
- run second guest with qemu: 
```bash
    qemu-system-x86_64 -enable-kvm \
    -vga std \
    -smp 8,cores=4 -m 1536 \
    -hda "mydisk.vmdk" \
    -boot once=c,menu=off \
    -net nic,macaddr='52:54:00:12:34:58' -net bridge,br=bridge0 \
    -name "Second guest" $*
```
- make sure ```macaddr``` (MAC address) of QEMU instances are unique, and make sure that ```macaddr``` starts with 52:54 (qemu's code)
- set ip address of both virtual machine: ```sudo ip addr add 192.168.1.102/24 dev eth0``` (in first guest), ```sudo ip addr add 192.168.1.103/24 dev eth0``` (in second virtual guest)
- set default gateway of both virtual machine: ```sudo ip route add default via 192.168.1.101 dev eth0``` (in guest)
- currently, the network should look like this:
    - host, interface wlo1 172.16.0.226/24 gateway 172.16.0.1 (to internet)
    - host, interface bridge0 192.168.1.101/24 (have slave tap0 and tap1)
    - guest 1, interface eth0 192.168.1.102/24 gateway 192.168.1.101
    - guest 2, interface eth0 192.168.1.103/24 gateway 192.168.1.101 
- test ping from virtual machine to host, from virtual machine to other virtual machine, and from host to virtual machine

### Scripted Shortcut
- in host:
```bash
# configure bridge
ip link add name bridge0 type bridge
ip link set bridge0 up
ip addr add 192.168.1.101/24 dev bridge0
# enable ip forward
echo "1" > /proc/sys/net/ipv4/ip_forward
# add iptables rule for ip forward
iptables --flush # flush / delete all rule
iptables --table nat --flush # delete all rule inside NAT table
iptables --delete-chain # delete optional user defined chain
iptables --zero # zero the packet and byte counters in all chains
iptables --policy INPUT ACCEPT # set policy for chain INPUT to ACCEPT
iptables --policy OUTPUT ACCEPT # set policy for chain OUTPUT to ACCEPT
iptables --policy FORWARD ACCEPT # set policy for chain FORWARD to ACCEPT
iptables --table nat --append POSTROUTING --out-interface wlo1 --source 192.168.1.0/24 --jump MASQUERADE # add rule in table "nat", rule "postrouting", set out interface to wlan0, match only if source from 192.168.1.0/24, then do masquerade
```
- in guest:
```bash
ip link set dev ens3 up
ip addr add 192.168.1.142/24 dev ens3
ip route add default via 192.168.1.101 dev ens3
```

### Autoconfigured IP Address Via DHCP
- in host: install dhcpd (dhcp daemon, might be in package 'dhcp'/'isc-dhcp-server')
- configure /etc/dhcpd.conf, example:
```
option domain-name "mylan";
option domain-name-servers 8.8.8.8;
default-lease-time 3600;
max-lease-time 7200;
authoritative;
subnet 192.168.1.0 netmask 255.255.255.0 {
        option routers                  192.168.1.101;
        option subnet-mask              255.255.255.0;
        option domain-search            "mylan";
        option domain-name-servers      8.8.8.8;
        range   192.168.1.201   192.168.1.254;
}
subnet 172.16.0.0 netmask 255.255.255.0 {
}
```
- start dhcpd4 service: ```sudo systemctl start dhcpd4```
