using System.IO.Ports;

namespace ModbusDevicesSimulator.Objects
{
    public class SerialSettings
    {
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
    }
}
