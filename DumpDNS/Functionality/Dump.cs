using DnsClient;

namespace DumpDNS.Functionality
{
    public static class Dump
    {
        public static LookupClient client = new();

        public static IDnsQueryResponse Start(string domain, (int, int) dimensions, string? ip_str = null)
        {
            if(ip_str != null)
            {
                (System.Net.IPAddress, int)? result = ParseIP(ip_str);
                if(result != null)
                    client = new(result.Value.Item1, result.Value.Item2);
            }

            Program.ActiveInstructions = Program.BottomInstructions.ProcessingNoCancel;
            if(Program.Render != null) Program.Render(null, dimensions);
            Console.CursorTop = 2;
            Console.CursorLeft = 0;
            Console.Write("Working...");
            foreach (var Record in Types.IRecords)
            {
                Record.Value.FetchData(domain);
                Console.Write($"\rWorking... {Record.Key.ToString().PadRight(8)}");
            }
            var lookup = new LookupClient();
            return lookup.Query(domain, QueryType.ANY); // This wont work, but needs to return something
        }

        public static (System.Net.IPAddress, int)? ParseIP(string ip)
        {
            System.Net.IPAddress? address;
            int port = 53;

            string[] split_a = ip.Split(":");
            if (split_a.Length == 2)
            {
                port = int.Parse(split_a[1]);
            }
            else if (split_a.Length == 0 || split_a.Length < 2)
                return null;

            if (!System.Net.IPAddress.TryParse(split_a[0], out address))
                return null;

            return (address, port);
        }
    }
}