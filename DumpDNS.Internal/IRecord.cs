using DnsClient;

namespace DumpDNS.Internal
{
    public interface IRecord<T>
    {
        Types.DnsRecordType RecordType { get; }

        T[] Data { get; }

        // Fetch the data from the LookupClient, and store within the Data argument
        void FetchData(LookupClient client, Types.LookupInfo info);
        Task FetchDataAsync(LookupClient client, Types.LookupInfo info);

        Types.TableData FetchTable(Query query);
    }
}
