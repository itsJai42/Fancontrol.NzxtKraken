using System;
using System.Collections.Generic;
using System.Linq;
using FanControl.Plugins;
using HidSharp;

namespace FanControl.NzxtKraken
{
    public class NzxtKrakenPlugin : IPlugin2
    {
        internal List<NzxtKrakenDevice> devices = new List<NzxtKrakenDevice>();
        internal IPluginLogger logger;

        public string Name => "NzxtKrakenPlugin";

        public NzxtKrakenPlugin(IPluginLogger pluginLogger)
        {
            logger = pluginLogger;
        }

        public void Initialize() {}

        public void Load(IPluginSensorsContainer _container)
        {
            var hidDevices = DeviceList.Local.GetHidDevices(0x1E71);

            foreach (var hidDevice in hidDevices)
            {
                logger.Log($"HID Device: PID=0x{hidDevice.ProductID:X4}, Path={hidDevice.DevicePath}, MaxIn={hidDevice.GetMaxInputReportLength()}, MaxOut={hidDevice.GetMaxOutputReportLength()}");
                if(NzxtKrakenX3.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKrakenX3(hidDevice, logger, _container));
                } else if (NzxtKrakenZ3.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKrakenZ3(hidDevice, logger, _container));
                } else if (NzxtKrakenX2.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKrakenX2(hidDevice, logger, _container));
                } else if (NzxtKrakenElite.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKrakenElite(hidDevice, logger, _container));
                } else if (NzxtKraken2023.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKraken2023(hidDevice, logger, _container));
                } else if (NzxtKrakenEliteV2.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKrakenEliteV2(hidDevice, logger, _container));
                } else if (NzxtKraken2024Plus.SupportsDevice(hidDevice))
                {
                    devices.Add(new NzxtKraken2024Plus(hidDevice, logger, _container));
                }
            }
        }

        public void Close()
        {
            devices.Clear();
        }
        public void Update()
        {
            foreach (NzxtKrakenDevice device in devices)
            {
                device.Update();
            }
        }
    }
}
