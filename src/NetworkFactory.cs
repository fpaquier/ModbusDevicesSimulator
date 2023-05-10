using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator
{
    public static class NetworkFactory
    {
        public static IModbusNetwork CreateNetwork(NetworkDescription network)
        {
            if (network.Mode == NetworkType.rtu)
            {
                return new NetworkRtu(network);
            }
            return new NetworkTcp(network);
        }
    }
}
