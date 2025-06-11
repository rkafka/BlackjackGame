namespace BlackjackGame.Utils;

class Startup {
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


    public static void PrintTitle(bool waitForInput = true)
    {
        Console.Clear();
        Console.WriteLine("\n" + ascii_Title);
        Console.WriteLine("\n\n\n");
        Console.Write(ascii_Hand);
        if (waitForInput) { Console.ReadLine(); }
    }
};