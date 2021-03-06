﻿namespace cpp.Sen.Gameplay
{
    using System;
    using System.Collections.Generic;

    public sealed class CardStack : ICardCollection, IInteractableCollection
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
        public bool IsCovered => Type == InteractionController.CardCollectionType.Draw;
        public InteractionController.CardCollectionType Type { get; }
        #endregion Public Variables

        #region Public Methods
        public CardStack(InteractionController interaction, InteractionController.CardCollectionType type)
        {
            Type = type;
            _Interaction = interaction;
        }

        public void AddCards(List<Card> cards)
        {
            if (_Stack.Count > 0) { TopCard.Interactable = false; }

            foreach (var card in cards) 
            { 
                _Stack.Push(card);
                card.OnInteraction += OnInteraction;
            }

            TopCard.Interactable = true;

            OnCardsAdded?.Invoke(this, new OnCardsAddedArgs(cards));
        }

        public void AddCard(Card card)
        {
            if (_Stack.Count > 0) { TopCard.Interactable = false; }

            _Stack.Push(card);
            card.OnInteraction += OnInteraction;

            card.Interactable = true;

            OnCardAdded?.Invoke(this, new OnCardAddedArgs(card));
        }

        public Card RemoveCard(Card optional = null)
        {
            var card = _Stack.Pop();
            card.Interactable = false;
            card.OnInteraction -= OnInteraction;

            if (_Stack.Count > 0) { TopCard.Interactable = true; }

            OnCardRemoved?.Invoke(this, new OnCardRemovedArgs(card));

            return card;
        }

        public void OnInteraction(object sender, Card.OnInteractionArgs args)
        {
            _Interaction.OnInteraction(args.Card, this);
        }
        #endregion Public Methods

        #region Private Variables
        private Stack<Card> _Stack = new Stack<Card>();
        private readonly InteractionController _Interaction;
        #endregion Private Variables
    }
}
