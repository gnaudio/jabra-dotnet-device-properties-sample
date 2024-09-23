using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class SampleForJabraPanacast50
{
  public static async void ReadWriteObserve(IDevice device, IPropertyFactory jabraSdkPropsFactory)
  {
    //Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them
    string[] propertyNames = [
      "zoomMode2",
      "firmwareVersion",
      "peopleCount",
    ];
    IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

    //Read properties from device. See README.md for details on each setting.
    PropertyValue firmwareVersion = await propertyMap["firmwareVersion"].Get();
    PropertyValue zoomMode2 = await propertyMap["zoomMode2"].Get();
    Console.WriteLine($"Properties read from '{device.Name}':");
    Console.WriteLine($"  * Firmware version: {firmwareVersion}");
    Console.WriteLine($"  * Zoom mode: {zoomMode2}");

    //Write properties (not requireing reboot) to device.
    IPropertyTransaction transaction = propertyMap.StartTransaction();
    transaction.Set("zoomMode2", new StringPropertyValue("activeSpeaker")); //Valid values: "fullScreen" | "intelligentZoom" | "activeSpeaker"
    await transaction.Commit();

    //Read the changed property back out
    zoomMode2 = await propertyMap["zoomMode2"].Get();
    Console.WriteLine($"Properties written to '{device.Name}':");
    Console.WriteLine($"  * Zoom mode: {zoomMode2}");

    SampleHelpers.ObserveProperty(propertyMap, "peopleCount");
  }
}