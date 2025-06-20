using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    public class TLSA : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.TLSA;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public TLSA()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Certificate Association Data", "Certificate Usage", "Domain", "TTL", "Initial TTL", "Matching Type", "Selector" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.TLSA);
            DnsClient.Protocol.TlsaRecord[] records = response.AllRecords.TlsaRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.CertificateAssociationDataAsString,
                    record.CertificateUsage.ToString(),
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.MatchingType.ToString(),
                    record.Selector.ToString()
                });
            }
        }
    }
}
