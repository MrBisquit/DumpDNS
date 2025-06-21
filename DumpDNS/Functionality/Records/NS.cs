using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class NS : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.NS;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        internal DnsClient.Protocol.NsRecord[] records = [];

        public NS()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "TTL", "Initial TTL", "NSD Name" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.NS);
            records = response.AllRecords.NsRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.NSDName.Original
                });
            }
        }

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.NsRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tDOMAIN: \t{record.DomainName}\n";
                str += $"\tTTL: \t\t{record.TimeToLive}\n";
                str += $"\tInitial TTL: \t{record.InitialTimeToLive}\n";
                str += $"\tNSD NAME: \t{record.NSDName}";
                str += "\n\n";
            }
            return str;
        }
    }
}
