namespace DumpDNS.Internal;

public static class Types
{
    public enum DnsRecordType
    {
        A,
        AAAA,
        CAA,
        CERT,
        CNAME,
        DNSKEY,
        DS,
        HTTPS,
        LOC,
        MX,
        NAPTR,
        NS,
        PTR,
        SMIMEA,
        SRV,
        SSHFP,
        SVCB,
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
        [DnsRecordType.DNSKEY] = "DNSKEY",
        [DnsRecordType.DS] = "DS",
        [DnsRecordType.HTTPS] = "HTTPS",
        [DnsRecordType.LOC] = "LOC",
        [DnsRecordType.MX] = "MX",
        [DnsRecordType.NAPTR] = "NAPTR",
        [DnsRecordType.NS] = "NS",
        [DnsRecordType.PTR] = "PTR",
        [DnsRecordType.SMIMEA] = "SMIMEA",
        [DnsRecordType.SRV] = "SRV",
        [DnsRecordType.SSHFP] = "SSHFP",
        [DnsRecordType.SVCB] = "SVCB",
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
        DnsRecordType.DNSKEY,
        DnsRecordType.DS,
        DnsRecordType.HTTPS,
        DnsRecordType.LOC,
        DnsRecordType.MX,
        DnsRecordType.NAPTR,
        DnsRecordType.NS,
        DnsRecordType.PTR,
        DnsRecordType.SMIMEA,
        DnsRecordType.SRV,
        DnsRecordType.SSHFP,
        DnsRecordType.SVCB,
        DnsRecordType.TLSA,
        DnsRecordType.TXT,
        DnsRecordType.URI
    };

    public class TableData
    {
        public List<string> Headers { get; set; } = [];
        public List<List<TableDataRow>> Rows { get; set; } = [];
    }

    public enum TableDataRowType
    {
        Text,
        IPAddrv4,
        IPAddrv6,
        Domain,
        TTL,
        ITTL,
        Bytes
    }

    public class TableDataRow(string content, TableDataRowType type)
    {
        public string Content { get; set; } = content;
        public TableDataRowType Type { get; set; } = type;
    }

    public class LookupInfo(string domain)
    {
        public string Domain { get; set; } = domain;
    }
}
