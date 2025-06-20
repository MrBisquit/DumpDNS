using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class MX : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.MX;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public MX()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Exchange", "Domain", "Preference", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.MX);
            DnsClient.Protocol.MxRecord[] records = response.AllRecords.MxRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.Exchange.ToString(),
                    record.DomainName.Original,
                    record.Preference.ToString(),
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString()
                });
            }
        }
    }
}
