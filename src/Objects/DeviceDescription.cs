using System.Collections.Generic;

namespace ModbusDevicesSimulator.Objects
{
    public class DeviceDescription
    {
        /// <summary>
        /// id of device
        /// </summary>
        public byte SlaveId { get; set; }

        /// <summary>
        /// data file to use for the replay
        /// </summary>
        public string DataFile { get; set; }

        /// <summary>
        /// if true addres offset will be incremented by 1
        /// </summary>
        public bool shiftOffest { get; set; }

        /// <summary>
        /// modbus addresses to simulate
        /// </summary>
        public List<AddressConfig> Addresses { get; set; }
    }
}
