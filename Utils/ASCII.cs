using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace BlackjackGame.Utils;

public class ASCII
{
    /// <summary> Default foreground color for Console Output </summary>
    public const ConsoleColor DEFAULT_FOREGROUND = ConsoleColor.White;

    /// <summary> Default background color for Console Output </summary>
    public const ConsoleColor DEFAULT_BACKGROUND = ConsoleColor.Black;

    
    /// <summary> String holding the ASCII art of the blackjack.dll title </summary>
    public const string ascii_Title = @"                                                                           
  88          88                       88        88                       88                     88 88 88  
  88          88                       88        """"                       88                     88 88 88  
  88          88                       88                                 88                     88 88 88  
  88,dPPYba,  88 ,adPPYYba,  ,adPPYba, 88   ,d8  88 ,adPPYYba,  ,adPPYba, 88   ,d8       ,adPPYb,88 88 88  
  88P'    ""8a 88 """"     `Y8 a8""     """" 88 ,a8""   88 """"     `Y8 a8""     """" 88 ,a8""       a8""    `Y88 88 88  
  88       d8 88 ,adPPPPP88 8b         8888[     88 ,adPPPPP88 8b         8888[         8b       88 88 88  
  88b,   ,a8"" 88 88,    ,88 ""8a,   ,aa 88`""Yba,  88 88,    ,88 ""8a,   ,aa 88`""Yba,  888 ""8a,   ,d88 88 88  
  8Y""Ybbd8""'  88 `""8bbdP""Y8  `""Ybbd8""' 88   `Y8a 88 `""8bbdP""Y8  `""Ybbd8""' 88   `Y8a 888  `""8bbdP""Y8 88 88  
                                                ,88                                                        
                                              888P""                                                        
";

    /// <summary> String holding the ASCII art of a hand holding a playing card </summary>
    public const string ascii_Hand = @"
                                _____________________
                               |                     |
                               |                     |
                    _.---------|.--.                 |
                 .-'  `       .'/  ``                |
              .-'           .' |    /|               |
           .-'         |   /   `.__//                |
        .-'           _.--/        /                 |
       |        _  .-'   /        /                  |
       |     ._  \      /     `  /                   |
       |        ` .    /     `  /                    |
       |         \ \ '/        /                     |
       |        - \  /        /|                     |
       |        '  .'        / |                     |
       |          '         |.'|                     |
       |                    |  |                     |
       |                    |  |_____________________|
       |                    |.'
       |                    /
       |                   /
       |                  /
       )                 /|
    .A/`-.              / |
   AMMMA. `-._         / /
  AMMMMMMMMA. `-.     / /
 AMMMMMMMMMMMMA. `.    /
AMMMMMMMMMMMMMMMMA.`. /
MMMMMMMMMMMMMMMMMMMA.`.
MMMMMMMMMMMMMMMMMMMMMA.`.
MMMMMMMMMMMMMMMMMMMMMMMA.
MMMMMMMMMMMMMMMMMMMMMMMMMA.
MMMMMMMMMMMMMMMMMMMMMMMMMMM
MMMMMMMMMMMMMMMMMMMMMMMMMV'
";

    /// <summary> String holding the ASCII art of a hand holding a playing card, cropped to dimensions befitting
    /// the launch screen. </summary>
    public const string ascii_HandCropped = @"
                                _____________________
                               |           ___  __   |
                               |          |__ \/_ |  |
                    _.---------|.--.         ) || |  |
                 .-'  `       .'/  ``       / / | |  |
              .-'           .' |    /|     / /_ | |  |
           .-'         |   /   `.__//     |____||_|  |
        .-'           _.--/        /                 |
       |        _  .-'   /        /      /\          |
       |     ._  \      /     `  /      /  \         |
       |        ` .    /     `  /      /    \        |
       |         \ \ '/        /       \    /        |
       |        - \  /        /|        \__/         |
       |        '  .'        / |         __          |
       |          '         |.'|        /  \         |";

    /// <summary> String holding the ASCII art of a queen. </summary>
    public const string ascii_TestQueen = @"
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡄⢤⣀⣯⣠⡄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⣿⡿⠿⢿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣷⣶⣶⣶⣾⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀
⢀⣤⣤⠤⠤⠤⠤⠤⠤⠌⣿⣿⣿⣿⣿⣿⣿⠥⠤⠤⠤⠤⠤⠤⣤⣄⠀
⣿⣏⣹⠆⠀⠀⠀⠀⠀⠀⠈⠙⣻⣿⡟⠉⠁⠀⠀⠀⠀⠀⠀⠰⣯⣿⡇
⠈⠙⠓⠲⣄⠀⠀⠀⢀⣶⣶⣾⣿⣿⣷⣶⣶⡄⠀⠀⠀⠀⣴⠖⠛⠋⠀
⠀⠀⠀⠀⣯⠀⠀⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⡇⠀⠀⠀⠀
⠀⠀⠀⠀⡇⠀⠀⢠⣿⡟⠙⣿⣿⣿⣿⡿⠙⣿⣇⠀⠀⠀⡇⠀⠀⠀⠀
⠀⠀⠀⠀⣧⣠⣴⣿⠏⠀⠀⢸⣿⣿⣿⡀⠀⠈⢿⣶⣤⡀⣇⠀⠀⠀⠀
⠀⢸⢉⣽⡿⢿⠋⠁⠀⢀⣶⣿⣿⣿⣿⣿⣦⠀⠀⠉⢛⠿⢿⣍⠩⡇⠀
⠀⢸⣿⣇⠀⢸⠀⠀⠀⣾⣩⣿⣿⣿⣿⣿⣿⡇⠀⠀⢸⠀⢀⣿⣷⡇⠀
⠀⣻⠏⠙⠆⠸⡆⠀⠀⢻⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⢸⠀⠘⠈⢿⡁⠀
⠀⢹⠀⠀⠀⠀⡧⠤⢤⣿⣿⣿⣿⣿⣿⣿⡿⠠⠤⠤⢼⠀⠀⠀⠐⡇⠀
⠀⢸⠀⠀⠀⠀⡇⠀⣸⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⡾⠀⠀⠀⠀⡇⠀
⠀⢸⠀⠀⠀⠀⡇⢠⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀⡇⠀⠀⠀⠀⡇⠀
⠀⢸⠀⠀⠀⠀⢧⣾⣿⡿⢻⣿⣿⣿⣿⣿⠀⠀⠀⠀⡇⠀⠀⠀⠀⡇⠀
⠀⢸⠀⠀⠀⣠⣾⣿⡟⣠⣾⣿⣿⣿⣿⣿⣧⡀⠀⠀⡇⠀⠀⠀⠀⡇⠀
⠀⠈⠀⠀⠀⣿⣿⣿⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣧⣀⠀⠀⠀⡇⠀
";
    public const string ascii_PressEnterToStart = @"
 ____                      _____       _            
|  _ \ _ __ ___  ___ ___  | ____|_ __ | |_ ___ _ __ 
| |_) | '__/ _ \/ __/ __| |  _| | '_ \| __/ _ \ '__|
|  __/| | |  __/\__ \__ \ | |___| | | | ||  __/ |   
|_|   |_|__\___||___/___/_|_____|_| |_|\__\___|_|   
      |_   _|__   / ___|| |_ __ _ _ __| |_          
        | |/ _ \  \___ \| __/ _` | '__| __|         
        | | (_) |  ___) | || (_| | |  | |_          
        |_|\___/  |____/ \__\__,_|_|   \__|         ";
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

    /// <summary> Displays a segment of ASCII art. Starts at the current cursor location and prints either 
    /// right and down or left and down depending on boolean input parameter value. Finally, returns
    /// the cursor to the left/right of the art at the starting y level, and resets the ConsoleColor
    /// changes. </summary>
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