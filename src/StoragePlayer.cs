using System;
using System.Data;
using System.Security.Cryptography;
using System.Threading;

using NModbus;

using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator
{
    public class StoragePlayer
    {
        private DataTable _dataTable;
        private ISlaveDataStore _slaveDataStore;
        private DeviceDescription _slaveDescription;

        private string _name;
        private int _playMode = 0;

        public StoragePlayer(DeviceDescription slaveDescription)
        {
            _slaveDataStore = new DeviceStorage();
            _slaveDescription = slaveDescription;
            _name = $"Player Id {_slaveDescription.SlaveId}";
        }

        public void Start(CancellationToken cancellationToken)
        {
            _dataTable = LoadData(_slaveDescription.DataFile);

            string threadName = $"Thread-{_name}";
            var thread = new Thread(Run) { Name = threadName, IsBackground = true };
            thread.Start(cancellationToken);
        }

        public ISlaveDataStore DataStore { get { return _slaveDataStore; } }

        private void Run(object obj)
        {
            Console.WriteLine($"Start {_name}...");

            int i = 0;
            Random rand = new Random(RandomNumberGenerator.GetInt32(268790));
            CancellationToken token = (CancellationToken)obj;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    int index = 0;
                    if (_playMode == 0)
                    {
                        index = i % _dataTable.Rows.Count;
                        i++;
                    }
                    else
                    {
                        index = rand.Next() % _dataTable.Rows.Count;
                    }

                    DataRow Row = _dataTable.Rows[index];
                    for (int col = 1; col < _dataTable.Columns.Count; col++)
                    {
                        string ioName = _dataTable.Columns[col].ToString();
                        string valueStr = Row[col].ToString();
                        SimulateValue(ioName, valueStr);
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }

                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void SimulateValue(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                Console.WriteLine("no value for that");
                return;
            }

            var address = _slaveDescription.Addresses.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (address == null)
            {
                Console.WriteLine($"no instruction for {name}");
                return;
            }

            int offsetShift = 0;
            if (_slaveDescription.shiftOffest)
            {
                offsetShift = 1;
            }
            
            try
            {
                //int coe_reverse = floatToInt(1.00000000f / coe);
                float fValue;
                if (value.Contains('.'))
                {
                    fValue = float.Parse(value);
                }
                else
                {
                    fValue = int.Parse(value);
                }

                switch (address.DataType.ToUpper())
                {
                    case "INT16":
                    case "WORD":
                    case "INT":
                        setValue16(address.Function, address.Offset + offsetShift, (ushort)(fValue * 1));
                        break;
                    case "REAL":
                    case "FLOAT":
                        fValue = float.Parse(value);
                        setValue32(address.Function, address.Offset + offsetShift, fValue * 1);
                        break;
                    case "INT32":
                    case "DINT":
                    case "DWORD":
                        setValue32(address.Function, address.Offset + offsetShift, (int)fValue * 1);
                        break;
                    case "BIT":
                    default:
                        throw new ModbusSimulatorException();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"SimulateValue error: {err.Message}");
            }
        }

        private IPointSource<ushort> getRegisterGroup(int groupindex)
        {
            switch (groupindex)
            {
                case 3: return _slaveDataStore.HoldingRegisters;
                case 4: return _slaveDataStore.InputRegisters;
                default: return _slaveDataStore.InputRegisters;
            }
        }

        private void setValue16(int groupindex, int offset, ushort value)
        {
            var data = getRegisterGroup(groupindex);
            data.WritePoints((ushort)offset, new ushort[] { value });
        }

        private void setValue32(int groupindex, int offset, float value)
        {
            ushort lowOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(value), 0);
            ushort highOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(value), 2);
            var data = getRegisterGroup(groupindex);
            var bytes = new ushort[] { lowOrderValue, highOrderValue };
            data.WritePoints((ushort)offset, bytes);
        }

        private void setValue32(int groupindex, int offset, int value)
        {
            byte[] valueBuf = BitConverter.GetBytes(value);
            ushort lowOrderValue = BitConverter.ToUInt16(valueBuf, 0);
            ushort highOrderValue = BitConverter.ToUInt16(valueBuf, 2);

            var data = getRegisterGroup(groupindex);
            var bytes = new ushort[] { lowOrderValue, highOrderValue };
            data.WritePoints((ushort)offset, bytes);
        }

        private DataTable LoadData(string dataSet)
        {
            var fileName = dataSet;
            CSVReader.readCSV(fileName, out var dataTable);
            return dataTable;
        }
    }
}
