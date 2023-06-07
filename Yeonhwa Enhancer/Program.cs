using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    const int PROCESS_ALL_ACCESS = 0x1F0FFF;  // Desired access rights for the process

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    static extern bool CloseHandle(IntPtr hObject);

    static void Main()
    {
        Console.WriteLine("Welcome to Yeonhwa Enhancer. Developed by Indexed");

        try
        {
            Process process = Process.GetProcessesByName("MapleStory")[0];  // Replace "Program" with the actual process name

            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            // Specify the memory address to modify
            IntPtr address = new IntPtr((long)process.MainModule.BaseAddress + 0x46C6510);  // Adjust the offset according to your needs

            // Convert the new value to bytes
            double newValue = 2.5;
            byte[] newValueBytes = BitConverter.GetBytes(newValue);

            int bytesWritten;
            bool success = WriteProcessMemory(processHandle, address, newValueBytes, (uint)newValueBytes.Length, out bytesWritten);

            if (success)
            {
                Console.WriteLine("Value modified successfully!");
            }
            else
            {
                Console.WriteLine("Failed to modify the value.");
            }

            CloseHandle(processHandle);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong. Please contact the developer.");
            Console.WriteLine($"Error Message: {ex}");
        }
        
    }
}
