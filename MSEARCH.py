import socket
import httplib
import StringIO
import struct

class SSDPResponse(object):
    class _FakeSocket(StringIO.StringIO):
        def makefile(self, *args, **kw):
            return self
        
    def __init__(self, response):
        r = httplib.HTTPResponse(self._FakeSocket(response))
        r.begin()
        self.location = r.getheader("location")
        self.usn = r.getheader("usn")
        self.st = r.getheader("st")
        self.cache = r.getheader("cache-control").split("=")[1]
    def __repr__(self):
        return "<SSDPResponse({location}, {st}, {usn})>".format(**self.__dict__)

def discover(service, timeout=10, retries=1, mx=3):
    group = ("239.255.255.250", 1900)
    message = "\r\n".join([
        'M-SEARCH * HTTP/1.1',
        'HOST: {0}:{1}',
        'MAN: "ssdp:discover"',
        'MX: {mx}', 'ST: {st}','',''])
    socket.setdefaulttimeout(timeout)
    responses = {}
    
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM, socket.IPPROTO_UDP)

    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    sock.setsockopt(socket.IPPROTO_IP, socket.IP_MULTICAST_TTL, 2)
    sock.setsockopt(socket.IPPROTO_IP, socket.IP_MULTICAST_LOOP, 1)

    sock.bind(('192.168.122.179', 0))

    group_meh = socket.inet_aton("239.255.255.250")
    mreq = struct.pack('4sL', group_meh, socket.INADDR_ANY)
    sock.setsockopt(socket.IPPROTO_IP, socket.IP_ADD_MEMBERSHIP, mreq)
    
    for _ in range(retries):
        sock.sendto(message.format(*group, st=service, mx=mx), group)
        
    while True:
        try:
            response = SSDPResponse(sock.recv(1024))
            responses[response.location] = response
        except socket.timeout:
            break
    return responses.values()

discover("ssdbp:all", retries=3, mx=2)