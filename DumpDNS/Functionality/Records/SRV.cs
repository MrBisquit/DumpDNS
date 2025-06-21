using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class SRV : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.SRV;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        internal DnsClient.Protocol.SrvRecord[] records = [];

        public SRV()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "TTL", "Initial TTL", "Port", "Priority", "Target", "Weight" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.SRV);
            records = response.AllRecords.SrvRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.Port.ToString(),
                    record.Priority.ToString(),
                    record.Target.Original,
                    record.Weight.ToString()
                });
            }
        }

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.SrvRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tDOMAIN: \t{record.DomainName}\n";
                str += $"\tTTL: \t\t{record.TimeToLive}\n";
                str += $"\tInitial TTL: \t{record.InitialTimeToLive}\n";
                str += $"\tPORT: \t{record.Port}\n";
                str += $"\tPRIORITY: \t{record.Priority}\n";
                str += $"\tTARGET: \t{record.Target}\n";
                str += $"\tWEIGHT: \t{record.Weight}";
                str += "\n\n";
            }
            return str;
        }
    }
}
