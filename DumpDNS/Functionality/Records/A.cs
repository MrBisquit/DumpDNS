using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class A : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.A;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        internal DnsClient.Protocol.ARecord[] records = [];

        public A()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Address", "Domain", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = Functionality.Dump.client;
            IDnsQueryResponse response = lookup.Query(domain, QueryType.A);
            records = response.AllRecords.ARecords().ToArray();

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
                DnsClient.Protocol.ARecord record = records[i];
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
    }
}
