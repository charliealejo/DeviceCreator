using DeviceManager;
using Shared.Models;
using System;

namespace CDeviceCreator
{
    /// <summary>
    /// Main class.
    /// </summary>
    class MainClass
    {
        /// <summary>
        /// Instance of the device to save.
        /// </summary>
        private static IDevice device;

        /// <summary>
        /// The instance of the class to send commands to the database.
        /// </summary>
        private static IDevicePublisher dbAccess;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// 
        /// This program allows the user to add devices into the database. First the
        /// device type is asked, then the particular properties for the given type.
        /// Finally the user is asked if they want to add another device.
        /// </summary>
        public static void Main()
        {
            dbAccess = new DevicePublisher();

            Console.WriteLine("Welcome to the device creator!");
            Console.WriteLine("------------------------------");

            bool doItAgain;

            do
            {
                bool error;
                int type = 0;
                do
                {
                    error = false;
                    Console.Write("Device type (1: water meter, 2: elec. meter, 3: gateway): ");
                    try
                    {
                        type = int.Parse(Console.ReadLine());
                        if (type < 1 || type > 3)
                        {
                            error = true;
                        }
                    }
                    catch
                    {
                        error = true;
                    }
                    if (error) Console.WriteLine("Invalid type, try again.");
                } while (error);

                device = type == 1 ? new WaterMeter() : type == 2 ? new ElectricityMeter() : (IDevice)new Gateway();

                readValue(device, "ID", "ID", true);
                readValue(device, "Serial number", "SerialNumber", true);
                readValue(device, "Firmware version", "FirmwareVersion", false);
                readValue(device, "State", "State", false);
                if (type == 3)
                {
                    readValue(device, "IP", "IP", false);
                    readValue(device, "Port", "Port", false);
                }

                saveData(device);

                Console.WriteLine("\nSave another device (Y/n)? ");
                var oneMore = Console.ReadKey().Key;
                Console.WriteLine();
                doItAgain = (oneMore == ConsoleKey.Enter || oneMore == ConsoleKey.Y);
            } while (doItAgain);
        }

        /// <summary>
        /// Reads a value into a field of the device form.
        /// </summary>
        /// <param name="device">Form.</param>
        /// <param name="textToShow">Text to show.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="mandatory">If set to <c>true</c> the value to read is mandatory.</param>
        private static void readValue(
            IDevice device, string textToShow, string propertyName, bool mandatory)
        {
            bool error;
            var property = device.GetType().GetProperty(propertyName);
            do
            {
                error = false;
                Console.Write(textToShow + ": ");
                var value = Console.ReadLine();
                if (mandatory && string.IsNullOrWhiteSpace(value))
                {
                    error = true;
                    Console.WriteLine("Value cannot be empty, try again.");
                }
                else if (property.PropertyType.Equals(typeof(string)))
                {
                    property.SetValue(device, value);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        property.SetValue(device, null);
                    }
                    else
                    {
                        property.SetValue(device, int.Parse(value));
                    }
                }
            } while (error);
        }

        /// <summary>
        /// Saves the device data.
        /// </summary>
        /// <param name="device">Device to save.</param>
        private static void saveData(IDevice device)
        {
            try
            {
                if (dbAccess.SaveDevice(device))
                {
                    Console.WriteLine("Device generated and saved successfully.");
                }
                else
                {
                    Console.WriteLine(
                        "A device with the same ID and serial number is already present in the database.");
                }
            }
            catch
            {
                Console.WriteLine("An error occurred while saving the device");
            }
        }
    }
}
