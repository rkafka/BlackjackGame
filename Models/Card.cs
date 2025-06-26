using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;

namespace BlackjackGame.Models;

/// <summary>
/// Represents a playing card with suit, rank, value, and display utilities for Blackjack.
/// </summary>
public class Card
{
    /// <summary>
    /// Maps suit symbols to their string names.
    /// </summary>
    public static readonly Dictionary<string, string> suitDict = new Dictionary<string, string> {
        { "♠", "Spade" }, { "♣", "Club" },
        { "♥", "Heart" }, { "♦", "Diamond" },
    };
    /// <summary>Minimum allowed rank for a card (Ace).</summary>
    public const int MinAllowedRank = 1;
    /// <summary>Maximum allowed rank for a card (King).</summary>
    public const int MaxAllowedRank = 13;
    /// <summary>Number of possible ranks in a standard deck.</summary>
    public const int numberOfRanks = MaxAllowedRank - MinAllowedRank + 1;
    /// <summary>Width of the ASCII art representation of a card.</summary>
    public const int ASCII_WIDTH = 9;
    /// <summary>Height of the ASCII art representation of a card.</summary>
    public const int ASCII_HEIGHT = 7;
    /// <summary>Maps rank integers to their string names.</summary>
    public static readonly string[] rankToStr = [
        "Joker",    // 0
        "Ace",      // 1
        "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
        "Jack",     // 11
        "Queen",    // 12
        "King"      // 13
     ];


    // Instance fields (unique to each instance of a Card object)
    // -- PUBLIC PROPERTIES
    /// <summary>The suit of the card (e.g., "Spades").</summary>
    public string Suit { get; }
    /// <summary>The rank of the card (1=Ace, 11=Jack, 12=Queen, 13=King).</summary>
    public int Rank { get; }
    /// <summary>The value of the card for Blackjack (1, 2-10, or 10 for face cards).</summary>
    public int Value { get; set; }
    // -- PRIVATE VARIABLES
    /// <summary>Whether the card is hidden (face down).</summary>
    private bool _isHidden;


    /// <summary>
    /// Constructs a new Card with the given suit, rank, and optional hidden state.
    /// </summary>
    /// <param name="suit">The suit of the card (e.g., "Spades").</param>
    /// <param name="rank">The rank of the card (1-13).</param>
    /// <param name="hidden">Whether the card is hidden (default: false).</param>
    public Card(string suit, int rank, bool hidden = false)
    {
        this.Suit = suit;
        this.Rank = rank;
        if (rank < 1 || rank > 13)
            throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be in range [1,13]");

        this.Value = GetValue(rank);
        this._isHidden = hidden;
    }
    /// <summary>
    /// Returns a string representation of the card (e.g., "Ace of Spades").
    /// </summary>
    /// <returns>The string representation of the card.</returns>
    public override string ToString()
    {
        if (Rank == 0)
            throw new ArgumentOutOfRangeException("Rank of Zero was assigned to a card, this is not allowed.");
        return $"{rankToStr[Rank]} of {suitDict[Suit]}s";
    }


    /// <summary>
    /// Gets the Blackjack value for a given rank, optionally considering the current hand value for Ace.
    /// </summary>
    /// <param name="rank">The rank of the card.</param>
    /// <param name="currentHandValue">The current value of the hand (optional, for Ace logic).</param>
    /// <returns>The Blackjack value of the card.</returns>
    public static int GetValue(int rank, int currentHandValue = 0)
    {
        // if (_hidden)
        //     return 0;
        return rank switch
        {
            // JOKER
            0 => throw new ArgumentOutOfRangeException(nameof(rank), "JOKER of RANK 0 is not supported"),
            // ACE
            1 => ((currentHandValue > 10) ? 1 : 11),// assumes you want highest value unless 
            2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 10 => rank,
            // JACK
            11 or 12 or 13 => 10,
            _ => throw new ArgumentOutOfRangeException(nameof(rank), "Ranks greater than 13 are not supported."),
        };
    }

    /// <summary>
    /// Returns the ASCII art representation of the card.
    /// </summary>
    /// <returns>A string containing the ASCII art for the card.</returns>
    public string GetASCII()
    {
        // adapted from https://github.com/naivoder/ascii_cards/blob/main/ascii_cards/cards.py
        // modified to C#
        string lineTop    = "┌───────┐";
        string lineMiddle = "│       │";
        string lineBottom = "└───────┘";

        string rankTopLeft      = $" {Rank}";
        string rankBottomRight  = $"{Rank} ";
        string sRank = Rank.ToString();

        if (Rank == 10)  // Ten is the only rank with two digits
        {
            rankTopLeft = sRank;
            rankBottomRight = sRank;
        }
        else
        {
            sRank = Rank switch
            {
                13 => "K",
                12 => "Q",
                11 => "J",
                10 => sRank,
                9 or 8 or 7 or 6 or 5 or 4 or 3 or 2 or 1 => sRank,
                _ => throw new ArgumentOutOfRangeException(nameof(Rank), $"value was {Rank} when it should be in the range 1-13"),
            };
            rankTopLeft = " " + sRank;
            rankBottomRight = sRank + " ";
        }

        // Card lines containing unique card characteristics (suit, rank)
        string lineSuit = $"│   {Suit}   │";
        string lineRankTopLeft = $"│{rankTopLeft}     │";
        string lineRankBottomRight = $"│     {rankBottomRight}│";

        string card_ascii = $"{lineTop}\n{lineRankTopLeft}\n{lineMiddle}\n{lineSuit}"
                            + $"\n{lineMiddle}\n{lineRankBottomRight}\n{lineBottom}";

        if (card_ascii.Split('\n')[0].Length != ASCII_WIDTH)
            throw new Exception("Card ASCII width does not match the constant required.");
        else if (card_ascii.Split('\n').Length != ASCII_HEIGHT)
            throw new Exception("Card ASCII height does not match the constant required.");
        else
            return card_ascii;
    }
}