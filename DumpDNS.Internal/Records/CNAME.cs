using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class CNAME : IRecord<CNameRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.CNAME;

    internal List<CNameRecord> _data = [];
    public CNameRecord[] Data { get { return [.._data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.CNAME);
        _data.AddRange(response.AllRecords.CnameRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = [ "Name", "Domain", "TTL", "Initial TTL" ],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.CanonicalName,                   Types.TableDataRowType.Text),
                new(record.DomainName,                      Types.TableDataRowType.Domain),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL)
            ]);
        }

        return data;
    }
}