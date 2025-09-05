using System.Reactive.Linq;
using Jabra.NET.Sdk.Core;
using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Modules.EasyCallControl;
using Jabra.NET.Sdk.Properties;

internal class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Jabra .NET SDK Device Settings Sample app starting. Press ctrl+c or close the window to end.\n");

        //Initialize the core SDK. Recommended to use Init.InitManualSdk(...) (not Init.Init(...)) to allow setup of listeners before the SDK starts discovering devices.
        var config = new Config(
            partnerKey: "get-partner-key-at-developer.jabra.com",
            appId: "JabraDotNETSettingsSample",
            appName: "Jabra .NET Settings Sample"
        );
        IManualApi jabraSdk = Init.InitManualSdk(config);

        //Subscribe to SDK log events.
        jabraSdk.LogEvents.Subscribe((log) =>
        {
            if (log.Level == LogLevel.Error) Console.WriteLine(log.ToString());
            //Ignore info, warning, and debug log messages.
        });

        //Initialize the SDK's properties module
        IPropertyModule jabraSdkProps = new PropertyModule(jabraSdk);
        IPropertyFactory jabraSdkPropsFactory = await jabraSdkProps.CreatePropertyFactory();

        //Setup listeners for Jabra devices being attached/detected.
        SetupDeviceListeners(jabraSdk, jabraSdkPropsFactory);

        // Enable the SDK's device discovery AFTER listeners and other necessary infrastructure is setup.
        await jabraSdk.Start();
        Console.WriteLine("Now listening for Jabra devices...\n");

        //Keep the sample app running until actively closed.
        Task.Delay(-1).Wait();
    }

    static void SetupDeviceListeners(IApi jabraSdk, IPropertyFactory jabraSdkPropsFactory)
    {
        //Subscribe to Jabra devices being attached/detected by the SDK
        jabraSdk.DeviceAdded.Subscribe((IDevice device) =>
        {
            Console.WriteLine($"> Device attached/detected: {device.Name} (Product ID: {device.ProductId}, Serial #: {device.SerialNumber})");

            //Determine which sample to run based on the device detected.
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

                case "Jabra Evolve2 65 Flex":
                    SampleForJabraEvolve2_65Flex.ReadWriteObserve(device, jabraSdkPropsFactory);
                    break;
                case "Jabra Engage 65":
                    SampleForJabraEngage65_75.ReadWriteObserve(device, jabraSdkPropsFactory);
                    break;
                case "Jabra Engage 75":
                    SampleForJabraEngage65_75.ReadWriteObserve(device, jabraSdkPropsFactory);
                    break;
                case "Jabra Link 380": 
                    SampleForJabraLink380_390.ReadWriteObserve(device, jabraSdkPropsFactory);
                    break;
                case "Jabra Link 390":
                    SampleForJabraLink380_390.ReadWriteObserve(device, jabraSdkPropsFactory);
                    break;
            }

            // If device supports call controls, subscribe to call control related telemetry events.
            var easyCallControlFactory = new EasyCallControlFactory(jabraSdk);
            if (easyCallControlFactory.SupportsEasyCallControl(device))
            {
                SampleForCallControlHeadsets.Observe(device, jabraSdkPropsFactory);
            }
        });

        //Subscribe to Jabra devices being detached/rebooted
        jabraSdk.DeviceRemoved.Subscribe((IDevice device) =>
        {
            Console.WriteLine($"< Device detached/reboots: {device.Name} (Product ID: {device.ProductId}, Serial #: {device.SerialNumber})");
        });
    }
}
