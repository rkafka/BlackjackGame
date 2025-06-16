using BlackjackGame.Utils;

namespace BlackjackGame.Models;

public class UI
{
    public static int TitleHeight = Startup.ascii_Title.Split('\n').Length;
    public const int Hand_RightBound_Text = 23;

    public int _x;
    public int _y;
    bool _doTextBased;
    Game _game;

    public UI(Game game, bool doTextBased = true)
    {
        this._x = Console.CursorLeft;
        this._y = Console.CursorTop;
        this._doTextBased = doTextBased;
        this._game = game;
    }

    // public void PrintHandsAsText()
    // {
    //     int cardPadding = 18;

    //     int currentScore = 0;
    //     Console.WriteLine($"\nUSER'S HAND:");
    //     foreach (Card card in _game._user._hand._cards)
    //     {
    //         Console.WriteLine(card.ToString().PadRight(cardPadding) + $"({card._value})");
    //         currentScore += card._value;
    //     }
    //     _game._user._hand._currentScore = currentScore;
    //     Console.WriteLine($"----------- SCORE: {_game._user._hand._currentScore}");

    //     Console.WriteLine("\nVS.");

    //     currentScore = 0;
    //     Console.WriteLine("\nDEALER'S HAND");
    //     foreach (Card card in _game._dealer._hand._cards)
    //     {
    //         Console.WriteLine(card.ToString().PadRight(cardPadding) + $"({card._value})");
    //         currentScore += card._value;
    //     }
    //     _game._dealer._hand._currentScore = currentScore;
    //     Console.WriteLine($"----------- SCORE: {_game._dealer._hand._currentScore}");
    // }

}