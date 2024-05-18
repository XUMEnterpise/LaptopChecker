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
        Laptop laptop = new Laptop();
        public Form1()
        {

            InitializeComponent();
            
            
            modelValue.Text = laptop.model;
            ssdValue.Text = laptop.storage;
            serialNumberValue=laptop.serialNumber;
            ramValue.Text=laptop.memory;
            cpuValue.Text = laptop.CPU;
            LoadLabel();
            
            
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

        private void button1_Click(object sender, EventArgs e)
        {
            TCPClient client = new TCPClient();
            Laptop testLaptop = new Laptop
            {
                memory="8 GB",
                model= "HP EliteBook 840 G3",
                storage="256 GB",
                serialNumber= serialNumberValue,
                CPU="i7-6300U"
            };
            client.SendPacket(laptop);
        }
    }
}
