using System.Runtime.CompilerServices;

namespace BlackjackGame.Models;

/// <summary>
/// Represents a deck of playing cards for Blackjack.
/// </summary>
public class Deck
{
    private List<Card> _cards;
    /// <summary> Gets the list of cards in the deck (read-only). </summary>
    public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    private Func<List<Card>>? _getCardsToOmit;


    /// <summary> Initializes a new shuffled deck of cards. </summary>
    /// <param name="doShuffle">Whether to shuffle the deck upon creation.</param>
    /// <param name="getCardsToOmit">The function which retrieves which cards to omit from new card list.</param>
    public Deck(bool doShuffle = true, Func<List<Card>>? getCardsToOmit = null) {
        _getCardsToOmit = getCardsToOmit;
        _cards = CreateNewDeck(doShuffle: doShuffle);
    }

    /// <summary> Initializes a deck with a custom set of cards (for testing or debugging). </summary>
    /// <param name="customCards">The custom set of cards to use as the deck.</param>
    public Deck(List<Card> customCards) { _cards = customCards; }


    private static List<Card> CreateNewDeck(bool doShuffle = true, List<Card>? cardsToOmit = null)
    {
        var newDeck = new List<Card>();
        foreach (string suit in Card.suitDict.Keys)
        {
            for (int rank = Card.MinAllowedRank; rank <= Card.MaxAllowedRank; rank++)
            {
                newDeck.Add(new Card(suit, rank));
            }
        }

        if (cardsToOmit != null)
        {
            foreach (Card card in cardsToOmit)
                newDeck.Remove(card);
        }

        if (doShuffle)
            Shuffle(newDeck);

        return newDeck;
    }


    /// <summary> Shuffles the specified list of cards in place using the Fisher-Yates algorithm.
    /// </summary>
    /// <param name="cards">The list of cards to shuffle.</param>
    /// <exception cref="Exception">Thrown if the list is empty.</exception>
    public static void Shuffle(List<Card> cards)
    {
        if (cards.Count < 1)
            throw new Exception("Attempted to shuffle an empty deck.");

        Random rng = new();
        int randomIndex = rng.Next(0, cards.Count);
        for (int iterativeIndex = 0; iterativeIndex < cards.Count - 1; iterativeIndex++)
        {
            randomIndex = rng.Next(iterativeIndex + 1, cards.Count);
            (cards[iterativeIndex], cards[randomIndex]) = (cards[randomIndex], cards[iterativeIndex]);
        }
    }
    /// <summary> Shuffles this deck's cards in place using the Fisher-Yates algorithm.  </summary>
    public void Shuffle()
    {
        Shuffle(_cards);
    }

    /// <summary> Outputs the Deck in a formatted table to the Console window. </summary>
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

    /// <summary> Pulls (removes and returns) the top card from the deck. </summary>
    /// <returns>The card that was removed from the deck.</returns>
    public Card PullCard()
    {
        if (_cards.Count == 0)
        {
            var cardsToOmit = _getCardsToOmit?.Invoke();
            _cards = CreateNewDeck(doShuffle: true, cardsToOmit:cardsToOmit);
            Console.WriteLine("\nNOTICE: Deck has been reset, all cards not actively in play have been readded to the deck.");
        }
        Card card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}