

using System.Runtime.CompilerServices;
using BlackjackGame.Utils;

namespace BlackjackGame.UI;

public static class Menu
{
    private const char cornerTL = '┌'; private const char cornerTR = '┐';
    private const char cornerBL = '└'; private const char cornerBR = '┘';
    private const char vertLine = '│'; private const char horizLine = '─';


    public static void Execute(string[] args)
    {
        // @"This is blackjack.dll, a terminal-based blackjack game. It follows typical blackjack rules, though specifics can be "
        // WriteWithinBounds();

        int validMenuOption = StartMenu();
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
    public static void DrawBox(Box box, string msg = "", int msgOffset=0)
    {
        DrawBox(width: box.Width, height: box.Height, x: box.X, y: box.Y, fgColor: box.FGColor, bgColor: box.BGColor);

        if (msg.Length > 0)
        {
            if (msg.Length > box.Width - 2 * msgOffset)
                WriteWithinBounds(msg, x: box.X + 1, y: box.Y + 1, width: box.Width - 2, height: box.Height - 2, delayInMS: 0);
            else
                Console.Write(msg);
        }
    }

    public static int StartMenu()
    {
        PrintBoundingBox();

        int currentX = 2; int currentY = 4;
        int boxWidth = Console.WindowWidth - 4;
        int defaultBoxHeight = 3;
        int bigBoxHeight = 22;

        Box bigBox = new(x: currentX, y: currentY, widthMod: -4, height: 22, fgColor: ConsoleColor.DarkGray);
        DrawBox(bigBox);//(width: boxWidth, height: bigBoxHeight, x: currentX, y: currentY, fgColor: ConsoleColor.DarkGray);

        // OPTIONS BOX
        string entryHeader = "Welcome! This is the start menu, view your options below.";
        DrawBox(width: entryHeader.Length + 4, height: defaultBoxHeight, x: currentX + 2, y: currentY - 1, fgColor: ConsoleColor.DarkGray);
        UIHelper.PrintColored(entryHeader, foregroundColor: ConsoleColor.DarkGray);

        // int numBoxes = 3;
        // string[] options = { "" };
        // for (int i = 0; i < numBoxes; i++)
        // {
        //     DrawBox(width: boxWidth, height: defaultBoxHeight, x: currentX, y: currentY, fgColor: ConsoleColor.DarkGray);
        //     currentY += defaultBoxHeight;
        // }

        currentX = 4;
        currentY = 7;
        Console.SetCursorPosition(currentX, 7);
        ASCII.DisplayASCII(ASCII.ascii_Title);

        currentY += ASCII.ascii_Title.Split('\n').Length;


        string[] options = {
            "Start game", "View the rules of the game", "Change your preferred options (difficulty and such)"
        };

        // Draw the options box
        int optionsBoxX = currentX + 1;
        int optionsBoxY = currentY;
        int optionsBoxWidth = boxWidth - 6;
        int optionsBoxHeight = options.Length + 4;
        Box optionsBox = new Box(currentX + 1, currentY, boxWidth - 6, options.Length + 4);
        DrawBox(optionsBoxWidth, optionsBoxHeight, x: optionsBoxX, y: optionsBoxY, fgColor: ConsoleColor.DarkGray);

        // Print the [OPTIONS] label centered at the top of the box
        int labelX = optionsBoxX + 1; //+ (optionsBoxWidth - 10) / 2; // 10 = length of " [OPTIONS] "
        int labelY = optionsBoxY;
        Console.SetCursorPosition(labelX, labelY);
        UIHelper.PrintColored(" [OPTIONS] ", foregroundColor: ConsoleColor.DarkGray);

        // Print each option inside the box, starting below the label
        int optionStartY = optionsBoxY + 2;
        int optionX = optionsBoxX + 2;
        for (int i = 0; i < options.Length; i++)
        {
            Console.SetCursorPosition(optionX, optionStartY + i);
            Console.Write("[");
            UIHelper.PrintColored((i + 1).ToString(), foregroundColor: IGameUI.COLOR_SPECIAL_1);
            Console.Write("] " + options[i]);
        }

        currentY += bigBoxHeight;//16;
        DrawBox(width: boxWidth, height: defaultBoxHeight, x: 2, y: Console.WindowHeight - 4, fgColor: ConsoleColor.DarkGray);
        UIHelper.PrintColored("Enter your selection:  ", foregroundColor: IGameUI.COLOR_PROMPT);
        currentX = Console.CursorLeft; currentY = Console.CursorTop;

        int menuSelection = -1;
        int[] supportedMenuOptions = [1];
        while (!int.TryParse(Console.ReadLine(), out menuSelection) || !supportedMenuOptions.Contains(menuSelection))
        {
            // Error Msg
            DrawBox(width: boxWidth, height: defaultBoxHeight, x: 2, y: Console.WindowHeight - 4, fgColor: ConsoleColor.DarkGray);
            UIHelper.PrintColored("(Sorry, that didn't work. Please type an integer) ", foregroundColor: IGameUI.COLOR_BAD);
            Thread.Sleep(2000);

            // Reset
            DrawBox(width: boxWidth, height: defaultBoxHeight, x: 2, y: Console.WindowHeight - 4, fgColor: ConsoleColor.DarkGray);
            UIHelper.PrintColored("Enter your selection:  ", foregroundColor: IGameUI.COLOR_PROMPT);
        }
        return menuSelection;
    }

    public static void WriteWithinBounds(string msg, int x, int y, int width, int height, int delayInMS = 0)
    {
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


public class Box
{
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public ConsoleColor FGColor;
    public ConsoleColor BGColor;

    public int[] TopLeft;
    public int[] MidLeft;

    public Box(int x = 0, int y = 0, int width = -1, int widthMod = 0, int height = 3, ConsoleColor fgColor = IGameUI.COLOR_DEFAULT_FOREGROUND, ConsoleColor bgColor = IGameUI.COLOR_DEFAULT_BACKGROUND)
    {
        // coordinates (topleft)
        this.X = x;
        this.Y = y;
        // dimensions
        this.Width = ((width <= 0) ? Console.WindowWidth : width) + widthMod;
        this.Height = height;
        // colors for output
        this.FGColor = fgColor;
        this.BGColor = bgColor;
        // markers to use for writing
        this.TopLeft = [(x + 1), (y + (height / 2))];
        this.MidLeft = [(x + 1), (y + (height / 2))];
    }
    public Box(Box otherBox, int shiftX = 0, int shiftY = 0, int decreaseWidth = 2, int decreaseHeight = 0, ConsoleColor fgColor = IGameUI.COLOR_DEFAULT_FOREGROUND, ConsoleColor bgColor = IGameUI.COLOR_DEFAULT_BACKGROUND)
        : this(x: (otherBox.X + shiftX), y: (otherBox.Y + shiftY), width: otherBox.Width, widthMod:decreaseWidth, height: (otherBox.Height - decreaseHeight), fgColor, bgColor) { }

    public (int, int) GetCoords() { return (this.X, this.Y); }

    public void SetCoords(int newX, int newY) { this.X = newX; this.Y = newY; }

    public void Shift(int horiz = 0, int vert = 0)
    {
        this.X += horiz;
        this.Y += vert;
    }
}
