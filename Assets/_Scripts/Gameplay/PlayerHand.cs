namespace cpp.Sen.Gameplay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class PlayerHand : ICardCollection, IInteractableCollection, ICPUPlayerHand
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

        public sealed class OnHandChangedArgs : EventArgs
        {
            public List<Card> CurrentCards { get; }

            public OnHandChangedArgs(List<Card> currentCards)
            {
                CurrentCards = currentCards;
            }
        }
        #endregion Public Types

        #region Public Variables
        public event EventHandler<OnCardsAddedArgs> OnCardsAdded;
        public event EventHandler<OnCardAddedArgs> OnCardAdded;
        public event EventHandler<OnCardRemovedArgs> OnCardRemoved;
        public event EventHandler<OnHandChangedArgs> OnHandChanged;

        public int Count => _Hand.Count;
        public int Sum => _Hand.Select(x => x.Value).Sum();
        public InteractionController.CardCollectionType Type { get; }
        public bool IsCovered => Type != InteractionController.CardCollectionType.Inspect;
        #endregion Public Variables

        #region Public Methods
        public PlayerHand(InteractionController interaction, InteractionController.CardCollectionType type)
        {
            _Interaction = interaction;
            Type = type;
        }

        public void AddCards(List<Card> cards)
        {
            _Hand.AddRange(cards);
        
            foreach(var card in cards)
            {
                card.Interactable = true;
                card.OnInteraction += OnInteraction;
            }

            OnCardsAdded?.Invoke(this, new OnCardsAddedArgs(cards));
            OnHandChanged?.Invoke(this, new OnHandChangedArgs(_Hand));
        }

        public void AddCard(Card card)
        {
            _Hand.Add(card);

            card.Interactable = true;
            card.OnInteraction += OnInteraction;

            OnCardAdded?.Invoke(this, new OnCardAddedArgs(card));
            OnHandChanged?.Invoke(this, new OnHandChangedArgs(_Hand));
        }

        public Card RemoveCard(Card card)
        {
            _Hand.Remove(card);
            card.Interactable = false;
            card.OnInteraction -= OnInteraction;

            OnCardRemoved?.Invoke(this, new OnCardRemovedArgs(card));
            OnHandChanged?.Invoke(this, new OnHandChangedArgs(_Hand));

            return card;
        }
        public Card RemoveFirstCard()
        {
            if (_Hand.Count <= 0) { throw new Exception("Trying to remove a card that does not exist"); }

            return RemoveCard(_Hand[0]);
        }

        public Card GetFirstCard()
        {
            if(_Hand.Count <= 0) { throw new Exception("Trying to get a card that does not exist"); }

            return _Hand[0];
        }

        public void OnInteraction(object sender, Card.OnInteractionArgs args)
        {
            _Interaction.OnInteraction(args.Card, this);
        }

        List<Card> ICPUPlayerHand.GetCards()
        {
            return _Hand;
        }

        void ICPUPlayerHand.AddCard(Card card)
        {
            AddCard(card);
        }

        void ICPUPlayerHand.RemoveCard(Card card)
        {
            RemoveCard(card);
        }
        #endregion Public Methods

        #region Private Variables
        private List<Card> _Hand = new List<Card>();
        private readonly InteractionController _Interaction;
        #endregion Private Variables
    }
}
