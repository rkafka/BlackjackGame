using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

public class BlackjackGame
{
    private readonly GameEngine _engine;
    private readonly IGameUI _ui;

    public BlackjackGame(IGameUI ui)
    {
        _ui = ui;
        _engine = new GameEngine(_ui);
    }

    public void Run()
    {
        // Launch Screen
        Console.Clear();
        _ui.DisplayTitle();
        _ui.PromptToContinue();
        Console.Clear();

        // Title
        _ui.DisplayTitle();

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