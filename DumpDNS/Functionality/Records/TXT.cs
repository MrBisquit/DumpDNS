using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class TXT : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.TXT;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public TXT()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "Text", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.TXT);
            DnsClient.Protocol.TxtRecord[] records = response.AllRecords.TxtRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    string.Join(", ", record.Text),
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString()
                });
            }
        }
    }
}
