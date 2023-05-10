using System;
using System.CommandLine;
using System.Threading;

using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator
{
    public class Driver
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private static void Main(string[] args)
        {
            try
            {
                var analyzer = CreateCommandAnalyzer();

                analyzer.Invoke(args);

                Console.WriteLine("Hit a key to stop.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
            _cancellationTokenSource.Cancel();
        }

        private static RootCommand CreateCommandAnalyzer()
        {
            var fileArgument = new Argument<string>
               ("config-file", "Config file to use.");

            var networkConfig = new Option<string>(
                    name: "--mode",
                    description: "network mode to use.").FromAmong("tcp", "rtu");

            var rootCommand = new RootCommand("Modbus Slave Simulator");
            rootCommand.AddArgument(fileArgument);
            rootCommand.AddOption(networkConfig);

            rootCommand.SetHandler((network, file) =>
                {
                    OnExecuteCommand(network, file);
                },
                networkConfig,
                fileArgument);

            return rootCommand;
        }

        private static void OnExecuteCommand(string mode, string file)
        {
            try
            {
                var networkConfig = new NetworkConfiguration();
                networkConfig.Load(file);

                var networkDescr = GetNetwork(networkConfig.Network, mode);

                var network = NetworkFactory.CreateNetwork(networkDescr);

                network.Start(_cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static NetworkDescription GetNetwork(NetworkDescription network, string mode)
        {
            if (!string.IsNullOrEmpty(mode))
            {
                if (mode == "rtu")
                {
                    network.Mode = Objects.NetworkType.rtu;
                }
                else
                {
                    network.Mode = Objects.NetworkType.tcp;
                }
            }
            return network;
        }
    }
}


