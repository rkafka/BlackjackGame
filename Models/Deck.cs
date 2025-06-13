namespace BlackjackGame.Models;

public class Deck
{
    private List<Card> _cards;

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


    public void Shuffle()
    {
        if (this._cards.Count < 1)
            throw new Exception("Attempted to shuffle a non-existent deck.");

        // PERFORM A FISHER YATES SHUFFLE
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
        // TO-DO: add bounds for cardsPerLine (base on console window width?)
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
    /// Testing summary function
    /// </summary>
    /// <returns>The card from the top of the deck</returns>
    public Card PullCard()
    {
        Card cardPulled = _cards[0];
        _cards.RemoveAt(0);
        return cardPulled;
    }
}