using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Yeonhwa_Enhancer;

class Program
{
    bool isAuthenticated;
    static async Task Main(string[] args)
    {
        Console.Title = "Yeonhwa Enhancer V1.2.374";
        Console.WriteLine("Enter username: ");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        bool isAuthenticated = await Authentication.Login(username, password);

        if (isAuthenticated)
        {
            Console.WriteLine($"Welcome {username}!");
            Console.WriteLine($"Your current damage modification is 10%");
            Console.WriteLine(Identifiers.GetProcessorId());
            Console.WriteLine(Identifiers.GetDiskDriveId());
            Console.WriteLine(Identifiers.GetHWID());
            Console.WriteLine(Identifiers.GetIP());
        }
        else
        {
            Console.WriteLine("Invalid username/password. Please restart the program and try again");
        }
    }
}
