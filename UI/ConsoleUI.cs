using BlackjackGame.Models;

namespace BlackjackGame.UI;


public static class UI
{

    public static ConsoleColor COLOR_PROMPT = ConsoleColor.Yellow;

    public static void DisplayTitle()
    {

    }

    public static void DisplayHand(string owner, Hand hand)
    {
        Console.WriteLine($"{owner}'s hand: ");
    }

    public static string PromptPlayerAction()
    {
        Console.Write("PLAYER OPTIONS:  ");
        string[] options = ["Hit", "Stand", "Double Down", "Surrender", "Split"];
        for (int i = 0; i < options.Length; i++)
        {
            Console.Write($"[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{i + 1}");
            Console.ResetColor();
            Console.Write($"] {options[i]}  ");
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n> Type the number to select: ");
        Console.ResetColor();
        return (Console.ReadLine() ?? "").Trim().ToLower(); // ?? ""   means you should return an empty string if the input is null
    }
}