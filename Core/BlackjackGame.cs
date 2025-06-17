using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

public class BlackjackGame
{
    private readonly GameEngine _engine;

    public BlackjackGame()
    {
        _engine = new GameEngine();
    }

    public void Run()
    {
        // show welcome message

        // Main game loop
        bool playAgain;
        do
        {
            playAgain = _engine.StartGame();
            // TO-DO: prompt to play again / take input from startgame

        } while (playAgain);
    }
}