using Jabra.NET.Sdk.Properties;

internal class SampleHelpers
{
  public static void ObserveProperty(IPropertyMap propertyMap, string propertyName)
  {
    IObservable<PropertyValue> propertyWatcher = propertyMap[propertyName].Watch;
    propertyWatcher.Subscribe(value =>
    {
      Console.WriteLine($"Property change observed on '{propertyMap.Device.Name}':");
      Console.WriteLine($"  * Received value ({propertyName}): {value}");
    });
  }
}