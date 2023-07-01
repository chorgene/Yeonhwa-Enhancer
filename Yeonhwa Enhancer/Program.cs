using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

class Program
{
    const int PROCESS_ALL_ACCESS = 0x1F0FFF;  // Desired access rights for the process

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    static extern bool CloseHandle(IntPtr hObject);

    static void Main()
    {
        try
        {
            Process process = Process.GetProcessesByName("MapleStory")[0];

            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            List<IntPtr> addresses = new List<IntPtr>()
            {
            new IntPtr((long)process.MainModule.BaseAddress + 0x5240EA0), //1st 3 Character Names
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C6510), //Claw Damage
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C64B8), //Wand, Staff, Shining Rod, Psy Limiter, Magic Gauntlet, One-handed Blunt Weapon Damage
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C64D0), //Two-handed Sword, Two-handed Axe
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C64C8), //Bow, Dagger, Dual Bowguns, Cane, Desperado, Energy Chain (Cadena), Ancient Bow, Buchae, Tuner, Breath Shooter
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C64D8), //Crossbow, Fan
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C64F0), //Spear, Polearm, Great Sword (Lapis)
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C64F8), //Gun, Cannon
            new IntPtr((long)process.MainModule.BaseAddress + 0x46C6508), //Knuckle, Soul Shooter, Arm Cannon/Revolver (Blaster)
            };

            IntPtr characterDamageAddress = new IntPtr((long)process.MainModule.BaseAddress + 0x46C64D8);

            double newValue;
            string configFileName = "config.txt";

            if (!File.Exists(configFileName))
            {
                File.WriteAllText(configFileName, "10");  // Create the file with default value of 10
            }

            // Read newValue from config.txt
            try
            {
                string newValueString = File.ReadAllText(configFileName);
                newValue = double.Parse(newValueString);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading newValue from config.txt: {e}");
                return;
            }

            byte[] newValueBytes = BitConverter.GetBytes(newValue);

            int bytesWritten;
            bool successWriteMemory = WriteProcessMemory(processHandle, characterDamageAddress, newValueBytes, (uint)newValueBytes.Length, out bytesWritten);

            CloseHandle(processHandle);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong. Please contact the developer.");
            Console.WriteLine($"Error Message: {ex}");
            Console.ReadLine();
        }

    }
}
