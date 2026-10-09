using DnsClient.Protocol;

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
        //DnsRecordType.DNSKEY,
        //DnsRecordType.DS,
        //DnsRecordType.HTTPS,
        //DnsRecordType.LOC,
        DnsRecordType.MX,
        DnsRecordType.NAPTR,
        DnsRecordType.NS,
        DnsRecordType.PTR,
        //DnsRecordType.SMIMEA,
        DnsRecordType.SRV,
        //DnsRecordType.SSHFP,
        //DnsRecordType.SVCB,
        DnsRecordType.TLSA,
        DnsRecordType.TXT,
        DnsRecordType.URI
    };

    public static Dictionary<DnsRecordType, Type> DNSRecordTasks = new()
    {
        [DnsRecordType.A] = typeof(Records.A),
        [DnsRecordType.AAAA] = typeof(Records.AAAA),
        [DnsRecordType.CAA] = typeof(Records.CAA),
        [DnsRecordType.CERT] = typeof(Records.CERT),
        [DnsRecordType.CNAME] = typeof(Records.CNAME),
        //[DnsRecordType.DNSKEY] = typeof(Records.DNSKEY),
        //[DnsRecordType.DS] = typeof(Records.DS),
        //[DnsRecordType.HTTPS] = typeof(Records.HTTPS),
        //[DnsRecordType.LOC] = typeof(Records.LOC),
        [DnsRecordType.MX] = typeof(Records.MX),
        [DnsRecordType.NAPTR] = typeof(Records.NAPTR),
        [DnsRecordType.NS] = typeof(Records.NS),
        [DnsRecordType.PTR] = typeof(Records.PTR),
        //[DnsRecordType.SMIMEA] = typeof(Records.SMIMEA),
        [DnsRecordType.SRV] = typeof(Records.SRV),
        //[DnsRecordType.SSHFP] = typeof(Records.SSHFP),
        //[DnsRecordType.SVCB] = typeof(Records.SVCB),
        [DnsRecordType.TLSA] = typeof(Records.TLSA),
        [DnsRecordType.TXT] = typeof(Records.TXT),
        [DnsRecordType.URI] = typeof(Records.URI)
    };

    public static Dictionary<DnsRecordType, Type> DNSRecordTypes = new()
    {
        [DnsRecordType.A] = typeof(ARecord),
        [DnsRecordType.AAAA] = typeof(AaaaRecord),
        [DnsRecordType.CAA] = typeof(CaaRecord),
        [DnsRecordType.CERT] = typeof(CertRecord),
        [DnsRecordType.CNAME] = typeof(CNameRecord),
        //[DnsRecordType.DNSKEY] = typeof(Records.DNSKEY),
        //[DnsRecordType.DS] = typeof(Records.DS),
        //[DnsRecordType.HTTPS] = typeof(Records.HTTPS),
        //[DnsRecordType.LOC] = typeof(Records.LOC),
        [DnsRecordType.MX] = typeof(MxRecord),
        [DnsRecordType.NAPTR] = typeof(NAPtrRecord),
        [DnsRecordType.NS] = typeof(NsRecord),
        [DnsRecordType.PTR] = typeof(PtrRecord),
        //[DnsRecordType.SMIMEA] = typeof(Records.SMIMEA),
        [DnsRecordType.SRV] = typeof(SrvRecord),
        //[DnsRecordType.SSHFP] = typeof(Records.SSHFP),
        //[DnsRecordType.SVCB] = typeof(Records.SVCB),
        [DnsRecordType.TLSA] = typeof(TlsaRecord),
        [DnsRecordType.TXT] = typeof(TxtRecord),
        [DnsRecordType.URI] = typeof(UriRecord)
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
        Bytes,
        Port,
        Priority,
        CertificateAssociation,
        CertificateUsage,
        TextValues
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
