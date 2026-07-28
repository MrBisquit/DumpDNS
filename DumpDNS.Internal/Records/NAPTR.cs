using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class NAPTR : IRecord<NAPtrRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.NAPTR;

    internal List<NAPtrRecord> _data = [];
    public NAPtrRecord[] Data { get { return [.._data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.NAPTR);
        _data.AddRange(response.AllRecords.NAPtrRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = [ "Domain", "Flags", "TTL", "Initial TTL", "Order", "Preference", "Regex", "Replacement", "Services" ],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(record.Flags,                           Types.TableDataRowType.Text),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL),
                new(record.Order.ToString(),                Types.TableDataRowType.Text),
                new(record.Preference.ToString(),           Types.TableDataRowType.Text),
                new(record.RegularExpression.ToString(),    Types.TableDataRowType.Text),
                new(record.Replacement.ToString(),          Types.TableDataRowType.Text)
            ]);
        }

        return data;
    }
}