using Jabra.NET.Sdk.Core.Types;
using Jabra.NET.Sdk.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class SampleForJabraEngage65_75
{
    public static async void ReadWriteObserve(IDevice device, IPropertyFactory jabraSdkPropsFactory)
    {
        //Define the properties you plan to interact with. This is not reading the property value from the device but prepares routines for interacting with them.
        string[] propertyNames = {
        "dectErrors"
    };
        IPropertyMap propertyMap = await jabraSdkPropsFactory.CreateProperties(device, propertyNames);

        Console.WriteLine("Observing DECT radio errors to keep getting updates when the headset reports a change");
        SampleHelpers.ObserveDectErrors(propertyMap); // Observe the property dectErrors
    }
}