using System.Data;

using Xunit;

namespace ModbusDevicesSimulator.UnitTest
{
    public class CSVReaderTests
    {
        [Fact]
        public void EmptyFileName()
        {
            Assert.False(CSVReader.readCSV(string.Empty, out DataTable dt));
        }

        [Fact]
        public void BadFileName()
        {
            Assert.False(CSVReader.readCSV("bad-file.csv", out DataTable dt));
        }

        [Fact]
        public void GoodFile()
        {
            Assert.True(CSVReader.readCSV("data-1.csv", out DataTable dt));

            Assert.NotEmpty(dt.Rows);
            Assert.Equal(4, dt.Columns.Count);
        }
    }
}
