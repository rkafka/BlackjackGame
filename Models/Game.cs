namespace BlackjackGame.Models;

public class Game
{
    public Deck _deck;
    public User _user;
    public Dealer _dealer;
    public Game(Deck deck, User user, Dealer dealer)
    {
        _deck = deck;
        _user = user;
        _dealer = dealer;
    }

    public void InitialDraw()
    {
        _user._hand.AddCard(_deck);
        _user._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
    }
    public void DisplayHands()
    {
        
    }
}