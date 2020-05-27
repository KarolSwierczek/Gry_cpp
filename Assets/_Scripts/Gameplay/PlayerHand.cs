namespace cpp.Sen.Gameplay
{
    using System;
    using System.Collections.Generic;

    public sealed class PlayerHand : ICardCollection, IInteractableCollection
    {
        #region Public Types
        public sealed class OnCardsAddedArgs : EventArgs
        {
            public List<Card> Cards { get; }
            public List<Card> OtherCards { get; }

            public OnCardsAddedArgs(List<Card> cards, List<Card> otherCards)
            {
                Cards = cards;
                OtherCards = otherCards;
            }
        }

        public sealed class OnCardAddedArgs : EventArgs
        {
            public Card Card { get; }
            public List<Card> OtherCards { get; }

            public OnCardAddedArgs(Card card, List<Card> otherCards)
            {
                Card = card;
                OtherCards = otherCards;
            }
        }

        public sealed class OnCardRemovedArgs : EventArgs
        {
            public Card Card { get; }
            public List<Card> OtherCards { get; }

            public OnCardRemovedArgs(Card card, List<Card> otherCards)
            {
                Card = card;
                OtherCards = otherCards;
            }
        }
        #endregion Public Types

        #region Public Variables
        public event EventHandler<OnCardsAddedArgs> OnCardsAdded;
        public event EventHandler<OnCardAddedArgs> OnCardAdded;
        public event EventHandler<OnCardRemovedArgs> OnCardRemoved;
        public int Count => _Hand.Count;
        public bool IsInspect { get; }
        #endregion Public Variables

        #region Public Methods
        public PlayerHand(InteractionController interaction, bool isInspect = false)
        {
            IsInspect = isInspect;
            _Interaction = interaction;
        }

        public void AddCards(List<Card> cards)
        {
            var other = _Hand;
            _Hand.AddRange(cards);
            foreach(var card in cards)
            {
                if (!IsInspect) { card.Interactable = true; }
                card.OnInteraction += OnInteraction;
            }

            OnCardsAdded?.Invoke(this, new OnCardsAddedArgs(cards, _Hand));
        }

        public void AddCard(Card card)
        {
            var other = _Hand;
            _Hand.Add(card);
            if (!IsInspect) { card.Interactable = true; }
            card.OnInteraction += OnInteraction;

            OnCardAdded?.Invoke(this, new OnCardAddedArgs(card, _Hand));
        }

        public Card RemoveCard(Card card)
        {
            _Hand.Remove(card);
            card.Interactable = false;
            card.OnInteraction -= OnInteraction;

            OnCardRemoved?.Invoke(this, new OnCardRemovedArgs(card, _Hand));

            return card;
        }
        public Card RemoveFirstCard()
        {
            var card = _Hand[0];

            return RemoveCard(card);
        }

        public void OnInteraction(object sender, Card.OnInteractionArgs args)
        {
            _Interaction.OnInteraction(args.Card, this, InteractionController.InteractableType.PlayerHand);
        }
        #endregion Public Methods

        #region Private Variables
        private List<Card> _Hand = new List<Card>();
        private readonly InteractionController _Interaction;
        #endregion Private Variables
    }
}
