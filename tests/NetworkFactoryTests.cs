using Xunit;

using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator.UnitTest
{
    public class NetworkFactoryTests
    {
        [Fact]
        public void Factory_DefaultIsTcp()
        {
            NetworkDescription description = new NetworkDescription();
            var network = NetworkFactory.CreateNetwork(description);
            Assert.True(network.GetType()==typeof(NetworkTcp));
        }

        [Fact]
        public void Factory_Tcp()
        {
            NetworkDescription description = new NetworkDescription()
            {
                Mode = NetworkType.tcp
            };
            var network = NetworkFactory.CreateNetwork(description);
            Assert.True(network.GetType() == typeof(NetworkTcp));
        }

        [Fact]
        public void Factory_Rtu()
        {
            NetworkDescription description = new NetworkDescription() 
            { 
                Mode = NetworkType.rtu 
            };
            var network = NetworkFactory.CreateNetwork(description);
            Assert.True(network.GetType() == typeof(NetworkRtu));
        }
    }
}
