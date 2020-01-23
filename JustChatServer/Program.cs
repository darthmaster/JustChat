using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JustChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var ip = GetLocalIPAddress();
                    webBuilder.UseUrls($"http://{ip}:5000;https://{ip}:5001");
                    webBuilder.UseStartup<Startup>();
                });
        public static string GetLocalFQDN()
        {
            var props = IPGlobalProperties.GetIPGlobalProperties();
            return props.HostName + (string.IsNullOrWhiteSpace(props.DomainName) ? "" : "." + props.DomainName);
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            List<string> ips = new List<string>();
            int i=0;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(i+": "+ip.ToString());
                    ips.Add(ip.ToString());
                    i++;
                }
            }
            Console.Write("Выберите аддрес запуска сервера: ");
            return ips[Convert.ToInt32(Console.ReadLine())];
            throw new Exception("Local IP Address Not Found!");
            
        }
    }
}
