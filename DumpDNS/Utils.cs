using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS
{
    public static class Utils
    {
        public static bool IsIPInCIDR(IPAddress ip, string cidr)
        {
            string[] parts = cidr.Split('/');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid CIDR format");

            IPAddress baseAddress = IPAddress.Parse(parts[0]);
            int prefixLength = int.Parse(parts[1]);

            if (ip.AddressFamily != baseAddress.AddressFamily)
                throw new ArgumentException("IP address families do not match");

            byte[] ipBytes = ip.GetAddressBytes();
            byte[] baseBytes = baseAddress.GetAddressBytes();

            int fullBytes = prefixLength / 8;
            int remainingBits = prefixLength % 8;

            for (int i = 0; i < fullBytes; i++)
            {
                if (ipBytes[i] != baseBytes[i]) return false;
            }

            if (remainingBits > 0)
            {
                int mask = (byte)~(25 >> remainingBits);
                if ((ipBytes[fullBytes] & mask) != (baseBytes[fullBytes] & mask))
                    return false;
            }

            return true;
        }
    }
}
