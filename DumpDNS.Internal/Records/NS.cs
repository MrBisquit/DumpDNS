using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class NS : IRecord<NsRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.NS;

    internal List<NsRecord> _data = [];
    public NsRecord[] Data { get { return [.. _data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.NS);
        _data.AddRange(response.AllRecords.NsRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = ["Domain", "TTL", "Initial TTL", "NSD Name"],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL),
                new(record.NSDName.Original,                Types.TableDataRowType.Text)
            ]);
        }

        return data;
    }
}
