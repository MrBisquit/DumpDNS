using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class MX : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.MX;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        DnsClient.Protocol.MxRecord[] records = [];

        public MX()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Exchange", "Domain", "Preference", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = Functionality.Dump.client;
            IDnsQueryResponse response = lookup.Query(domain, QueryType.MX);
            records = response.AllRecords.MxRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.Exchange.ToString(),
                    record.DomainName.Original,
                    record.Preference.ToString(),
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
                DnsClient.Protocol.MxRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tEXCHANGE:".PadRight(DumpFile.Padding) + $"{record.Exchange}\n";
                str += $"\tDOMAIN:".PadRight(DumpFile.Padding) + $"{record.DomainName}\n";
                str += $"\tPREFERENCE:".PadRight(DumpFile.Padding) + $"{record.Preference}\n";
                str += $"\tTTL:".PadRight(DumpFile.Padding) + $"{record.TimeToLive}\n";
                str += $"\tInitial TTL:".PadRight(DumpFile.Padding) + $"{record.InitialTimeToLive}";
                str += "\n\n";
            }
            return str;
        }

        public int Count() => records.Length;
    }
}
