using System.Net;

new AsyncSocketLib.AsynchronousClient().
//StartClient(Dns.GetHostName(), 11000);
StartClientReal(Dns.GetHostName(), 11000, "alex.pigida");
//StartClientReal("10.10.19.152", 6756, "alex.pigida");
