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

        internal DnsClient.Protocol.CaaRecord[] records = [];

        public CAA()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Value", "Tag", "Flags", "Domain", "TTL", "Initial TTL" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.CAA);
            records = response.AllRecords.CaaRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.Value,
                    record.Tag,
                    record.Flags.ToString(),
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString()
                });
            }
        }

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.CaaRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tVALUE: \t{record.Value}\n";
                str += $"\tTAG: \t\t{record.Tag}\n";
                str += $"\tFLAGS: \t{record.Flags}\n";
                str += $"\tDOMAIN: \t{record.DomainName}\n";
                str += $"\tTTL: \t\t{record.TimeToLive}\n";
                str += $"\tInitial TTL: \t{record.InitialTimeToLive}";
                str += "\n\n";
            }
            return str;
        }
    }
}
