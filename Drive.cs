using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Drive
    {
        private string boot = "N/A", sec = "N/A";
        public Drive()
        {
            GetDriveInfo();
        }
        /// <summary>
        /// Formats bytes value into highest value up to PB.
        /// It works by dividing bytes by 1000 and checking if it <1000 and adding +1 to prefix if it is
        /// that is the value whatever the prefix is
        /// </summary>
        /// <param name="bytes">Size of a disk in bytes to be converted</param>
        /// <returns>String that is formated value of long bytes</returns>
        public string FormatBytes(long bytes)
        {

            string[] Suffix = { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1000; i++, bytes /= 1000)
            {
                dblSByte = bytes / 1000.0;
            }
            string[] finished = ConvertToClosest(dblSByte, Suffix[i]).Split(' ');
            return String.Format("{0:0.##} {1}", finished[0], finished[1]);
        }
        public void GetDriveInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            try
            {
                foreach (DriveInfo drive in allDrives)
                {
                    if (drive.IsReady)
                    {
                        string driveFormat = FormatBytes(drive.TotalSize);
                        double drivesize = Double.Parse(driveFormat.TrimEnd('G', 'B', 'T', 'M', 'K'));

                        if (drive.DriveType.ToString() == "Fixed")
                        {
                            if (drive.Name.Contains("C:"))
                            {
                                boot = driveFormat;
                            }
                            else if (drive.Name.Contains("D:"))
                            {
                                sec = driveFormat;
                            }
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error with drive get");
            }
        }
        /// <summary>
        /// Converts the inputed size to the closest size. 1000gb = 1tb 127gb=128gb
        /// </summary>
        /// <param name="size">Double value needed converting</param>
        /// <param name="prefix">Pefix of the size ie kb gb tb</param>
        /// <returns>Returns rounded value</returns>
        public string ConvertToClosest(double size, string prefix)
        {
            // Define the list of predefined sizes in GB
            double[] predefinedSizes = { 128, 256, 512, 1024, 2048, 4096, 8192 }; // Added up to 8 TB (8192 GB)

            // Convert TB to GB if necessary
            if (prefix.ToLower() == "tb")
            {
                size *= 1024; // Convert TB to GB
            }

            // Find the closest size using a simple comparison
            double closestSize = predefinedSizes[0];
            double minDifference = Math.Abs(size - closestSize);

            for (int i = 1; i < predefinedSizes.Length; i++)
            {
                double currentDifference = Math.Abs(size - predefinedSizes[i]);
                if (currentDifference < minDifference)
                {
                    closestSize = predefinedSizes[i];
                    minDifference = currentDifference;
                }
            }

            // Determine the appropriate prefix for the closest size
            string outputPrefix = "GB";
            if (closestSize >= 1024)
            {
                closestSize /= 1024;
                outputPrefix = "TB";
            }

            // Construct the output string
            string converted = closestSize + " " + outputPrefix;
            return converted;
        }
        public string getBoot()
        {
            return boot;
        }
        public string getSec()
        {
            return sec;
        }
    }
}
