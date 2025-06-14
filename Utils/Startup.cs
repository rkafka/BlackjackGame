using BlackjackGame.Models;

namespace BlackjackGame.Utils;

class Startup
{
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

    public static void PrintTitle()
    {
        Console.WriteLine("\n" + ascii_Title);
    }

    public static void BootSequence(bool waitForInput=true)
    {
        Console.Clear();
        PrintTitle();
        
        int middleWhiteSpace = Console.WindowHeight - (ascii_Title.Split("\n").Length + 1);
        middleWhiteSpace -= ascii_HandCropped.Split("\n").Length;
        // if (middleWhiteSpace > ascii_PressEnterToStart.Split("\n").Length + 2)
        // {
        //     middleWhiteSpace -= (ascii_PressEnterToStart.Split("\n").Length + 2);
        //     Console.Write("".PadLeft(middleWhiteSpace / 2 + middleWhiteSpace % 2, '\n'));
        //     middleWhiteSpace /= 2;
        //     Console.Write(ascii_PressEnterToStart);
        // }
        Console.Write("".PadRight((middleWhiteSpace > 0 ? middleWhiteSpace : 0), '\n'));

        Console.Write(ascii_HandCropped);
        if (waitForInput) { Console.ReadLine(); }
        Console.Clear();
    }
};