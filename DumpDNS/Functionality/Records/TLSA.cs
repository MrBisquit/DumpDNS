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

        internal DnsClient.Protocol.TlsaRecord[] records = [];

        public TLSA()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Certificate Association Data", "Certificate Usage", "Domain", "TTL", "Initial TTL", "Matching Type", "Selector" };
            Rows = new List<List<string>>();

            var lookup = Functionality.Dump.client;
            IDnsQueryResponse response = lookup.Query(domain, QueryType.TLSA);
            records = response.AllRecords.TlsaRecords().ToArray();

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

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.TlsaRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tCERTIFICATE ASSOCIATION DATA:".PadRight(DumpFile.Padding) + $"{record.CertificateAssociationDataAsString}\n";
                str += $"\tCERTIFICATE USAGE:".PadRight(DumpFile.Padding) + $"{record.CertificateUsage}\n";
                str += $"\tDOMAIN:".PadRight(DumpFile.Padding) + $"{record.DomainName}\n";
                str += $"\tTTL:".PadRight(DumpFile.Padding) + $"{record.TimeToLive}\n";
                str += $"\tInitial TTL:".PadRight(DumpFile.Padding) + $"{record.InitialTimeToLive}";
                str += $"\tMATCHING TYPE:".PadRight(DumpFile.Padding) + $"{record.MatchingType}\n";
                str += $"\tSELECTOR:".PadRight(DumpFile.Padding) + $"{record.Selector}\n";
                str += "\n\n";
            }
            return str;
        }

        public int Count() => records.Length;
    }
}
