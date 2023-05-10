using ModbusDevicesSimulator.Objects;
using Xunit;

namespace ModbusDevicesSimulator.UnitTest
{
    public class DeviceStorageTests
    {
        private DeviceStorage _deviceStorage = new DeviceStorage();

        [Fact]
        public void Creation()
        {
            Assert.NotNull(_deviceStorage.CoilDiscretes);
            Assert.NotNull(_deviceStorage.CoilInputs);
            Assert.NotNull(_deviceStorage.HoldingRegisters);
            Assert.NotNull(_deviceStorage.InputRegisters);
        }

        [Fact]
        public void CoilDiscretesReadWrite()
        {
            Assert.False(_deviceStorage.CoilDiscretes[0]);

            _deviceStorage.CoilDiscretes[0]= true;
            Assert.True(_deviceStorage.CoilDiscretes[0]);
        }

        [Fact]
        public void CoilInputsReadWrite()
        {
            Assert.False(_deviceStorage.CoilInputs[1]);

            _deviceStorage.CoilInputs[1] = true;
            Assert.True(_deviceStorage.CoilInputs[1]);
        }

        [Fact]
        public void HoldingRegistersReadWrite()
        {
            Assert.Equal(0, _deviceStorage.HoldingRegisters[0]);

            _deviceStorage.HoldingRegisters[0] = 42;
            Assert.Equal(42, _deviceStorage.HoldingRegisters[0]);
        }

        [Fact]
        public void InputRegistersReadWrite()
        {
            Assert.Equal(0, _deviceStorage.InputRegisters[0]);

            _deviceStorage.InputRegisters[0] = 52;
            Assert.Equal(52, _deviceStorage.InputRegisters[0]);
        }
    }
}
