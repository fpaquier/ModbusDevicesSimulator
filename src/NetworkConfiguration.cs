using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using ModbusDevicesSimulator.Objects;

namespace ModbusDevicesSimulator
{
    public class NetworkConfiguration : INetworkConfiguration
    {
        private NetworkDescription _networkDescription = null;

        public NetworkDescription Network { get { return _networkDescription; } }

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            Converters = {
                new JsonStringEnumConverter()
            },
        };

        public void Load(string fileName)
        {
            try
            {
                var content = File.ReadAllText(fileName);
                _networkDescription = JsonSerializer.Deserialize<NetworkDescription>(content, _jsonOptions);
            }
            catch (Exception err)
            {
                Console.WriteLine($"Failed to load configuration: {err.Message}");
            }
        }
    }
}
