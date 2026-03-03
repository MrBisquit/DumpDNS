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

        internal DnsClient.Protocol.CertRecord[] records = [];

        public CERT()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Algorithm", "Type", "Domain", "TTL", "Initial TTL", "Key Tag", "Public Key" };
            Rows = new List<List<string>>();

            var lookup = Functionality.Dump.client;
            IDnsQueryResponse response = lookup.Query(domain, QueryType.CERT);
            records = response.AllRecords.CertRecords().ToArray();

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

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.CertRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tALGORITHM:".PadRight(DumpFile.Padding) + $"{record.Algorithm}\n";
                str += $"\tTYPE:".PadRight(DumpFile.Padding) + $"{record.CertType}\n";
                str += $"\tDOMAIN:".PadRight(DumpFile.Padding) + $"{record.DomainName}\n";
                str += $"\tTTL:".PadRight(DumpFile.Padding) + $"{record.TimeToLive}\n";
                str += $"\tInitial TTL:".PadRight(DumpFile.Padding) + $"{record.InitialTimeToLive}";
                str += $"\tKEY TAG:".PadRight(DumpFile.Padding) + $"{record.KeyTag}\n";
                str += $"\tPUBLIC KEY:".PadRight(DumpFile.Padding) + $"{record.PublicKey}\n";
                str += "\n\n";
            }
            return str;
        }

        public int Count() => records.Length;
    }
}
