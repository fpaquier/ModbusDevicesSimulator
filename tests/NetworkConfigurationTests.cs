using ModbusDevicesSimulator.Objects;
using System.Data;
using Xunit;

namespace ModbusDevicesSimulator.UnitTest
{
    public class NetworkConfigurationTests
    {
        private NetworkConfiguration _configuration = new NetworkConfiguration();

        [Fact]
        public void Creation()
        {
            Assert.Null(_configuration.Network);
        }

        [Fact]
        public void LoadBadFile()
        {
            _configuration.Load("bad-file.json");
            Assert.Null(_configuration.Network);
        }

        [Fact]
        public void LoadGoodFile()
        {
            _configuration.Load("config-2-devices.json");
            Assert.NotNull(_configuration.Network);
            Assert.Equal(NetworkType.tcp, _configuration.Network.Mode);
        }
    }
}
