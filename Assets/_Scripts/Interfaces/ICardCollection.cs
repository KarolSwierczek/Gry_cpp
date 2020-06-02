namespace cpp.Sen.Gameplay 
{
    using System.Collections.Generic;

    public interface ICardCollection
    {
        void AddCards(List<Card> cards);
        void AddCard(Card card);
        Card RemoveCard(Card card);

        InteractionController.CardCollectionType Type {get;}
    }
}