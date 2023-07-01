using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Yeonhwa_Enhancer
{
    public class Identifiers
    {
        public static string GetProcessorId()
        {
            var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
            var collection = searcher.Get();
            foreach (var obj in collection)
            {
                return obj["ProcessorId"].ToString();
            }
            return String.Empty;
        }

        public static string GetDiskDriveId()
        {
            var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");
            var collection = searcher.Get();
            foreach (var obj in collection)
            {
                return obj["SerialNumber"].ToString();
            }
            return String.Empty;
        }

        public static string GetHWID()
        {
            string registryPath = @"SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001";
            string valueName = "HwProfileGuid";
            string hwProfileGuid = "";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    object value = key.GetValue(valueName);
                    if (value != null)
                    {
                        hwProfileGuid = value.ToString();
                    }
                }
            }

            return hwProfileGuid;
        }

        public static string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                Console.WriteLine(ip.ToString());
            }
            return "cock";
        }
    }
}