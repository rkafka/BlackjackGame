using System.Security.Cryptography.X509Certificates;

namespace BlackjackGame.Utils;

public class ASCII
{

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

    public static void DisplayASCII(string content)
    {
        int designWidth = content.Split('\n').OrderByDescending(line => line.Length).First().Length; // LINQ
        var (x, y) = Console.GetCursorPosition();
        var (startingX, startingY) = (x, y);
        // iterate through each line of the ASCII art, printing & then shifting the cursor
        foreach (string line in content.Split("\n"))
        {
            Console.SetCursorPosition(x, y);
            Console.Write(line);
            // x -= line.Length;
            Thread.Sleep(10);
            y += 1;
        }
        // set cursor position to be the top point immediately to the right of what was just drawn
        Console.SetCursorPosition(startingX + designWidth, startingY);

    }
}