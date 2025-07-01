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
            playAgain = _engine.PlayRound();

            // TO-DO: prompt to play again

            //
        } while (playAgain);


        if (_engine.User.CurrentMoney >= GameRules.GameWinConditionTarget)
            _ui.VictoryMessage(_engine.User);
        else
            _ui.GameOverMessage(_engine.User);
    }
}