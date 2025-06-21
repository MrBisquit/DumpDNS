using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality.Records
{
    /// <summary>
    /// Record interface
    /// </summary>
    public interface IRecord
    {
        Types.DnsRecordType Type { get; set; }
        List<string> Headers { get; set; }
        List<List<string>> Rows { get; set; }
        void FetchData(string domain);
        string Dump();
    }
}
