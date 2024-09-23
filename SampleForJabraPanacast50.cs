using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class SampleForJabraPanacast50
{

    public static async void ReadWriteWithReboot(IDevice device, IPropertyFactory jabraSdkPropsFactory)
    {
        string[] propertyNames = [
            "zoomMode2",
            "firmwareVersion",
            "peopleCount",
            "videoStitchMode2",
            "fieldOfView2",
            "triggerForRoomVideoDefaults",
            "intelligentZoomLatency",
            "plazaMode"
            ];
        IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

        //Read properties from device. See README.md for details on each setting.
        Console.WriteLine($"* Writing settings to Panacast 50 requiring the device to reboot. After reboot it will come back with a new attach event.");

        //Write properties (requiring reboot) to device.
        IPropertyTransaction transaction = propertyMap.StartTransaction();
        transaction.Set("videoStitchMode2", new StringPropertyValue("blend")); //Valid values: "blend" | "hybrid"
        transaction.Set("fieldOfView2", new StringPropertyValue("_140deg")); //Valid values: "_180deg" | "_140deg" | "_120deg" | "_90deg"
        transaction.Set("triggerForRoomVideoDefaults", new StringPropertyValue("endCall")); //Valid values: "endCall" | "pcUnplug"
        transaction.Set("intelligentZoomLatency", new IntegerPropertyValue(5)); //Valid values: integer 0-30
        transaction.Set("plazaMode", new StringPropertyValue("off")); //Valid values: "off" | "mode1" | "mode2"
        await transaction.Commit();
    }

    public static async void ReadWriteObserve(IDevice device, IPropertyFactory jabraSdkPropsFactory)
    {
        //Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them
        string[] propertyNames = [
          "zoomMode2",
          "firmwareVersion",
          "peopleCount",
          "videoStitchMode2",
          "fieldOfView2",
          "triggerForRoomVideoDefaults",
          "intelligentZoomLatency",
          "plazaMode"
        ];
        IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

        //Read properties from device. See README.md for details on each setting.
        PropertyValue firmwareVersion = await propertyMap["firmwareVersion"].Get();
        PropertyValue zoomMode2 = await propertyMap["zoomMode2"].Get();
        PropertyValue videoStitchMode2 = await propertyMap["videoStitchMode2"].Get();
        PropertyValue fieldOfView2 = await propertyMap["fieldOfView2"].Get();
        PropertyValue triggerForRoomVideoDefaults = await propertyMap["triggerForRoomVideoDefaults"].Get();
        PropertyValue intelligentZoomLatency = await propertyMap["intelligentZoomLatency"].Get();
        PropertyValue plazaMode = await propertyMap["plazaMode"].Get();
        Console.WriteLine($"Properties read from '{device.Name}':");
        Console.WriteLine($"  * Firmware version: {firmwareVersion}");
        Console.WriteLine($"  * Zoom mode: {zoomMode2}");
        Console.WriteLine($"  * Video Stitch mode: {videoStitchMode2}");
        Console.WriteLine($"  * Video field of view: {fieldOfView2}");
        Console.WriteLine($"  * Camera view default settings: {triggerForRoomVideoDefaults}");
        Console.WriteLine($"  * Automatic Zoom Speed: {intelligentZoomLatency}");
        Console.WriteLine($"  * Dynamic Composition mode: {plazaMode}");

        //Write properties (not requiring reboot) to device.
        IPropertyTransaction transaction = propertyMap.StartTransaction();
        transaction.Set("zoomMode2", new StringPropertyValue("activeSpeaker")); //Valid values: "fullScreen" | "intelligentZoom" | "activeSpeaker"
        await transaction.Commit();

        //Read the changed property back out
        zoomMode2 = await propertyMap["zoomMode2"].Get();
        Console.WriteLine($"Properties written to '{device.Name}' without triggering device reboot:");
        Console.WriteLine($"  * Zoom mode: {zoomMode2}");

        PropertyValue peopleCount = await propertyMap["peopleCount"].Get();
        Console.WriteLine($"  * People count initial value: {peopleCount}");
        Console.WriteLine("  * Observing 'peopleCount' property for changes. \nChange the number of people visible to the device to see the value change in the console.");
        SampleHelpers.ObserveProperty(propertyMap, "peopleCount"); //Observe the property 'peopleCount' for changes
    }
}