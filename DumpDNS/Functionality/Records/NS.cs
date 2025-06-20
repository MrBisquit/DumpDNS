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
            DnsClient.Protocol.NsRecord[] records = response.AllRecords.NsRecords().ToArray();

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
    }
}
