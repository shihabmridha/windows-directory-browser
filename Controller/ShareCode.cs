using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project.Controller {
    class ShareCode {
        public static string  LocalIPAddress() {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return null;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
        public static string GenerateShareCode(string LocalIP) {
            return BitConverter.ToInt32(IPAddress.Parse(LocalIP).GetAddressBytes(), 0).ToString();
        }
        public static string GetIPFromCode(string code) {
            return new IPAddress(BitConverter.GetBytes(Convert.ToInt32(code))).ToString(); ;
        }

        public static bool ValidateIP(string ip) {
            if (String.IsNullOrWhiteSpace(ip)) {
                return false;
            }
            string[] splitValues = ip.Split('.');
            if (splitValues.Length != 4) {
                return false;
            }
            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}
