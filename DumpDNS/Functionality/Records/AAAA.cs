using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class AAAA : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.AAAA;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        internal DnsClient.Protocol.AaaaRecord[] records = [];

        public AAAA()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Address", "Domain", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = Functionality.Dump.client;
            IDnsQueryResponse response = lookup.Query(domain, QueryType.AAAA);
            records = response.AllRecords.AaaaRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.Address.ToString(),
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString()
                });
            }
        }

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.AaaaRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tIP ADDRESS:".PadRight(DumpFile.Padding) + $"{record.Address}\n";
                str += $"\tDOMAIN:".PadRight(DumpFile.Padding) + $"{record.DomainName}\n";
                str += $"\tTTL:".PadRight(DumpFile.Padding) + $"{record.TimeToLive}\n";
                str += $"\tInitial TTL:".PadRight(DumpFile.Padding) + $"{record.InitialTimeToLive}";
                str += "\n\n";
            }
            return str;
        }

        public int Count() => records.Length;

        public void Results(CLI.CLI.Depth depth, bool colour, int i)
        {
            var sources = Ranges.Find(records[i].Address, Ranges.Family.IPv6);
            Console.Write($"\t{(Headers[0] + ":").PadRight(CLI.Results.ResultsPadding)}");
            if (colour) Console.ForegroundColor = ConsoleColor.White;
            Console.Write(records[i].Address);
            if (colour) Console.ResetColor();
            if (sources.Count == 0) Console.WriteLine("\t(No sources)");
            else Console.Write("\t(");
            for (int j = 0; j < sources.Count; j++)
            {
                if (colour) Console.ForegroundColor = sources[j].Colour;
                Console.Write(sources[j].Label);
                if (colour) Console.ResetColor();
                if (j != sources.Count - 1) Console.Write(", ");
            }
            if (sources.Count == 0) Console.WriteLine();
            else Console.WriteLine(")");

            Console.Write($"\t{(Headers[1] + ":").PadRight(CLI.Results.ResultsPadding)}");
            if (colour) Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(records[i].DomainName);
            if (colour) Console.ResetColor();

            Console.Write($"\t{(Headers[2] + ":").PadRight(CLI.Results.ResultsPadding)}");
            if (colour) Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(records[i].TimeToLive);
            if (colour) Console.ResetColor();

            Console.Write($"\t{(Headers[3] + ":").PadRight(CLI.Results.ResultsPadding)}");
            if (colour) Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(records[i].InitialTimeToLive);
            if (colour) Console.ResetColor();
        }
    }
}
