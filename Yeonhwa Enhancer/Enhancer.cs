using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

public class Enhancer
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

    public static void Run()
    {
        try
        {
            Process process = Process.GetProcessesByName("MapleStory")[0];
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            Console.WriteLine("Connected");
            Console.WriteLine("");

            Console.Clear();

            IntPtr characterNameAddress = new IntPtr((long)process.MainModule.BaseAddress + 0x5240EA0);

            IntPtr address = new IntPtr((long)process.MainModule.BaseAddress + 0x5240EA0);

            const int MAX_STRING_LENGTH = 256;

            byte[] buffer = new byte[MAX_STRING_LENGTH];

            int bytesRead;
            bool successReadName = ReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead);
            string characterName = "";

            IntPtr characterDamageAddress = new IntPtr((long)process.MainModule.BaseAddress + 0x46C64C8);

            Console.WriteLine("Character is authorized");
            // Convert the new value to bytes
            double newValue = 2.5;
            byte[] newValueBytes = BitConverter.GetBytes(newValue);

            int bytesWritten;
            WriteProcessMemory(processHandle, characterDamageAddress, newValueBytes, (uint)newValueBytes.Length, out bytesWritten);

            CloseHandle(processHandle);
           
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