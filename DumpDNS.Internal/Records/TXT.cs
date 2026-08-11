using System;
using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal.Records;

public class TXT : IRecord<TxtRecord>
{
    public Types.DnsRecordType RecordType { get; } = Types.DnsRecordType.TXT;

    internal List<TxtRecord> _data = [];
    public TxtRecord[] Data { get { return [.. _data]; } }

    public void FetchData(LookupClient client, Types.LookupInfo info)
    {
        FetchDataAsync(client, info).Wait();
    }

    public async Task FetchDataAsync(LookupClient client, Types.LookupInfo info)
    {
        IDnsQueryResponse response = await client.QueryAsync(info.Domain, QueryType.TXT);
        _data.AddRange(response.AllRecords.TxtRecords());
    }

    public Types.TableData FetchTable(Query query)
    {
        Types.TableData data = new()
        {
            Headers = ["Domain", "Text", "TTL", "Initial TTL"],
            Rows = []
        };

        foreach (var record in _data)
        {
            data.Rows.Add([
                new(record.DomainName.Original,             Types.TableDataRowType.Domain),
                new(JoinTXTValues(record.Text),             Types.TableDataRowType.TextValues),
                new(record.TimeToLive.ToString(),           Types.TableDataRowType.TTL),
                new(record.InitialTimeToLive.ToString(),    Types.TableDataRowType.ITTL)
            ]);
        }

        return data;
    }

    public static string JoinTXTValues(string[] values)
    {
        string str = "";
        for (int i = 0; i < values.Length; i++)
            str += $"\"{values[i]}\"{(i < values.Length - 1 ? ";" : "")}";
        return str;
    }

    public static string JoinTXTValues(ICollection<string> values)
    {
        return JoinTXTValues([.. values]);
    }
}
