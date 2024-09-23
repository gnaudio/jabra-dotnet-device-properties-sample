using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class SampleForJabraEngage50II
{
  public static async void ReadWriteObserve(IDevice device, IPropertyFactory jabraSdkPropsFactory)
  {
    //Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them.
    string[] propertyNames = [
        "firmwareVersion",
        "smartRingerEnabled",
        "backgroundNoiseLevel"
    ];
    IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

    //Read properties from device
    PropertyValue firmwareVersion = await propertyMap["firmwareVersion"].Get();
    PropertyValue smartRingerEnabled = await propertyMap["smartRingerEnabled"].Get();
    Console.WriteLine($"Properties read from '{device.Name}':");
    Console.WriteLine($"  * Firmware version: {firmwareVersion}");
    Console.WriteLine($"  * Smart Ringer enabled: {smartRingerEnabled}");

    //Write properties to device
    IPropertyTransaction transaction = propertyMap.StartTransaction();
    transaction.Set("smartRingerEnabled", new BooleanPropertyValue(true));  //Valid values: true | false
    await transaction.Commit(); // Commit the transaction to write the property changes to the device. Depending on property type, this can trigger a reboot of a device in which case you will lose the connection to the device and cannot asume that subsequent lines will work. 

    //Read the changed property back out
    smartRingerEnabled = await propertyMap["smartRingerEnabled"].Get();
    Console.WriteLine($"Properties written to '{device.Name}':");
    Console.WriteLine($"  * Smart Ringer enabled: {smartRingerEnabled}");

    SampleHelpers.ObserveProperty(propertyMap, "backgroundNoiseLevel"); // Observe the property for background noise level in dB around the headset wearer. 
  }
}