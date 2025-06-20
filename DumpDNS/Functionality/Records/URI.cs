using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class URI : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.URI;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public URI()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "TTL", "Initial TTL", "Priority", "Target", "Weight" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.URI);
            DnsClient.Protocol.UriRecord[] records = response.AllRecords.UriRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.Priority.ToString(),
                    record.Target,
                    record.Weight.ToString()
                });
            }
        }
    }
}
