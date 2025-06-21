using DnsClient;

namespace DumpDNS.Functionality.Records
{
    public class URI : IRecord
    {
        public Types.DnsRecordType Type { get; set; } = Types.DnsRecordType.URI;
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        internal DnsClient.Protocol.UriRecord[] records = [];

        public URI()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }

        public void FetchData(string domain)
        {
            Headers = new List<string> { "Domain", "TTL", "Initial TTL", "Priority", "Target", "Weight" };
            Rows = new List<List<string>>();

            var lookup = new LookupClient();
            IDnsQueryResponse response = lookup.Query(domain, QueryType.URI);
            records = response.AllRecords.UriRecords().ToArray();

            foreach (var record in records)
            {
                Rows.Add(new List<string>
                {
                    record.DomainName.Original,
                    record.TimeToLive.ToString(),
                    record.InitialTimeToLive.ToString(),
                    record.Priority.ToString(),
                    record.Target,
                    record.Weight.ToString()
                });
            }
        }

        public string Dump()
        {
            string str = "";
            for (int i = 0; i < records.Length; i++)
            {
                DnsClient.Protocol.UriRecord record = records[i];
                str += $"Record {i}\n";
                str += $"\tDOMAIN: \t{record.DomainName}\n";
                str += $"\tTTL: \t\t{record.TimeToLive}\n";
                str += $"\tInitial TTL: \t{record.InitialTimeToLive}";
                str += $"\tPRIORITY: \t{record.Priority}\n";
                str += $"\tTARGET: \t\t{record.Target}\n";
                str += $"\tWEIGHT: \t{record.Target}\n";
                str += "\n\n";
            }
            return str;
        }
    }
}
