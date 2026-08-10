using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class TLSA : IRecord<TlsaRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.TLSA;

    internal List<TlsaRecord> _data = [];
    public TlsaRecord[] Data { get { return [.. _data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.TLSA);
        _data.AddRange(response.AllRecords.TlsaRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = ["Certificate Association Data", "Certificate Usage", "Domain", "TTL", "Initial TTL", "Matching Type", "Selector"],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.CertificateAssociationDataAsString, Types.TableDataRowType.CertificateAssociation),
                new(record.CertificateUsage.ToString(),     Types.TableDataRowType.CertificateUsage),
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL),
                new(record.MatchingType.ToString(),         Types.TableDataRowType.Text),
                new(record.Selector.ToString(),             Types.TableDataRowType.Text)
            ]);
        }

        return data;
    }
}
