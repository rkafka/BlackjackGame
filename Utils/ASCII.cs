using System.Security.Cryptography.X509Certificates;

namespace BlackjackGame.Utils;

public class ASCII
{
    public static void DisplayASCII(string content)
    {
        var (x,y) = Console.GetCursorPosition();
        foreach (string line in content.Split("\n"))
        {
            Console.SetCursorPosition(x, y);    // THROWING AN ERROR B/C IT EXTENDS BEYOND BOTTOM BOUND
            Console.Write(line);
            x = Console.CursorLeft - line.Length;
            y = Console.CursorTop + 1;
        }
    }
}