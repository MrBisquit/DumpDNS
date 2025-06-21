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

            var lookup = new LookupClient();
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
                str += $"\tALGORITHM: \t{record.Algorithm}\n";
                str += $"\tTYPE: \t\t{record.CertType}\n";
                str += $"\tDOMAIN: \t{record.DomainName}\n";
                str += $"\tTTL: \t\t{record.TimeToLive}\n";
                str += $"\tInitial TTL: \t{record.InitialTimeToLive}";
                str += $"\tKEY TAG: \t{record.KeyTag}\n";
                str += $"\tPUBLIC KEY: \t{record.PublicKey}\n";
                str += "\n\n";
            }
            return str;
        }
    }
}
