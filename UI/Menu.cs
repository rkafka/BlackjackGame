

using System.Runtime.CompilerServices;

namespace BlackjackGame.UI;

public static class Menu
{
    private const char cornerTL = '┌'; private const char cornerTR = '┐';
    private const char cornerBL = '└'; private const char cornerBR = '┘';
    private const char vertLine = '│'; private const char horizLine = '─';


    public static void Execute(string[] args)
    {
        PrintBoundingBox();
        // @"This is blackjack.dll, a terminal-based blackjack game. It follows typical blackjack rules, though specifics can be "
        // WriteWithinBounds();

        int currentX = 2;
        int currentY = 4;
        int boxWidth = Console.WindowWidth - 4;
        int defaultBoxHeight = 3;

        DrawBox(width: boxWidth, height: 16, x: currentX, y: currentY, fgColor: ConsoleColor.DarkGray);
        currentY += 16;

        int numBoxes = 3;
        for (int i = 0; i < numBoxes; i++)
        {
            DrawBox(width: boxWidth, height: defaultBoxHeight, x: currentX, y: currentY, fgColor: ConsoleColor.DarkGray);
            currentY += defaultBoxHeight;
        }

        Console.ReadLine();
    }

    public static void PrintBoundingBox()
    {
        // Bounding box
        DrawBox(Console.WindowWidth, Console.WindowHeight, x: 0, y: 0, fgColor: ConsoleColor.Gray);
        // Header box
        // DrawBox(Console.WindowWidth - 4, height:3, x: 2, y: 1, fgColor: ConsoleColor.White, bgColor:ConsoleColor.Magenta);
        DrawBox(Console.WindowWidth, height: 3, x: 0, y: 0, fgColor: ConsoleColor.White, bgColor: ConsoleColor.White);
        // Console.SetCursorPosition(left: 3, top: 2);
        // Console.SetCursorPosition(1,1);
        UIHelper.PrintColored(" blackjack.dll   \\menu", foregroundColor: ConsoleColor.Black, backgroundColor: ConsoleColor.White);
    }

    /// <summary>
    /// Draws a box of particular weidth/height starting at a given coordinate (x,y) using specified colors.
    /// The box will be drawn from x to x+width, and from y to y+height.
    /// </summary>
    /// <param name="width">The width of the box to draw (in number of characters).</param>
    /// <param name="height">The height of the box to draw (in number of characters).</param>
    /// <param name="x">The x-coordinate to start drawing from.</param>
    /// <param name="y">The y-coordinate to start drawing from.</param>
    /// <param name="overwrites">Whether the box will clear any output within the bounds its drawn in.</param>
    /// <param name="centerVertAfter">Whether the cursor will be centered vertically in the box's bounds after. Helpful for putting text in the space.</param>
    /// <param name="fgColor">The foreground color the box will be displayed in.</param>
    /// <param name="bgColor">The bkackground color the box will be displayed in.</param>
    public static void DrawBox(int width, int height, int x = 0, int y = 0, bool overwrites = true, bool centerVertAfter = true,
                                ConsoleColor fgColor = IGameUI.COLOR_DEFAULT_FOREGROUND, ConsoleColor bgColor = IGameUI.COLOR_DEFAULT_BACKGROUND)
    {
        // ERROR CATCHING
        if (width < 2 || height < 2)
            throw new ArgumentOutOfRangeException(nameof(width), nameof(height),
            message: "attempted to DrawBox that was too small (must be at least 2x2)");
        else if (width > Console.WindowWidth || height > Console.WindowWidth)
            throw new ArgumentOutOfRangeException(nameof(width), nameof(height),
            message: "attempted to DrawBox that was too large (must fit within the dimensions of the window)");

        // Set color & cursor coordinates
        Console.ForegroundColor = fgColor; Console.BackgroundColor = bgColor;
        Console.SetCursorPosition(x, y);

        Console.Write(cornerTL + new string(horizLine, width - 2) + cornerTR); // Top line

        int curY = y + 1;
        Console.SetCursorPosition(x, curY);
        while (curY < y + height - 1)
        {
            Console.Write(vertLine);
            if (overwrites)
                Console.Write(new string(' ', width - 2)); // overwrites the space under the box being drawn
            else
                Console.CursorLeft = x + width - 1;
            Console.Write(vertLine);

            curY++;
            Console.SetCursorPosition(x, curY);
        }
        Console.Write(cornerBL + new string(horizLine, width - 2) + cornerBR); // bottom line

        UIHelper.ResetConsoleColors();

        if (centerVertAfter)
            Console.SetCursorPosition(left: x + 2, top: (y + (height / 2))); //+ ((y + height) % 2) );

    }

    public static void WriteWithinBounds(string msg, int x, int y, int width, int height, int delayInMS = 0) {
        if (msg.Length > width * height)
            throw new ArgumentOutOfRangeException(nameof(msg), "The message given was longer than allowed within the given bounds");

        // split message into words
        string[] words = msg.Split(' ');
        int curX = x; int curY = y;
        int i = 0;
        while (i < words.Length && curY <= y + height)
        {
            Console.SetCursorPosition(curX, curY);

            while (i < words.Length && curX + words[i].Length <= x + width)
            {
                Console.Write(words[i]);
                i++;
                curX = Console.CursorLeft;
            }
            curY++;
        }

        if (i < words.Length)
            throw new ArgumentException(nameof(msg), "The message could not be output properly within these bounds");
    }
}