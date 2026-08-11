using System.Text;
using DnsClient;

namespace DumpDNS
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                return CLI.CLI.Run(args);
            }

            return CLI.CLI.Run(["-?"]);
        }
    }
}
