using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class SampleForJabraEvolve2_65Flex
{
  public static async void ReadWriteObserve(IDevice device, IPropertyFactory jabraSdkPropsFactory)
  {
    //Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them.
    string[] propertyNames = {
        "batteryLevel"
    };
    IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

    //Read properties from device
    var batteryLevel = await propertyMap["batteryLevel"].Get();
    Console.WriteLine($"Properties read from '{device.Name}':");
    Console.WriteLine($"  * batteryLevel: {batteryLevel}");

    Console.WriteLine("Observing battery level to keep getting updates when the headset reports a change");
    SampleHelpers.ObserveProperty(propertyMap, "batteryLevel"); // Observe the property batteryLevel
    }
}