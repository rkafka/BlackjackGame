namespace BlackjackGame.Models;

public class Hand
{
    public List<Card> _cards;
    public bool _isDealer;

    public Hand(bool isDealer=false)
    {
        this._isDealer = isDealer;

        _cards = [];
    }

    public void AddCard(Deck deck)
    {
        this._cards.Add(deck.PullCard());
    }
}