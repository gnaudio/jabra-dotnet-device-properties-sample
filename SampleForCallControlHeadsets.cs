using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class SampleForCallControlHeadsets
{
    public static async void Observe(IDevice device, IPropertyFactory jabraSdkPropsFactory)
    {
        //Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them.
        string[] propertyNames = {
        "volumeUp",
        "volumeDown",
        "offHook",
        "lineBusy",
        "muteChanged"
    };
        IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

        // Observing various properties for headsets.
        // Please note, that these properties are NOT for implementing call controls - that should be done using the EasyCallControl module. 
        // Instead this part of the sample is to demonstrate how to observe properties for applications gathering usage telemetry from headsets. 
        // The events correspond to the headset buttons for volume up and volume down. Off-hook and line busy events are related to when a headset enters call state.
        // The muteChanged event is for mute state changes requested by the headset. Does not report mute state changes if not initiated by headset. 
        // Important note: muteChanged is "true" whenever the state is requested to change - it does not report the absolute mute state of the headset.
        Console.WriteLine("Observing volumeUp, volumeDown, offHook, lineBusy and muteChanged events.");
        SampleHelpers.ObserveProperty(propertyMap, "volumeUp"); // Observe the property volumeUp for volume up events.
        SampleHelpers.ObserveProperty(propertyMap, "volumeDown"); // Observe the property volumeDown for volume down events.
        SampleHelpers.ObserveProperty(propertyMap, "offHook"); // Observe the property offHook for off-hook events.
        SampleHelpers.ObserveProperty(propertyMap, "lineBusy"); // Observe the property lineBusy for line busy events.
        SampleHelpers.ObserveProperty(propertyMap, "muteChanged"); // Observe the property muteChanged for mute state changes requested by headset. 
    }
}