namespace cpp.Sen.Gameplay
{
    using System;
    using System.Collections.Generic;

    public sealed class CardStack : ICardCollection
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

            public OnCardRemovedArgs(Card card)
            {
                Card = card;
            }
        }
        #endregion Public Types

        #region Public Variables
        public event EventHandler<OnCardsAddedArgs> OnCardsAdded;
        public event EventHandler<OnCardAddedArgs> OnCardAdded;
        public event EventHandler<OnCardRemovedArgs> OnCardRemoved;
        public Card TopCard => _Stack.Peek();
        public int Count => _Stack.Count;
        public bool IsCovered { get; }
        #endregion Public Variables

        #region Public Methods
        public CardStack(bool isCovered)
        {
            IsCovered = isCovered;
        }

        public void AddCards(List<Card> cards)
        {
            if (TopCard != null) { TopCard.Interactable = false; }

            foreach (var card in cards) { _Stack.Push(card); }
            TopCard.Interactable = true;

            OnCardsAdded?.Invoke(this, new OnCardsAddedArgs(cards));
        }

        public void AddCard(Card card)
        {
            if(TopCard != null) { TopCard.Interactable = false; }

            _Stack.Push(card);
            card.Interactable = true;

            OnCardAdded?.Invoke(this, new OnCardAddedArgs(card));
        }

        public Card RemoveCard(Card optional = null)
        {
            var card = _Stack.Pop();
            card.Interactable = false;

            if(TopCard != null) { TopCard.Interactable = true; }

            OnCardRemoved?.Invoke(this, new OnCardRemovedArgs(card));

            return card;
        }
        #endregion Public Methods

        #region Private Variables
        private Stack<Card> _Stack = new Stack<Card>();
        #endregion Private Variables
    }
}
