using System;
using System.Runtime.Serialization;

namespace ModbusDevicesSimulator
{
    public class ModbusSimulatorException: Exception
    {
        public ModbusSimulatorException()
        : base()
        {
        }

        public ModbusSimulatorException(string message)
            : base(message)
        {
        }

        public ModbusSimulatorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ModbusSimulatorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
