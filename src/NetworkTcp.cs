using System;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

using NModbus;

using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator
{
    public class NetworkTcp : IModbusNetwork
    {
        private bool _isRunning;

        private List<StoragePlayer> _players = new List<StoragePlayer>();

        private NetworkDescription _network;

        private IModbusSlaveNetwork _modbusSlaveNetwork;

        public NetworkTcp(NetworkDescription network)
        {
            _network = network;
            _isRunning = false;
        }

        public void Start(CancellationToken token)
        {
            if (_isRunning)
            {
                Console.WriteLine("Error: TCP network already running");
                return;
            }

            try
            {
                Console.WriteLine("Create the TCP Network");

                // create and open a TCP connection
                int port = _network.TcpSettings.Port;
                IPAddress address = IPAddress.Parse(_network.TcpSettings.Address);
                TcpListener slaveTcpListener = new TcpListener(address, port);
                slaveTcpListener.Start();

                var factory = new ModbusFactory();

                _modbusSlaveNetwork = factory.CreateSlaveNetwork(slaveTcpListener);

                foreach (DeviceDescription device in _network.Devices)
                {
                    var player = new StoragePlayer(device);

                    IModbusSlave slave = factory.CreateSlave(device.SlaveId, player.DataStore);

                    _modbusSlaveNetwork.AddSlave(slave);

                    _players.Add(player);
                }

                var thread = new Thread(Run) { Name = "TCP_Network", IsBackground = true };
                thread.Start(token);
                _players.ForEach(player => { player.Start(token); });

                _isRunning = true;
                Console.WriteLine("network created...");
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error when creation TCP Network: {err.Message}");
            }
        }

        private async void Run(Object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            Console.WriteLine("TCP Network running...");
            await _modbusSlaveNetwork.ListenAsync(token);
            Console.WriteLine("exited!");
        }
    }
}
