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
    }
}
