namespace BlackjackGame.Models;

/// <summary>
/// Represents a hand of cards in Blackjack.
/// </summary>
public class Hand
{
    private List<Card> _cards;
    private bool _isDealer;
    private int _currentScore;
    private int _betAmount;

    /// <summary>
    /// Gets the list of cards in the hand.
    /// </summary>
    public List<Card> Cards
    {
        get => _cards;
        set => _cards = value;
    }

    /// <summary>
    /// Gets a value indicating whether this hand belongs to the dealer.
    /// </summary>
    public bool IsDealer => _isDealer;

    /// <summary>
    /// Gets or sets the current score of the hand.
    /// </summary>
    public int CurrentScore
    {
        get => _currentScore;
        set => _currentScore = value;
    }

    /// <summary>
    /// Gets or sets the bet amount for this hand.
    /// </summary>
    public int BetAmount
    {
        get => _betAmount;
        set => _betAmount = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hand"/> class.
    /// </summary>
    /// <param name="betAmount">The bet amount for this hand.</param>
    /// <param name="isDealer">Whether this hand belongs to the dealer.</param>
    public Hand(int betAmount, bool isDealer = false)
    {
        _cards = [];
        _isDealer = isDealer;
        _currentScore = 0;
        _betAmount = betAmount;
    }

    /// <summary>
    /// Returns a string representation of the hand.
    /// </summary>
    public override string ToString() => string.Join(", ", _cards);

    /// <summary>
    /// Adds a card from the deck to this hand and updates the score.
    /// </summary>
    /// <param name="deck">The deck to pull a card from.</param>
    public void AddCard(Deck deck)
    {
        var card = deck.PullCard();
        _cards.Add(card);
        _currentScore += Card.GetValue(card.Rank, _currentScore);
    }
}