using System.Threading;

namespace ModbusDevicesSimulator
{
    public interface IModbusNetwork
    {
        void Start(CancellationToken token);
    }
}
