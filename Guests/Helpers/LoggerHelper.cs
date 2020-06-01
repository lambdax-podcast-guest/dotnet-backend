
using System;
using System.Threading.Tasks;

// abstract class with a static print method that will just make nicer output when calling Console.WriteLine
public abstract class LoggerHelper
{
    /// <summary>
    /// This is an attempt to create easy formatted printing.
    /// If you just use Console.WriteLine the output gets lost in all the other console output.
    /// Making it asynchronous is my attempt to keep the output together with the color, but sometimes it is still getting mixed up
    /// Don't forget to await this method when you call it
    /// </summary>
    public async static Task Print<T>(T arg)
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        await Console.Out.WriteLineAsync("*****YOUR PRINT OUTPUT*****\n" + arg.ToString() + "\n" + "***************************");

        Console.ResetColor();
    }
}