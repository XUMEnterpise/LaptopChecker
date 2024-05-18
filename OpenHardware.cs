using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class OpenHardware
    {

            private double cpuTemp = 0;
            static Computer c = new Computer()
            {
                GPUEnabled = true,
                CPUEnabled = true,
                RAMEnabled = true,
                MainboardEnabled = true,
                FanControllerEnabled = true,
                HDDEnabled = true,

            };
            /// <summary>
            /// Opens access to values of pc sensors
            /// </summary>
            public void openPC()
            {
                closePC();
                c.Open();
            }
            public void closePC()
            {
                c.Close();
            }
            /// <summary>
            /// Returns temp of Inputed hardware type
            /// </summary>
            /// <param name="type">Hardware type parameter</param>
            /// <param name="name">Name of the sensor trying to get access to</param>
            /// <returns>float value of tempartature</returns>
            public double DisplayTemp(HardwareType type, string name)
            {

                foreach (var hardware in c.Hardware)
                {
                    if (hardware.HardwareType == type)
                    {
                        hardware.Update();

                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains(name))
                            {
                                cpuTemp = sensor.Value.GetValueOrDefault();
                            }
                        }
                    }
                }

                return cpuTemp;
            }
            /// <summary>
            /// Displays the name of the hardware inputed
            /// </summary>
            /// <param name="type">Hardware type trying to get name of</param>
            /// <returns>Returns string value of the hardware name</returns>
            public string DisplayName(HardwareType type)
            {
                string cpuName = "Unknown";
                try
                {
                    foreach (var hardware in c.Hardware)
                    {
                        if (hardware.HardwareType == type)
                        {
                            hardware.Update();
                            cpuName = hardware.Name;
                        }
                    }
                }
                catch
                {
                    cpuName = "Error";
                }

                return cpuName;
            }
            /// <summary>
            /// Returns min value of specific sensor
            /// </summary>
            /// <param name="type">Type of hardware</param>
            /// <param name="sensorName">Sensor name</param>
            /// <returns>Min value of sensor in float</returns>
            public double? minValue(HardwareType type, string sensorName)
            {
                double? min = 0.0f;
                foreach (var hardware in c.Hardware)
                {
                    if (hardware.HardwareType == type)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains(sensorName))
                            {
                                min = sensor.Min;
                            }
                        }
                    }
                }
                return min;
            }
            /// <summary>
            /// Returns max value of specific sensor
            /// </summary>
            /// <param name="type">Hardware type</param>
            /// <param name="sensorName">Sensor name</param>
            /// <returns>Max value of sensor in float</returns>
            public double? maxValue(HardwareType type, string sensorName)
            {
                double? max = 0.0f;
                foreach (var hardware in c.Hardware)
                {
                    if (hardware.HardwareType == type)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains(sensorName))
                            {
                                max = sensor.Max;
                            }
                        }
                    }
                }
                return max;
            }

            /// <summary>
            /// Checks if hardware exists
            /// </summary>
            /// <param name="type"></param>
            /// <returns>True if it does false in not</returns>
            public bool isThere(HardwareType type)
            {
                foreach (var hardware in c.Hardware)
                {
                    if (hardware.HardwareType == type)
                    {
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// Returns ammount of ram in the system by adding used and free ram togheter
            /// </summary>
            /// <returns>Float value of ammout of ram in the system</returns>
            public double availableMemory()
            {
                double ram = 0;
                double ram1 = 0;
                foreach (var hardware in c.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.RAM)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Data && sensor.Name.Contains("Available Memory"))
                            {
                                ram = sensor.Value.GetValueOrDefault();
                            }
                            if (sensor.SensorType == SensorType.Data && sensor.Name.Contains("Used Memory"))
                            {
                                ram1 = sensor.Value.GetValueOrDefault();
                            }
                        }
                    }
                }
                return ram + ram1;
            }
            public double? getCPU()
            {
                return DisplayTemp(HardwareType.CPU, "CPU Package");
            }
            public double? getGPU()
            {
                if (isThere(HardwareType.GpuAti))
                {
                    return DisplayTemp(HardwareType.GpuAti, "GPU Core");
                }
                else if (isThere(HardwareType.GpuNvidia))
                {
                    return DisplayTemp(HardwareType.GpuNvidia, "GPU Core");
                }
                else return 0;
            }
            public double? getMinGPU()
            {
                if (isThere(HardwareType.GpuAti))
                {
                    return minValue(HardwareType.GpuAti, "GPU Core");
                }
                else if (isThere(HardwareType.GpuNvidia))
                {
                    return minValue(HardwareType.GpuNvidia, "GPU Core");
                }
                else return 0;
            }
            public double? getMaxGPU()
            {
                if (isThere(HardwareType.GpuAti))
                {
                    return maxValue(HardwareType.GpuAti, "GPU Core");
                }
                else if (isThere(HardwareType.GpuNvidia))
                {
                    return maxValue(HardwareType.GpuNvidia, "GPU Core");
                }
                else return 0;
            }
            public double? getMinCPU()
            {
                return minValue(HardwareType.CPU, "CPU Package");
            }
            public double? getMaxCPU()
            {
                return maxValue(HardwareType.CPU, "CPU Package");
            }
            public string GetCPUName()
            {
                return DisplayName(HardwareType.CPU).ToUpper();
            }
            public string GetGPUName()
            {
                if (isThere(HardwareType.GpuAti))
                    return DisplayName(HardwareType.GpuAti).ToUpper();
                else if (isThere(HardwareType.GpuNvidia))
                    return DisplayName(HardwareType.GpuNvidia).ToUpper();
                else
                    return "ONBOARD";
            }
            public string GetMBName()
            {
                return DisplayName(HardwareType.Mainboard);
            }
            public string GetMemory()
            {
                return Math.Ceiling(availableMemory()).ToString() + " GB";
            }
        
    }
}
