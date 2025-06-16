namespace BlackjackGame.Models;

public class Hand
{
    public List<Card> _cards;
    public bool _isDealer;
    public int _currentScore;

    public Hand(bool isDealer = false)
    {
        this._isDealer = isDealer;

        _cards = [];
        _currentScore = 0;
    }

    public void AddCard(Deck deck)
    {
        this._cards.Add(deck.PullCard());
        _currentScore += Card.GetValue(this._cards.Last()._rank, _currentScore);
    }
}