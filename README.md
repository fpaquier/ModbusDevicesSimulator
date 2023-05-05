# Modbus Devices Simulator

This utility is able to simulate several Modbus devices on a single network (TCP or RTU).

## References

This sample is based on:

- [modbusPlcSimulator](https://github.com/alongL/modbusPlcSimulator) project.
- It uses [NModbus 3.0.78](https://www.nuget.org/packages/NModbus)

## How to use

launch it from the command prompt with command like: `ModbusDevicesSimulator --config my-config.json`

### config file

The network is described through an input config file. See [config-2-device](./data/config-2-devices.json) for a sample.

### data file

data played by the simulator are stored in CSV files like [data-1](./data/data-1.csv).
