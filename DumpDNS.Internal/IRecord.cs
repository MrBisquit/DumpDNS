using DnsClient;
using DnsClient.Protocol;

namespace DumpDNS.Internal
{
    public interface IRecord
    {
        Types.DnsRecordType RecordType { get; }

        void FetchData(LookupClient client, Types.LookupInfo info);
        Task FetchDataAsync(LookupClient client, Types.LookupInfo info);

        Types.TableData FetchTable(Query query);

        public static IRecord? Create(Types.DnsRecordType type)
        {
            switch(type)
            {
                case Types.DnsRecordType.A:
                    return new Records.A();
            }

            return null;
        }
    }

    public interface IRecord<T> : IRecord
    {
        //Types.DnsRecordType RecordType { get; }

        T[] Data { get; }

        // Fetch the data from the LookupClient, and store within the Data argument
        //void FetchData(LookupClient client, Types.LookupInfo info);
        //Task FetchDataAsync(LookupClient client, Types.LookupInfo info);

        //Types.TableData FetchTable(Query query);
    }
}
