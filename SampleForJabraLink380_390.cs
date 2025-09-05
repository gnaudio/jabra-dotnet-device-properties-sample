using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.DeviceConnector.Protocol.Events;
using Jabra.NET.Sdk.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class SampleForJabraLink380_390
{ 
    public static async void ReadWriteObserve(IDevice device, IPropertyFactory jabraSdkPropsFactory)
    {
        // Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them.
        string[] propertyNames = {
            "bluetoothLinkQuality", "firmwareVersion"
        };
        IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

        //Read properties from device
        PropertyValue firmwareVersion = await propertyMap["firmwareVersion"].Get();
        Console.WriteLine($"Properties read from '{device.Name}':");
        Console.WriteLine($"  * Firmware version: {firmwareVersion}");

        // Please note that BT Link quality will only be reported when the Link 380/390 is connected to a headset and the headset is active in a call. 
        // BT Link quality is not reported when the headset is idle or when not in a call (e.g. listening to music). 
        Console.WriteLine($"Observing bluetooth link quality for {device.Name}");
        SampleHelpers.ObserveProperty(propertyMap, "bluetoothLinkQuality"); // Observe the property bluetoothLinkQuality
    }
}

