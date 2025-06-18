using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

public class BJGame
{
    private readonly GameEngine _engine;
    private readonly IGameUI _ui;

    public BJGame(IGameUI ui)
    {
        _ui = ui;
        _engine = new GameEngine(_ui);
    }

    public void Run()
    {
        // Launch Screen
        _ui.LaunchScreen();
        Console.Clear();

        // Main game loop
        bool playAgain;
        do
        {
            playAgain = _engine.StartGame();
            // TO-DO: prompt to play again / take input from startgame
        } while (playAgain);

        _ui.GameOverMessage(_engine);
    }
}