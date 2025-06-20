using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class CERT : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.CERT;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public CERT()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Algorithm", "Type", "Domain", "TTL", "Initial TTL", "Key Tag", "Public Key" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.CERT);
            DnsClient.Protocol.CertRecord[] records = response.AllRecords.CertRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.Algorithm.ToString(),
                    record.CertType.ToString(),
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.KeyTag.ToString(),
                    record.PublicKeyAsString
                });
            }
        }
    }
}
