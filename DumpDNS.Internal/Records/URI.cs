using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class URI : IRecord<UriRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.URI;

    internal List<UriRecord> _data = [];
    public UriRecord[] Data { get { return [.. _data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.URI);
        _data.AddRange(response.AllRecords.UriRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = ["Domain", "TTL", "Initial TTL", "Priority", "Target", "Weight"],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL),
                new(record.Priority.ToString(),             Types.TableDataRowType.Priority),
                new(record.Target,                          Types.TableDataRowType.Text),
                new(record.Weight.ToString(),               Types.TableDataRowType.Text)
            ]);
        }

        return data;
    }
}
