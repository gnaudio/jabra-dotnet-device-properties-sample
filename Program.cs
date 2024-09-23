using System.Reactive.Linq;
using Jabra.NET.Sdk.Core;
using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class Program
{
  static void Main()
  {
    Console.WriteLine("Jabra SDK Device Settings Sample app starting. Press any key to end.\n");
    Start();
    Console.ReadKey(); //Keep the console app running until a key is pressed
  }

  static async void Start()
  {
    //Initialize the core SDK
    var config = new Config(
        partnerKey: "get-partner-key-at-developer.jabra.com",
        appId: "JabraDotNETSettingsSample",
        appName: "Jabra .NET Settings Sample"
    );
    IApi jabraSdk = Init.InitSdk(config);

    //Subscribe to SDK log events
    jabraSdk.LogEvents.Subscribe((log) =>
    {
      if (log.Level == LogLevel.Error) Console.WriteLine(log.ToString());
      //Ignore info, warning, and debug log messages
    });

    //Initialize the SDK's properties module
    PropertyModule jabraSdkProps = new PropertyModule(jabraSdk); // In principle you can write just "new(jabraSdk);" if you prefer to be more concise - keeping the full name for clarity. 
    IPropertyFactory jabraSdkPropsFactory = await jabraSdkProps.CreatePropertyFactory();

    //Subscribe to Jabra devices being attached/detected by the SDK
    Console.WriteLine("Looking for Jabra devices...\n");
    jabraSdk.DeviceAdded.Subscribe(async (IDevice device) =>
    {
      Console.WriteLine($"> Device attached/detected: {device.Name} (Product ID: {device.ProductId}, Serial #: {device.SerialNumber})");
      switch (device.Name)
      {
        case "Jabra PanaCast 50":
          SampleForJabraPanacast50.ReadWriteObserve(device, jabraSdkPropsFactory);
          break;
        case "Jabra Engage 50 II":
          SampleForEngage50II.ReadWriteObserve(device, jabraSdkPropsFactory);
          break;
      }
    });

    jabraSdk.DeviceRemoved.Subscribe((IDevice device) =>
    {
      Console.WriteLine($"< Device detached/reboots: {device.Name} (Product ID: {device.ProductId}, Serial #: {device.SerialNumber})");
    });
  }
}
