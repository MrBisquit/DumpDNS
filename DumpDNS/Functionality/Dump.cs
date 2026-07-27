using DnsClient;
using System.Net;

namespace DumpDNS.Functionality
{
    public static class Dump
    {
        internal static LookupClient client = new();

        public static IDnsQueryResponse Start(string domain, (int, int) dimensions, string? ip_str = null, bool log = true)
        {
            if (ip_str != null)
            {
                (IPAddress, int)? result = ParseIP(ip_str);
                if (result != null)
                    client = new(result.Value.Item1, result.Value.Item2);
            }

            Program.ActiveInstructions = Program.BottomInstructions.ProcessingNoCancel;
            if (Program.Render != null) Program.Render(null, dimensions);
            if (log)
            {
                Console.CursorTop = 2;
                Console.CursorLeft = 0;
                Console.WriteLine($"Using {client.Settings.NameServers[0].Address}:{client.Settings.NameServers[0].Port}");
                Console.Write("Working...");
            }
            void DrawBar(double progress)
            {
                if (!log) return;

                dimensions = new(Console.BufferWidth, Console.BufferHeight);

                Console.CursorTop = 5;
                Console.CursorLeft = 0;

                int filled = (int)Math.Ceiling(progress * ((double)dimensions.Item1 - 2) / 100);
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(new string(' ', filled));
                Console.ResetColor();

                Console.CursorTop = 3;
                Console.CursorLeft = 0;
            }
            for (int i = 0; i < Types.IRecords.Count; i++)
            {
                var Record = Types.IRecords[(Types.DnsRecordType)i];

                try
                {
                    Record.FetchData(domain);
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
                finally { DrawBar((double)i / (double)Types.IRecords.Count * 100); }
                if (log) Console.Write($"\rWorking... {Types.IRecords.Keys.ToList()[i],-8}");
            }
            var lookup = new LookupClient();
            return lookup.Query(domain, QueryType.ANY); // This wont work, but needs to return something
        }

        public static (IPAddress, int)? ParseIP(string ip)
        {
            var parts = ip.Split(':', 2);
            int port = 53;
            if (!IPAddress.TryParse(parts[0], out var address))
                return null;
            if (parts.Length == 2 && !int.TryParse(parts[1], out port))
                return null;

            return (address, port);
        }
    }
}