using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace BlackjackGame.Utils;

public class ASCII
{
    public const ConsoleColor DEFAULT_FOREGROUND = ConsoleColor.White;
    public const ConsoleColor DEFAULT_BACKGROUND = ConsoleColor.Black;
    public static string ascii_User = string.Join('\n', @"
 _   _               
| | | |___  ___ _ __ 
| | | / __|/ _ \ '__|
| |_| \__ \  __/ |   
 \___/|___/\___|_|   ".Split('\n')[1..]);

    public static string ascii_Dealer= string.Join('\n', @"
 ____             _           
|  _ \  ___  __ _| | ___ _ __ 
| | | |/ _ \/ _` | |/ _ \ '__|
| |_| |  __/ (_| | |  __/ |   
|____/ \___|\__,_|_|\___|_|   ".Split('\n')[1..]);

    /// <summary>
    /// Displays a segment of ASCII art. Starts at the current cursor location and prints either 
    /// right and down or left and down depending on boolean input parameter value. Finally, returns
    /// the cursor to the left/right of the art at the starting y level, and resets the ConsoleColor
    /// changes.
    /// </summary>
    /// <param name="content">The ASCII art to be displayed, in string format</param>
    /// <param name="foregroundColor">The foreground color output will be in.</param>
    /// <param name="backgroundColor">The background color output will be in.</param>
    public static void DisplayASCII(string content, bool leftToRight = true,
                                    ConsoleColor foregroundColor = DEFAULT_FOREGROUND,
                                    ConsoleColor backgroundColor = DEFAULT_BACKGROUND)
    {
        //
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        //
        int designWidth = content.Split('\n').OrderByDescending(line => line.Length).First().Length; // LINQ
        var (x, y) = Console.GetCursorPosition();
        var (startingX, startingY) = (x, y);

        // iterate through each line of the ASCII art, printing & then shifting the cursor
        foreach (string line in content.Split("\n"))
        {
            Console.SetCursorPosition(x, y);
            Console.Write(line);
            // x -= line.Length;
            Thread.Sleep(25);
            y += 1;
        }
        // set cursor position to be the top point immediately to the right of what was just drawn
        if (leftToRight)
            (x, y) = (startingX + designWidth, startingY);
        else
            (x, y) = (startingX-designWidth, startingY);
        Console.SetCursorPosition(x,y);
        // reset color
        Console.ForegroundColor = DEFAULT_FOREGROUND;
        Console.BackgroundColor = DEFAULT_BACKGROUND;
    }
}