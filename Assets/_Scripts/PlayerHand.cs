namespace cpp.Sen.Gameplay
{
    using System;
    using System.Collections.Generic;

    public sealed class PlayerHand //todo: maybe add abstract class for all card collections
    {
        #region Public Types
        public sealed class OnCardsAddedArgs : EventArgs
        {
            public List<Card> Cards { get; }

            public OnCardsAddedArgs(List<Card> cards)
            {
                Cards = cards;
            }
        }

        public sealed class OnCardAddedArgs : EventArgs
        {
            public Card Card { get; }

            public OnCardAddedArgs(Card card)
            {
                Card = card;
            }
        }

        public sealed class OnCardRemovedArgs : EventArgs
        {
            public Card Card { get; }
            public List<Card> RemainingCards { get; }

            public OnCardRemovedArgs(Card card, List<Card> remainingCards)
            {
                Card = card;
                RemainingCards = remainingCards;
            }
        }
        #endregion Public Types

        #region Public Variables
        public event EventHandler<OnCardsAddedArgs> OnCardsAdded;
        public event EventHandler<OnCardAddedArgs> OnCardAdded;
        public event EventHandler<OnCardRemovedArgs> OnCardRemoved;
        public int Count => _Hand.Count;
        #endregion Public Variables

        #region Public Methods


        public void AddCards(List<Card> cards)
        {
            _Hand.AddRange(cards);
            OnCardsAdded?.Invoke(this, new OnCardsAddedArgs(cards));
        }

        public void AddCard(Card card)
        {
            _Hand.Add(card);
            OnCardAdded?.Invoke(this, new OnCardAddedArgs(card));
        }

        public void RemoveCard(Card card)
        {
            _Hand.Remove(card);
            OnCardRemoved?.Invoke(this, new OnCardRemovedArgs(card, _Hand));
        }
        #endregion Public Methods

        #region Private Variables
        private List<Card> _Hand = new List<Card>();
        #endregion Private Variables
    }
}
