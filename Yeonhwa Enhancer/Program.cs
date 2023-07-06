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
        Console.Title = GenerateRandomString();
        Console.WriteLine("Enter username: ");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        bool isAuthenticated = await Authentication.Login(username, password);

        if (isAuthenticated)
        {
            await Authentication.UpdateLastLogin();
            Console.Clear();
            Console.WriteLine($"Welcome {username}!");
            Console.WriteLine($"Your current damage multiplier is {Authentication.GetModifier()}");
            Console.WriteLine("");
            Console.WriteLine("Press [Y] to start. Program will immediately exit if successful.");
            string input = Console.ReadLine();
            if (input.ToUpper() == "Y")
            {
                DamageModification.ModifyDamage();
            }
            else
            {
                Console.WriteLine("Closing Program");
                Thread.Sleep(2000);
            }
        }
    }

    static string GenerateRandomString()
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder randomString = new StringBuilder();

        Random random = new Random();
        for (int i = 0; i < 12; i++)
        {
            int index = random.Next(characters.Length);
            randomString.Append(characters[index]);
        }

        return randomString.ToString();
    }
}
