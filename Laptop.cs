using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Laptop
    {
        public string model { get; set; }
        public string CPU { get; set; }
        public string storage { get; set; }
        public string memory { get; set; }
        public string serialNumber { get; set; }
        public Laptop()
        {
            OpenHardware openHardware = new OpenHardware();
            Drive drive = new Drive();
            model = GetModel();
            openHardware.openPC();
            CPU=openHardware.GetCPUName();
            memory = openHardware.GetMemory();
            openHardware.closePC();
            serialNumber=GetSystemSerialNumber();
            storage = drive.getBoot();
        }

        public static string GetSystemSerialNumber()
        {
            string serialNumber = string.Empty;

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");

                foreach (ManagementObject obj in searcher.Get())
                {
                    serialNumber = obj["SerialNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving the serial number: " + ex.Message);
            }

            return serialNumber;
        }
        public string GetModel()
        {
            // Create a query to select the model number from Win32_ComputerSystem
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Model FROM Win32_ComputerSystem");
            string model = "";
            // Execute the query and get the results
            foreach (ManagementObject obj in searcher.Get())
            {
                // Access the Model property of the result
                model = (string)obj["Model"];

                // Output the model number
                Console.WriteLine("Laptop Model: " + model);
            }
            return Regex.Replace(model, " ", "_");
        }
    }
}
