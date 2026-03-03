using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class NAPTR : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.NAPTR;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        internal DnsClient.Protocol.NAPtrRecord[] records = [];

        public NAPTR()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "Flags", "TTL", "Initial TTL", "Order", "Preference", "Regex", "Replacement", "Services" };
            Rows = new List<List<string>>();

            var lookup = Functionality.Dump.client;
            IDnsQueryResponse response = lookup.Query(domain, QueryType.NAPTR);
            DnsClient.Protocol.NAPtrRecord[] records = response.AllRecords.NAPtrRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    record.Flags.ToString(),
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.Order.ToString(),
                    record.Preference.ToString(),
                    record.RegularExpression,
                    record.Replacement.ToString(),
                    record.Services
                });
            }
        }

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.NAPtrRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tDOMAIN:".PadRight(DumpFile.Padding) + $"{record.DomainName}\n";
                str += $"\tFLAGS:".PadRight(DumpFile.Padding) + $"{record.Flags}\n";
                str += $"\tTTL:".PadRight(DumpFile.Padding) + $"{record.TimeToLive}\n";
                str += $"\tInitial TTL:".PadRight(DumpFile.Padding) + $"{record.InitialTimeToLive}\n";
                str += $"\tORDER:".PadRight(DumpFile.Padding) + $"{record.Order}\n";
                str += $"\tPREFEREMCE:".PadRight(DumpFile.Padding) + $"{record.Preference}\n";
                str += $"\tREGEX:".PadRight(DumpFile.Padding) + $"{record.RegularExpression}\n";
                str += $"\tREPLACEMENT:".PadRight(DumpFile.Padding) + $"{record.Replacement}\n";
                str += "\n\n";
            }
            return str;
        }

        public int Count() => records.Length;
    }
}
