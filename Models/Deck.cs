namespace BlackjackGame.Models;

/// <summary>
/// Represents a deck of playing cards for Blackjack.
/// </summary>
public class Deck
{
    private List<Card> _cards;

    /// <summary>
    /// Gets the list of cards in the deck (read-only).
    /// </summary>
    public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    /// <summary>
    /// Initializes a new shuffled deck of cards.
    /// </summary>
    /// <param name="doShuffle">Whether to shuffle the deck upon creation.</param>
    public Deck(bool doShuffle = true)
    {
        _cards = [];
        foreach (string suit in Card.suitDict.Keys)
        {
            for (int rank = Card.MinAllowedRank; rank <= Card.MaxAllowedRank; rank++)
            {
                _cards.Add(new Card(suit, rank));
            }
        }

        if (doShuffle)
            Shuffle();
    }

    /// <summary>
    /// Initializes a deck with a custom set of cards (for testing or debugging).
    /// </summary>
    /// <param name="customCards">The custom set of cards to use as the deck.</param>
    public Deck(List<Card> customCards)
    {
        _cards = customCards;
    }

    /// <summary>
    /// Shuffles the deck using the Fisher-Yates algorithm.
    /// </summary>
    public void Shuffle()
    {
        if (_cards.Count < 1)
            throw new Exception("Attempted to shuffle a non-existent deck.");

        Random rng = new();
        int randomIndex = rng.Next(0, _cards.Count);
        for (int iterativeIndex = 0; iterativeIndex < _cards.Count - 1; iterativeIndex++)
        {
            randomIndex = rng.Next(iterativeIndex + 1, _cards.Count);
            (_cards[iterativeIndex], _cards[randomIndex]) = (_cards[randomIndex], _cards[iterativeIndex]);
        }
    }

    /// <summary>
    /// Outputs the Deck in a formatted table to the Console window.
    /// </summary>
    /// <param name="cardsPerLine">How many cards to output, per row, before adding a newline ('\n')</param>
    /// <param name="title">A title for the deck's output, useful for showing how many shuffles have occured.</param>
    public void Print(int cardsPerLine = 4, string title = "Current Deck")
    {
        int longestCardNameLength = "Queen of Diamonds".Length;
        Console.Write($"\n{title}:  \n| ");
        for (int i = 0; i < _cards.Count; i++)
        {
            Console.Write($"({(i + 1).ToString().PadRight(2)}) {_cards[i].ToString().PadRight((longestCardNameLength), ' ')} | ");
            if (i % cardsPerLine == cardsPerLine - 1)
                Console.Write("\n| ");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Pulls (removes and returns) the top card from the deck.
    /// </summary>
    /// <returns>The card that was removed from the deck.</returns>
    public Card PullCard()
    {
        if (_cards.Count == 0)
            throw new InvalidOperationException("No cards left in the deck.");
        Card card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}