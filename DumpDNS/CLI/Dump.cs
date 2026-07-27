using DumpDNS.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.CLI
{
    public static class Dump
    {
        public static int StartDump(string domain, IPAddress? dns, int port, List<Types.DnsRecordType>? records, bool statistics, bool colour, CLI.Format format, string? dump, CLI.Depth depth)
        {
            records ??= [.. Types.RecordTypes];
            records.Sort();

            if (dns != null)
                Functionality.Dump.client = new(dns, port);
            else
                Functionality.Dump.client = new();

            for(int i = 0; i < records.Count; i++)
            {
                var record = Types.IRecords[records[i]];

                try
                {
                    record.FetchData(domain);
                } catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: Failed to get {records[i]} records: {ex.Message}");
                }
            }

            if(depth > CLI.Depth.Minimal)
            {
                // Looks up things such as IP Addresses in CIDR ranges
                Ranges.LoadRanges();
            }

            if(dump != null)
            {
                DumpFile.CreateDump(dump, domain, records);
            } else
            {
                if (statistics) Results.DisplayResultTotals(records, colour, depth);
                Results.DisplayResults(records, colour, depth);
            }

            return 0;
        }
    }
}