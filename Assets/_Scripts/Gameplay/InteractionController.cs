namespace cpp.Sen.Gameplay
{
    public sealed class InteractionController
    {
        //todo: inject game mode and get info about special cards => modify interactions
        #region Public Types
        public enum InteractableType
        {
            PlayerHand,
            CardStack,
        }
        #endregion Public Types

        #region Public Methods
        public void Initialize(PlayerHand inspectHand, CardStack uncoveredStack)
        {
            _InspectHand = inspectHand;
            _Discard = uncoveredStack;
        }

        public void OnInteraction(Card card, ICardCollection source, InteractableType type)
        {
            switch (type)
            {
                case InteractableType.CardStack:
                    {
                        if (_InspectHand.Count > 0) { break; }

                        var topCard = source.RemoveCard(default);
                        _InspectHand.AddCard(topCard);

                        break;
                    }
                case InteractableType.PlayerHand:
                    {
                        if (_InspectHand.Count < 1) { break; }

                        var selectedCard = source.RemoveCard(card);
                        _Discard.AddCard(selectedCard);

                        var inspectedCard = _InspectHand.RemoveFirstCard();
                        source.AddCard(inspectedCard);

                        break;
                    }
            }
        }
        #endregion Public Methods

        #region Private Variables
        private PlayerHand _InspectHand;
        private CardStack _Discard;
        #endregion Private Variables
    }
}
