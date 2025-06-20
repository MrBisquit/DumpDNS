using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class AAAA : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.AAAA;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public AAAA()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Address", "Domain", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.AAAA);
            DnsClient.Protocol.AaaaRecord[] records = response.AllRecords.AaaaRecords().ToArray();

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
    }
}
