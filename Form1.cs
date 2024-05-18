using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string serialNumberValue = "";
        public Form1()
        {
            InitializeComponent();
            serialNumberValue = GetSystemSerialNumber();
            modelValue.Text = GetModel();
            Drive drive = new Drive();
            drive.GetDriveInfo();
            ssdValue.Text = drive.getBoot();
            OpenHardware c=new OpenHardware();
            c.openPC();
            cpuValue.Text=c.GetCPUName();
            ramValue.Text = c.GetMemory();
            c.closePC();
            LoadLabel();
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
        public void LoadLabel()
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 75
                }
            };

            // Generate the barcode image
            Bitmap barcodeBitmap = barcodeWriter.Write(serialNumberValue.ToUpper());
            barcode.Size=new Size(barcodeBitmap.Width, barcodeBitmap.Height);
            barcode.Image= barcodeBitmap;

            
           

        }
    }
}
