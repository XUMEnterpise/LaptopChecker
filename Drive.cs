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
            string converted;
            double psize = 1;
            bool found = false;
            while (!found)
            {
                if (psize < 8)
                {
                    if (psize / size >= 0.9 && psize / size <= 1.1)
                    {
                        found = true;
                    }
                    else
                    {
                        psize++;
                    }
                }
                /*else if (psize > size) {
                    found = true;
                    psize=Math.Ceiling(size);
                }*/
                else
                {
                    if (psize / size >= 0.9 && psize / size <= 1.1)
                    {
                        found = true;
                    }
                    else
                    {
                        psize *= 2;
                    }
                }

            }

            if (psize % 1024 == 0)
            {
                double i = psize / 1024;
                converted = i.ToString() + " TB";
            }
            else
            {
                converted = psize.ToString() + " " + prefix;
            }
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
