using System.Reactive.Linq;
using Jabra.NET.Sdk.Core;
using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;

internal class Program
{
  static void Main()
  {
    Console.WriteLine("Jabra .NET SDK Device Settings Sample app starting. Press ctrl+c or close the window to end.\n");
    Start();
    Task.Delay(-1).Wait(); //Keep the console app running.
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
    jabraSdk.DeviceAdded.Subscribe((IDevice device) =>
    {
      Console.WriteLine($"> Device attached/detected: {device.Name} (Product ID: {device.ProductId}, Serial #: {device.SerialNumber})");
      switch (device.Name)
      {
        case "Jabra PanaCast 50":
          Console.WriteLine("\tPress '1': To write settings requiring the device to reboot.\n\tPress any other key to read, write and observe properties not requiring device reboot.\nAwaiting your input...");
          var userSelection = Console.ReadKey(intercept: true);
          if (userSelection.KeyChar == '1')
            SampleForJabraPanacast50.ReadWriteWithReboot(device, jabraSdkPropsFactory);
          else
            SampleForJabraPanacast50.ReadWriteObserve(device, jabraSdkPropsFactory);
          break;

        case "Jabra Engage 50 II":
          SampleForJabraEngage50II.ReadWriteObserve(device, jabraSdkPropsFactory);
          break;
      }
    });

    jabraSdk.DeviceRemoved.Subscribe((IDevice device) =>
    {
      Console.WriteLine($"< Device detached/reboots: {device.Name} (Product ID: {device.ProductId}, Serial #: {device.SerialNumber})");
    });
  }
}
