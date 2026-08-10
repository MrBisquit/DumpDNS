using DumpDNS.Internal;

namespace DumpDNS.Test
{

    public class Headers
    {

        /*
            Test records for headers
        */

        [Fact]
        public void Record_A()
        {
            var a = new Internal.Records.A();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_AAAA()
        {
            var a = new Internal.Records.AAAA();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_CAA()
        {
            var a = new Internal.Records.CAA();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_CERT()
        {
            var a = new Internal.Records.CERT();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_CNAME()
        {
            var a = new Internal.Records.CNAME();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_MX()
        {
            var a = new Internal.Records.MX();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_NAPTR()
        {
            var a = new Internal.Records.NAPTR();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_NS()
        {
            var a = new Internal.Records.NS();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_PTR()
        {
            var a = new Internal.Records.PTR();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_SRV()
        {
            var a = new Internal.Records.SRV();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_TLSA()
        {
            var a = new Internal.Records.TLSA();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_TXT()
        {
            var a = new Internal.Records.TXT();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }

        [Fact]
        public void Record_TXT_Test_Join()
        {
            var result = Internal.Records.TXT.JoinTXTValues(["A", "B"]);

            Assert.Equal("\"A\";\"B\"", result);
        }

        [Fact]
        public void Record_URI()
        {
            var a = new Internal.Records.URI();
            var table = a.FetchTable(new Query("*"));

            Assert.NotEmpty(table.Headers);
        }
    }
}
