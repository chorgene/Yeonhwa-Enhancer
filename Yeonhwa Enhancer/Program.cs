using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Yeonhwa Enhancer KMS V1.2.374.");
        Thread.Sleep(1000);
        Console.WriteLine("");
        Console.WriteLine("Communicating with Yeonhwa Servers");
        Thread.Sleep(2000);
        Console.WriteLine("");

        Enhancer.Run();

        Console.ReadLine();
    }
}
