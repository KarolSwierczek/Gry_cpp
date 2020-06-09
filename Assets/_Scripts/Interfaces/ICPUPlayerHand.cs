namespace cpp.Sen.Gameplay
{
    using System.Collections.Generic;

    public interface ICPUPlayerHand
    {
        List<Card> GetCards();
        void AddCard(Card card);
        void RemoveCard(Card card);
    }
}