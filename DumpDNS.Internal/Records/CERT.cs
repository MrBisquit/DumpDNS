using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class CERT : IRecord<CertRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.CERT;

    internal List<CertRecord> _data = [];
    public CertRecord[] Data { get { return [.._data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.CERT);
        _data.AddRange(response.AllRecords.CertRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = [ "Algorithm", "Type", "Domain", "TTL", "Initial TTL", "Key Tag", "Public Key" ],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.Algorithm.ToString(),            Types.TableDataRowType.Text),
                new(record.CertType.ToString(),             Types.TableDataRowType.Text),
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL),
                new(record.PublicKeyAsString,               Types.TableDataRowType.Text)
            ]);
        }

        return data;
    }
}