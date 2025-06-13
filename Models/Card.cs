using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;

namespace BlackjackGame.Models;

public class Card
{
    // Static fields (shared across all Card objects)
    public static readonly Dictionary<string, string> suitDict = new Dictionary<string, string> {
        { "♠", "Spade" }, { "♣", "Club" },
        { "♥", "Heart" }, { "♦", "Diamond" },
    };
    public const int MinAllowedRank = 1;
    public const int MaxAllowedRank = 13;
    public const int numberOfRanks = MaxAllowedRank - MinAllowedRank + 1;
    public static readonly string[] rankToStr = [
        "Joker",    // 0
        "Ace",      // 1
        "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
        "Jack",     // 11
        "Queen",    // 12
        "King"      // 13
     ];


    // Instance fields (unique to each instance of a Card object)
    public string _suit { get; }
    public int _rank { get; }
    public int _value;

    public Card(string suit, int rank)
    {
        _suit = suit;
        _rank = rank;
        if (rank < 1 || rank > 13)
            throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be in range [1,13]");
        _value = getValue(rank);
    }
    public override string ToString()
    {
        if (_rank == 0)
            throw new ArgumentOutOfRangeException("Rank of Zero was assigned to a card, this is not allowed.");
        return $"{rankToStr[_rank]} of {suitDict[_suit]}s";
    }


    /**
     */
    public static int getValue(int rank, int currentHandValue = 0)
    {
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
        // OLD VERSION
        // switch (rank)
        // {
        //     case 0: // JOKER
        //         throw new ArgumentOutOfRangeException("getValue() -- JOKER of RANK 0 is not supported");
        //     case 1: // ACE
        //         // assumes you want highest value unless 
        //         return ((currentHandValue > 10) ? 1 : 11);
        //     case 2:
        //     case 3:
        //     case 4:
        //     case 5:
        //     case 6:
        //     case 7:
        //     case 8:
        //     case 9:
        //     case 10:
        //         return rank;
        //     case 11: // JACK
        //     case 12: // QUEEN
        //     case 13: // KING
        //         return 10;
        //     default:
        //         throw new ArgumentOutOfRangeException("getValue() -- ranks greater than 13 are not supported.");
        // }
    }

    public string GetASCII()
    {
        // adapted from https://github.com/naivoder/ascii_cards/blob/main/ascii_cards/cards.py
        // modified to C#
        string lineTop    = "┌───────┐";
        string lineMiddle = "│       │";
        string lineBottom = "└───────┘";

        string rankTopLeft      = $" {_rank}";
        string rankBottomRight  = $"{_rank} ";
        string sRank = _rank.ToString();

        if (_rank == 10)  // Ten is the only rank with two digits
        {
            rankTopLeft = sRank;
            rankBottomRight = sRank;
        }
        else
        {
            sRank = _rank switch
            {
                13 => "K",
                12 => "Q",
                11 => "J",
                10 => sRank,
                9 or 8 or 7 or 6 or 5 or 4 or 3 or 2 or 1 => sRank,
                _ => throw new ArgumentOutOfRangeException(nameof(_rank), $"value was {_rank} when it should be in the range 1-13"),
            };
            rankTopLeft = " " + sRank;
            rankBottomRight = sRank + " ";
        }

        // Card lines containing unique card characteristics (suit, rank)
        string lineSuit = $"│   {_suit}   │";
        string lineRankTopLeft = $"│{rankTopLeft}     │";
        string lineRankBottomRight = $"│     {rankBottomRight}│";

        string card_ascii = $"{lineTop}\n{lineRankTopLeft}\n{lineMiddle}\n{lineSuit}"
                            + $"\n{lineMiddle}\n{lineRankBottomRight}\n{lineBottom}";

        return card_ascii;
    }
}