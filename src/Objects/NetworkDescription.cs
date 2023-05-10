
namespace ModbusDevicesSimulator.Objects
{
    public class NetworkDescription
    {
        /// <summary>
        /// Mode that will be used by default
        /// </summary>
        public NetworkType Mode { get; set; }

        /// <summary>
        /// COM port settings used if Mode == "rtu"
        /// </summary>
        public SerialSettings SerialSettings { get; set; }

        /// <summary>
        /// TCP settings used if Mode == "tcp"
        /// </summary>
        public TcpSettings TcpSettings { get; set; }

        /// <summary>
        /// list of devices on the network
        /// </summary>
        public DeviceDescription[] Devices { get; set; }
    }
}
