using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class PTR : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.PTR;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public PTR()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "TTL", "Initial TTL", "PTR Domain" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.PTR);
            DnsClient.Protocol.PtrRecord[] records = response.AllRecords.PtrRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.PtrDomainName.Original
                });
            }
        }
    }
}
