namespace DumpDNS
{
    public static class Types
    {
        public enum DnsRecordType
        {
            A,
            AAAA,
            CAA,
            CERT,
            CNAME,
            //DNSKEY,
            //DS,
            //HTTPS,
            //LOC,
            MX,
            NAPTR,
            NS,
            PTR,
            //SMIMEA,
            SRV,
            //SSHFP,
            //SVCB,
            TLSA,
            TXT,
            URI
        }

        public static Dictionary<DnsRecordType, string> DNSRecordTypeDictionary = new()
        {
            [DnsRecordType.A] = "A",
            [DnsRecordType.AAAA] = "AAAA",
            [DnsRecordType.CAA] = "CAA",
            [DnsRecordType.CERT] = "CERT",
            [DnsRecordType.CNAME] = "CNAME",
            //[DnsRecordType.DNSKEY] = "DNSKEY",
            //[DnsRecordType.DS] = "DS",
            //[DnsRecordType.HTTPS] = "HTTPS",
            //[DnsRecordType.LOC] = "LOC",
            [DnsRecordType.MX] = "MX",
            [DnsRecordType.NAPTR] = "NAPTR",
            [DnsRecordType.NS] = "NS",
            [DnsRecordType.PTR] = "PTR",
            //[DnsRecordType.SMIMEA] = "SMIMEA",
            [DnsRecordType.SRV] = "SRV",
            //[DnsRecordType.SSHFP] = "SSHFP",
            //[DnsRecordType.SVCB] = "SVCB",
            [DnsRecordType.TLSA] = "TLSA",
            [DnsRecordType.TXT] = "TXT",
            [DnsRecordType.URI] = "URI"
        };

        public static DnsRecordType[] RecordTypes =
        {
            DnsRecordType.A,
            DnsRecordType.AAAA,
            DnsRecordType.CAA,
            DnsRecordType.CERT,
            DnsRecordType.CNAME,
            //DnsRecordType.DNSKEY,       // NA
            //DnsRecordType.DS,           // NA
            //DnsRecordType.HTTPS,        // NA
            //DnsRecordType.LOC,          // NA
            DnsRecordType.MX,
            DnsRecordType.NAPTR,
            DnsRecordType.NS,
            DnsRecordType.PTR,
            //DnsRecordType.SMIMEA,       // NA
            DnsRecordType.SRV,
            //DnsRecordType.SSHFP,        // NA
            //DnsRecordType.SVCB,         // NA
            DnsRecordType.TLSA,
            DnsRecordType.TXT,
            DnsRecordType.URI
        };

        public static Dictionary<DnsRecordType, Functionality.Records.IRecord> IRecords =
        new()
        {
            [DnsRecordType.A] = new Functionality.Records.A(),
            [DnsRecordType.AAAA] = new Functionality.Records.AAAA(),
            [DnsRecordType.CAA] = new Functionality.Records.CAA(),
            [DnsRecordType.CERT] = new Functionality.Records.CERT(),
            [DnsRecordType.CNAME] = new Functionality.Records.CNAME(),
            [DnsRecordType.MX] = new Functionality.Records.MX(),
            [DnsRecordType.NAPTR] = new Functionality.Records.NAPTR(),
            [DnsRecordType.NS] = new Functionality.Records.NS(),
            [DnsRecordType.PTR] = new Functionality.Records.PTR(),
            [DnsRecordType.SRV] = new Functionality.Records.SRV(),
            [DnsRecordType.TLSA] = new Functionality.Records.TLSA(),
            [DnsRecordType.TXT] = new Functionality.Records.TXT(),
            [DnsRecordType.URI] = new Functionality.Records.URI()
        };
    }
}