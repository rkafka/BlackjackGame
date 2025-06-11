using System.Net;
using System.Reflection.Metadata;

namespace BlackjackGame.Models;

public class Card(string suit, int rank)
{
    // Static fields (shared across all Card objects)
    public static readonly Dictionary<string, string> suitDict = new Dictionary<string, string> {
        { "♠", "Spades" }, { "♠", "Clubs" },
        { "♥", "Hearts" }, { "♦", "Diamonds" },
    };


    // Instance fields (unique to each instance of a Card object)
    public string Suit { get; } = suit;
    public int Rank { get; } = rank;


    // public string getCardName()
    // {
    //     string cardName = $"{suitDict.}";



    //         return;
    // }

}