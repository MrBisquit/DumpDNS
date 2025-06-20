using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class CNAME : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.A;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public CNAME()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Name", "Domain", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.CNAME);
            DnsClient.Protocol.CNameRecord[] records = response.AllRecords.CnameRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.CanonicalName.ToString(),
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString()
                });
            }
        }
    }
}
