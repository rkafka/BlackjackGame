namespace BlackjackGame.Models;


/// <summary> Represents a <see cref="Hand"/> of cards in Blackjack. </summary>
/// <param name="betAmount">The bet amount for this hand.</param>
/// <param name="isDealer">Whether this hand belongs to the dealer.</param>
public class Hand(int betAmount, bool isDealer = false)
{
    private List<Card> _cards = [];
    /// <summary> Gets the list of cards in the hand. </summary>
    public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    private bool _isDealer = isDealer;
    /// <summary> Gets a value indicating whether this hand belongs to the dealer. </summary>
    public bool IsDealer => _isDealer;

    /// <summary>  </summary> 
    public int CurrentScore => CalculateHandValue();  

    private int _betAmount = betAmount;
    /// <summary> Gets or sets the bet amount for this hand. </summary>
    public int BetAmount
    {
        get => _betAmount;
        set => _betAmount = value;
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
        // CurrentScore += Card.GetValue(card.Rank, _currentScore);
    }

    /// <summary> Calculates the total value of the hand, treating Aces as 11 or 1 as needed to avoid busting. 
    /// Updates hand.CurrentScore. </summary>
    /// <returns>The total value of the hand.</returns>
    public int CalculateHandValue()
    {
        int handValue = 0;
        List<Card> aces = [];
        foreach (var card in this.Cards)
        {
            if (card.Rank == 1) // Ace
            {
                handValue += 11;
                aces.Add(card);
            }
            else if (card.Rank >= 10) // Face cards or 10
                handValue += 10;
            else
                handValue += card.Rank;
        }
        // Reduce Ace(s) from 11 to 1 as needed to avoid bust
        while (handValue > 21 && aces.Count > 0)
        {
            handValue -= 10;
            aces[^1].Value = 1;
            aces.RemoveAt(aces.Count - 1);
        }

        return handValue;
    }
}