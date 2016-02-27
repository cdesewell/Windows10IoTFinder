using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FindWin10IoT
{
    class Program
    {
        static void Main(string[] args)
        {
            var gateway = GetGatewayAddress();
            int port = 8080;
            var addressPrefix = gateway.Split('.');

            for (int i = 2; i <= 255; i++)
            {
                string address = addressPrefix[0] + "." + addressPrefix[1] + "." + addressPrefix[2] + "." + i;
                IsIoTDevice(address, port);
            }
            Console.ReadLine();
        }

        private async static Task IsIoTDevice(string address,int port)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.SendTimeout = 1;
                await tcpClient.ConnectAsync(address, port);
                Process.Start("http://" + address + ":" + port.ToString());
                Environment.Exit(1);
            }
            catch (Exception)
            {
                
            }
            
        }

        private static string GetGatewayAddress()
        {
            foreach (var router in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (router.OperationalStatus == OperationalStatus.Up)
                {
                    var addresses = router.GetIPProperties().GatewayAddresses.Select(a => a.Address).ToList();
                    foreach (var address in addresses)
                    {
                        return address.ToString();
                    }
                }
            }
            return null;
        }
    }
}
