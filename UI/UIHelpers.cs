using BlackjackGame.Models;

namespace BlackjackGame.UI;

public static class UIHelper
{
    private static int titleNumber = 0;
    private static Dictionary<string, int> _titleCounts = [];

    /// <summary> Method for breaking up sections and adding a title </summary>
    /// <param name="header">The text which will be displayed formatted as a section header</param>
    /// <param name="addNumber">Optional toggle for whether to enumerate the section title</param>
    /// <param name="markDuplicates">Dictates whether the header will be denoted as having 
    ///                              been repeated (and how many times).</param>
    public static void PrintSectionHeader(string header, bool addNumber = false, bool markDuplicates = false)
    {
        if (addNumber)
        {
            titleNumber++;
            header = $" {titleNumber}. {header} ";
            // if title is repeated, indicate so
            if (!_titleCounts.ContainsKey(header))
            {
                _titleCounts[header] = 1;
            }
            else
            {
                _titleCounts[header]++;
                if (markDuplicates)
                    header += $"({_titleCounts[header]}) ";

            }
        }
        else
            header = $" {header} ";

        int numLines = Console.WindowWidth - header.Length;
        // if numLines odd, truncation occurs below
        Console.Write("[".PadLeft(numLines / 2, '-') + header);
        Console.Write("]".PadRight(numLines / 2, '-'));
        // Adjust title for numLines' truncation, if needed
        Console.WriteLine(((numLines % 2 == 1) ? "-" : "") + "\n");
    }

    /// <summary> Capitalizes the first letter of the input string </summary>
    /// <param name="input">String to capitalize the first letter of.</param>
    /// <returns>Input string with its first letter capitalized</returns>
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + ((input.Length > 1) ? input.Substring(1) : "");
    }

    /// <summary> Prints the input string with set color. </summary>
    /// <param name="message"></param>
    /// <param name="doNewLine"></param>
    /// <param name="resetColorsAfter"></param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    public static void PrintColored(string message, bool doNewLine = false, bool resetColorsAfter = true,
                                    ConsoleColor foregroundColor = IGameUI.COLOR_DEFAULT_FOREGROUND,
                                    ConsoleColor backgroundColor = IGameUI.COLOR_DEFAULT_BACKGROUND)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        Console.Write(message);
        if (doNewLine) { Console.WriteLine(); }

        if (resetColorsAfter)
            ResetConsoleColors();
    }

    /// <summary>  </summary>
    /// 
    ///  
    public static void PrintColoredLine(string message, bool resetColorsAfter = true,
                                        ConsoleColor foregroundColor = IGameUI.COLOR_DEFAULT_FOREGROUND,
                                        ConsoleColor backgroundColor = IGameUI.COLOR_DEFAULT_BACKGROUND)
    {
        PrintColored(message, doNewLine: true, resetColorsAfter: resetColorsAfter, foregroundColor: foregroundColor, backgroundColor: backgroundColor);
    }

    /// <summary> Prints a message character-by-character. </summary>
    /// <param name="msg">The message to output to the console.</param>
    /// <param name="doNewLine">Whether to print a newline character at the end of message output.</param>
    /// <param name="resetColorAfter">Whether to reset foreground and background colors to default at the end of execution.</param>
    /// <param name="msPerChar">The time in ms the thread is put to sleep between each character printed</param>
    /// <param name="foregroundColor">Foreground color the console will print the message in.</param>
    /// <param name="backgroundColor">Background color the console will print the message in.</param>
    public static void PrintSlowly(string msg, bool doNewLine = false, bool resetColorAfter = true, int msPerChar = 30,
                                   ConsoleColor? foregroundColor = IGameUI.COLOR_DEFAULT_FOREGROUND,
                                   ConsoleColor? backgroundColor = IGameUI.COLOR_DEFAULT_BACKGROUND)
    {
        if (foregroundColor != null)
            Console.ForegroundColor = (ConsoleColor)foregroundColor;
        if (backgroundColor != null)
            Console.BackgroundColor = (ConsoleColor)backgroundColor;


        foreach (char c in msg)
        {
            Console.Write(c);
            Thread.Sleep(msPerChar);
        }
        if (doNewLine)
            Console.WriteLine();

        if (resetColorAfter)
            ResetConsoleColors();
    }

    public static void PrintUserWinLossRecord(User user)
    {
        // Print W-L-T with colors: W (green), L (red), T (yellow), dashes (white)
        string record = user.GetWinLossRecord(); // e.g., "3-2-1"
        string[] parts = record.Split('-');
        if (parts.Length != 3)
        {
            Console.WriteLine(record); // fallback
            return;
        }
        // Wins
        PrintColored(parts[0], foregroundColor:IGameUI.COLOR_GOOD, doNewLine: false);
        // Dash
        Console.Write("-");
        // Losses
        PrintColored(parts[1], foregroundColor:IGameUI.COLOR_BAD, doNewLine: false);
        // Dash
        Console.Write("-");
        // Ties
        PrintColored(parts[2], foregroundColor:IGameUI.COLOR_PROMPT);
    }


    /// <summary> Resets the console's background and foreground colors to their default values. </summary>
    public static void ResetConsoleColors()
    {
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;
        Console.ForegroundColor = Utils.ASCII.DEFAULT_FOREGROUND;
    }

}