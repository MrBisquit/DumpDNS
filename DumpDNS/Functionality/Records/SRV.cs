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
            DnsClient.Protocol.SrvRecord[] records = response.AllRecords.SrvRecords().ToArray();

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
    }
}
