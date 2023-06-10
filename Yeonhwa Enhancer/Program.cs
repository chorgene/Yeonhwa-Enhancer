using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

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
        Console.WriteLine("Welcome to Yeonhwa Enhancer KMS V1.2.374 Developed by Indexed");
        Thread.Sleep(1000);
        Console.WriteLine("");
        Console.WriteLine("Communicating with Yeonhwa Servers");
        Thread.Sleep(2000);
        Console.WriteLine("");

        try
        {
            Process process = Process.GetProcessesByName("MapleStory")[0];

            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            Console.WriteLine("Connected");
            Console.WriteLine("");

            Console.Clear();


            /*List<IntPtr> addresses = new List<IntPtr>()
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

            };*/

            IntPtr characterNameAddress = new IntPtr((long)process.MainModule.BaseAddress + 0x5240EA0);

            IntPtr address = new IntPtr((long)process.MainModule.BaseAddress + 0x5240EA0);

            const int MAX_STRING_LENGTH = 256;

            byte[] buffer = new byte[MAX_STRING_LENGTH];

            int bytesRead;
            bool successReadName = ReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead);
            string characterName = "";
            
            if (successReadName)
            {
                characterName = System.Text.Encoding.Default.GetString(buffer);
                characterName = characterName.Substring(0, characterName.IndexOf('\0'));  // Trim any null characters at the end
                if (string.IsNullOrEmpty(characterName))
                {
                    Console.WriteLine("No Character Detected. Please log into the first 3 main characters");
                }
                else
                {
                    Console.WriteLine($"Character: {characterName} is detected");
                }
            }
            else
            {
                Console.WriteLine("Failed.");
            }


            IntPtr characterDamageAddress = new IntPtr((long)process.MainModule.BaseAddress + 0x46C64C8);

            if (characterName == "Smooth" || characterName == "Sorry")
            {
                Console.WriteLine("Character is authorized");
                // Convert the new value to bytes
                double newValue = 2.5;
                byte[] newValueBytes = BitConverter.GetBytes(newValue);

                int bytesWritten;
                bool successWriteMemory = WriteProcessMemory(processHandle, characterDamageAddress, newValueBytes, (uint)newValueBytes.Length, out bytesWritten);

                if (successWriteMemory)
                {
                    Console.WriteLine($"{characterName} is enhanced!");
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                CloseHandle(processHandle);
            }
            else
            {
                CloseHandle(processHandle);
                if (string.IsNullOrEmpty (characterName))
                {

                }
                else
                {
                    Console.WriteLine("Character is not authorized");
                }
                
            }
            
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong. Please contact the developer.");
            Console.WriteLine($"Error Message: {ex}");
            Console.ReadLine();
        }
        
    }
}
