using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class MX : IRecord<MxRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.MX;

    internal List<MxRecord> _data = [];
    public MxRecord[] Data { get { return [.. _data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.MX);
        _data.AddRange(response.AllRecords.MxRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = ["Exchange", "Domain", "Preference", "TTL", "Initial TTL"],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.Exchange.ToString(),             Types.TableDataRowType.Text),
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.Preference.ToString(),           Types.TableDataRowType.Text),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL)
            ]);
        }

        return data;
    }
}
