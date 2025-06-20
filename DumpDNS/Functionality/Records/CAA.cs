using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class CAA : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.CAA;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public CAA()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Value", "Tag", "Flags", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.CAA);
            DnsClient.Protocol.CaaRecord[] records = response.AllRecords.CaaRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.Value,
                    record.Tag,
                    record.Flags.ToString(),
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString()
                });
            }
        }
    }
}
