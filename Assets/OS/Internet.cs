using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Internet emulator
// holds fake servers for attacks
public class Internet : MonoBehaviour
{
    public Dictionary<string, string> DNSRecords; // domain. ip // multiple domain can point to the same ip
    public class Server
    {
        public abstract class Service
        {
            public string name;
            public string version; // 1.0.3

            public Service(string name, string version)
            {
                this.name = name;
                this.version = version;
            }

        }

        public class SSH : Service
        {
            Dictionary<string, string> loginCredentials;
            public SSH(string name, string version) : base(name, version)
            {
                loginCredentials = new Dictionary<string, string>();
                loginCredentials.Add("root", "toor");
            }
            public bool Login(string login, string password)
            {
                if (!loginCredentials.ContainsKey(login))
                    return false;
                if (loginCredentials[login] != password)
                    return false;
                return true;
            }
        }

        public class HTTPS : Service
        {
            public bool heartbleedable;
            public HTTPS(string name, string version, string sslVersion) : base(name, version)
            {
                // 1.0.1 - 1.0.1g
                if (sslVersion.Contains("1.0.1") && !sslVersion.Contains("f"))
                {
                    heartbleedable = true;
                }
                else heartbleedable = false;
            }
        }

        Dictionary<int, Service> services; // port, Service

        public Server()
        {
            services = new Dictionary<int, Service>();
        }

        public void StartService(int port, Service service)
        {
            services.Add(port, service);
        }

        public bool HasService(string serviceName)
        {
            foreach (KeyValuePair<int, Service> s in services)
            {
                if (s.Value.name == serviceName)
                    return true;
            }
            return false;
        }

        public Service GetService(string serviceName)
        {
            foreach (KeyValuePair<int, Service> s in services)
            {
                if (s.Value.name == serviceName)
                    return s.Value;
            }
            return null;
        }

        public Dictionary<int, Service> GetServices()
        {
            return services;
        }

    }
    Dictionary<string, Server> servers; // ip, Server
    void Awake()
    {
        servers = new Dictionary<string, Server>();
        DNSRecords = new Dictionary<string, string>();
    }

    public Server StartServer(string ip)
    {
        Server s = new Server();
        if (PingServer(ip))
        {
            KillServer(ip);
        }
        servers.Add(ip, s);
        return s;
    }

    public void KillServer(string ip)
    {
        servers.Remove(ip);
    }

    public bool PingServer(string ip)
    {
        return servers.ContainsKey(ip);
    }

    public Server GetServer(string ip)
    {
        if (PingServer(ip))
        {
            return servers[ip];
        }
        return null;
    }

    public void RegisterDomain(string ip, string domainName)
    {
        if (DNSRecords.ContainsKey(domainName))
            return;
        DNSRecords.Add(domainName, ip);
    }

    public void RemoveDomain(string domainName)
    {
        DNSRecords.Remove(domainName);
    }

    public string DNSLookUp(string domainName)
    {
        return DNSRecords.GetValueOrDefault(domainName);
    }
}
