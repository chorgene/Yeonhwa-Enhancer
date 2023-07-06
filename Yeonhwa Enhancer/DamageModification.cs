using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Yeonhwa_Enhancer
{
    public class DamageModification
    {

        public static void ModifyDamage()
        {
            const int PROCESS_ALL_ACCESS = 0x1F0FFF;  // Desired access rights for the process

            [DllImport("kernel32.dll")]
            static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll")]
            static extern bool CloseHandle(IntPtr hObject);

            try
            {
                Process process = Process.GetProcessesByName("MapleStory")[0];

                IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr characterName = new IntPtr((long)process.MainModule.BaseAddress + 0x5240EA0);

                Dictionary<IntPtr, double> addressDictionary = new Dictionary<IntPtr, double>
                {
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C6510), 1.75 }, // Claw Damage Base Value = 1.75 | 0
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C64B8), 1.20 }, // Wand, Staff, Shining Rod, Psy Limiter, Magic Gauntlet, One-handed Blunt Weapon Damage Base Value = 1.20 | 1
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C64D0), 1.34 }, // Two-handed Sword, Two-handed Axe Base Value = 1.34 | 2
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C64C8), 1.30 }, // Bow, Dagger, Dual Bowguns, Cane, Desperado, Energy Chain (Cadena), Ancient Bow, Buchae, Tuner, Breath Shooter Base Value = 1.30 | 3
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C64D8), 1.35 }, // Crossbow, Fan Base Value = 1.35 | 4
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C64F0), 1.49 }, // Spear, Polearm, Great Sword (Lapis) Base Value = 1.49 | 5
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C64F8), 1.50 }, // Gun, Cannon Base Value = 1.50 | 6
                    { new IntPtr((long)process.MainModule.BaseAddress + 0x46C6508), 1.70 }  // Knuckle, Soul Shooter, Arm Cannon/Revolver (Blaster) Base Value = 1.70 | 7
                };

                double multiplier = Authentication.GetModifier();
                int weapon = Authentication.GetWeapon();

                KeyValuePair<IntPtr, double> weaponItem = addressDictionary.ElementAt(weapon);
                IntPtr damageAddress = weaponItem.Key;
                double damageValue = weaponItem.Value;

                byte[] multiplierBytes = BitConverter.GetBytes(multiplier);
                int bytesWritten;
                bool successWriteMemory = WriteProcessMemory(processHandle, damageAddress, multiplierBytes, (uint)multiplierBytes.Length, out bytesWritten);

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
}
