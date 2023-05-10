using System;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;

using NModbus;
using NModbus.Serial;

using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator
{
    public class NetworkRtu : IModbusNetwork
    {
        private bool _isRunning;

        private SerialPort _serialPort;

        private SerialPortAdapter _serialAdapter;

        private NetworkDescription _network;

        private List<StoragePlayer> _players = new List<StoragePlayer>();

        private IModbusSlaveNetwork _modbusSlaveNetwork;

        public NetworkRtu(NetworkDescription network)
        {
            _isRunning = false;
            _network = network;
        }

        public void Start(CancellationToken token)
        {
            if (_isRunning)
            {
                Console.WriteLine("RTU Network already running");
                return;
            }

            try
            {
                Console.WriteLine($"Create RTU Network on Port: {GetPortSettings(_network.SerialSettings)}.");

                // Open and configure the serial port
                _serialPort = new SerialPort(_network.SerialSettings.Port);
                _serialPort.BaudRate = _network.SerialSettings.BaudRate;
                _serialPort.DataBits = _network.SerialSettings.DataBits;
                _serialPort.Parity = _network.SerialSettings.Parity;
                _serialPort.StopBits = _network.SerialSettings.StopBits;
                _serialPort.Open();

                _serialAdapter = new SerialPortAdapter(_serialPort);

                var factory = new ModbusFactory();

                _modbusSlaveNetwork = factory.CreateRtuSlaveNetwork(_serialAdapter);

                foreach (DeviceDescription device in _network.Devices)
                {
                    var player = new StoragePlayer(device);

                    IModbusSlave slave = factory.CreateSlave(device.SlaveId, player.DataStore);

                    _modbusSlaveNetwork.AddSlave(slave);

                    _players.Add(player);
                }

                var thread = new Thread(Run) { Name = "RTU_Network", IsBackground = true };
                thread.Start(token);

                _players.ForEach(player => { player.Start(token); });

                _isRunning = true;
                Console.WriteLine("network created...");
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error when creation RTU Network: {err.Message}");
            }
        }

        private async void Run(Object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            Console.WriteLine($"RTU Network running...");
            await _modbusSlaveNetwork.ListenAsync(token);
            Console.WriteLine("exited?");
        }

        static private string GetPortSettings(SerialSettings settings)
        {
            return $"{settings.Port} - {settings.BaudRate}/{settings.DataBits}/{settings.Parity}/{settings.StopBits}";
        }
    }
}
