using Jabra.NET.Sdk.Properties;
using Jabra.NET.Sdk.Properties.Validation;
using Json.More;

internal class SampleHelpers
{
    static int lastHandoverCount;
    static bool isFirstHandoverCount = true;

    public static void ObserveProperty(IPropertyMap propertyMap, string propertyName)
    {
        IObservable<PropertyValue> propertyWatcher = propertyMap[propertyName].Watch;
        propertyWatcher.Subscribe(value =>
        {
            Console.WriteLine($"Property change observed on '{propertyMap.Device.Name}':");
            Console.WriteLine($"  * Received value ({propertyName}): {value}");
        });
    }

    public static void ObserveDectErrors(IPropertyMap propertyMap)
    {
        IObservable<PropertyValue> propertyWatcher = propertyMap["dectErrors"].Watch;
        propertyWatcher.Subscribe(value =>
        {
            // "dectErrors" property returns an object containing multiple PropertyValue instances, so first we interpret the value as an ObjectPropertyValue
            if (value is ObjectPropertyValue dectErrors)
            {
                // The specific value we're looking for is handoverCount
                // handoverCount is absolute for the device and we need to calculate the difference since last update to know if there has been any new handover errors.
                if (dectErrors.TryGetProperty("handoverCount", out PropertyValue handoverCount) && handoverCount is IntegerPropertyValue integerHandoverCount)
                {
                    int handoverCountValue = integerHandoverCount.Value;
                    // First handoverCount received is just to initialize lastHandoverCount
                    if (!isFirstHandoverCount)
                    {
                        int handoverErrorsSinceLast = handoverCountValue - lastHandoverCount;
                        // We only deal with error count 0 and above. If negative number it indicates that the device counter was reset since last readout and should be ignored. 
                        if (handoverErrorsSinceLast >= 0)
                        {
                            Console.WriteLine($"Property change observed on '{propertyMap.Device.Name}':");
                            Console.WriteLine($"  * DECT handover errors since last update: {handoverErrorsSinceLast}");
                            // More than 10 errors since last update (every 8 seconds from the DECT headset) indicates that audio quality was likely impacted in our experience. 
                            if (handoverErrorsSinceLast > 10)
                            {
                                Console.WriteLine("  * Warning: High number of DECT handover errors - likely to impact audio quality");
                            }
                            else
                            {
                                Console.WriteLine("  * Note: Number of handover errors is low - no noticeable impact on audio quality");
                            }
                        }
                    }
                    lastHandoverCount = handoverCountValue;
                    isFirstHandoverCount = false;
                }
            }
        });
    }
}
