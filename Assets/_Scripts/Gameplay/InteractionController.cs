namespace cpp.Sen.Gameplay
{
    using System;

    public sealed class InteractionController
    {
        #region Public Types
        public enum InteractableType
        {
            PlayerHand,
            CardStack,
        }

        public sealed class OnInteractionArgs: EventArgs
        {
            public Card Card { get; }
            public ICardCollection Source { get; }
            public InteractableType Type { get; }

            public OnInteractionArgs(Card card, ICardCollection source, InteractableType type)
            {
                Card = card;
                Source = source;
                Type = type;
            }
        }
        #endregion Public Types

        #region Public Methods
        public void Initialize(PlayerHand inspectHand, CardStack uncoveredStack)
        {
            _InspectHand = inspectHand;
            _Discard = uncoveredStack;
        }

        public void Register (Card card)
        {
            //todo maybe separate class for interaction
        }

        public void Unregister(Card card)
        {
            //todo maybe separate class for interaction
        }
        #endregion Public Methods

        #region Private Variables
        private PlayerHand _InspectHand;
        private CardStack _Discard;
        #endregion Private Variables

        #region Private Methods
        private void OnInteraction(object sender, OnInteractionArgs args)
        {
            switch (args.Type)
            {
                case InteractableType.CardStack:
                    {
                        if (_InspectHand.Count > 0) { break; }

                        var topCard = args.Source.RemoveCard(default);
                        _InspectHand.AddCard(topCard);

                        break;
                    }
                case InteractableType.PlayerHand:
                    {
                        if(_InspectHand.Count < 1) { break; }

                        var selectedCard = args.Source.RemoveCard(args.Card);
                        _Discard.AddCard(selectedCard);

                        var inspectedCard = _InspectHand.RemoveFirstCard();
                        args.Source.AddCard(inspectedCard);

                        break;
                    }
            }
        }
        #endregion Private Methods
    }
}
