using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class CAA : IRecord<CaaRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.CAA;

    internal List<CaaRecord> _data = [];
    public CaaRecord[] Data { get { return [.._data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.CAA);
        _data.AddRange(response.AllRecords.CaaRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = [ "Value", "Tag", "Flags", "Domain", "TTL", "Initial TTL" ],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.Value,                           Types.TableDataRowType.Text),
                new(record.Tag,                             Types.TableDataRowType.Text),
                new(record.Flags.ToString(),                Types.TableDataRowType.Bytes),
                new(record.DomainName,                      Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL)
            ]);
        }

        return data;
    }
}