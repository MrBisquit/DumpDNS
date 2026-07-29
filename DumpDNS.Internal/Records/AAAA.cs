using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class AAAA : IRecord<AaaaRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.AAAA;

    internal List<AaaaRecord> _data = [];
    public AaaaRecord[] Data { get { return [.. _data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.AAAA);
        _data.AddRange(response.AllRecords.AaaaRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = ["Address", "Domain", "TLL", "Initial TTL"],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.Address.ToString(),              Types.TableDataRowType.IPAddrv6),
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL)
            ]);
        }

        return data;
    }
}
