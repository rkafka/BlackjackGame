namespace BlackjackGame.Models;

public class Hand
{
    public List<Card> cardList;
    public bool isDealer;

    public Hand(bool isDealer=false)
    {
        this.isDealer = isDealer;

        cardList = [];
    }

    public void AddCard(Deck deck)
    {
        this.cardList.Add(deck.PullCard());
    }
}