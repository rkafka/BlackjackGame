using BlackjackGame.Models;

namespace BlackjackGame.Utils;

class Startup
{

    public static void PrintTitle()
    {
        Console.WriteLine("\n" + Utils.ASCII.ascii_Title);
    }

    public static void BootSequence(bool waitForInput=true)
    {
        Console.Clear();
        PrintTitle();
        
        int middleWhiteSpace = Console.WindowHeight - (Utils.ASCII.ascii_Title.Split("\n").Length + 1);
        middleWhiteSpace -= Utils.ASCII.ascii_HandCropped.Split("\n").Length;
        // if (middleWhiteSpace > ascii_PressEnterToStart.Split("\n").Length + 2)
        // {
        //     middleWhiteSpace -= (ascii_PressEnterToStart.Split("\n").Length + 2);
        //     Console.Write("".PadLeft(middleWhiteSpace / 2 + middleWhiteSpace % 2, '\n'));
        //     middleWhiteSpace /= 2;
        //     Console.Write(ascii_PressEnterToStart);
        // }
        Console.Write("".PadRight((middleWhiteSpace > 0 ? middleWhiteSpace : 0), '\n'));

        Console.Write(Utils.ASCII.ascii_HandCropped);
        if (waitForInput) { Console.ReadLine(); }
        Console.Clear();
    }
};