namespace cpp.Sen.Gameplay
{
    using System.Collections.Generic;
    using Zenject;
    using System.Linq;

    public sealed class CPUPlayer
    {
        #region Public Methods
        public CPUPlayer (PlayerHand hand, List<Card> startingCards, int numOfKnownCards = 2)
        {
            _Hand = hand;
            for(var i = 0; i < startingCards.Count; i++)
            {
                if(i < numOfKnownCards)
                {
                    _KnownCards.Add(startingCards[i]);
                }
                else
                {
                    _UnknownCards.Add(startingCards[i]);
                }
            }
        }

        public void PlayTurn()
        {
            if (!_CardCollections.IsInitialized) { throw new System.Exception("CPU player is trying to play its turn but card collections are not initialized!"); }

            var maxKnown = _KnownCards.Max();

            if (CompareWithStack(maxKnown, _CardCollections.Discard)) { return; }
            if (CompareWithStack(maxKnown, _CardCollections.Draw)) { return; }
            if (_UnknownCards.Count > 0 && _CardCollections.Draw.TopCard.Value <= 6) { TakeFromCoveredStack(); return; } //todo: add 6 to settings

            DiscardFromCovered();
        }
        #endregion Public Methods

        #region Private Variables
        [Inject] private CardCollections _CardCollections;

        private readonly List<Card> _KnownCards = new List<Card>();
        private readonly List<Card> _UnknownCards = new List<Card>();
        private readonly PlayerHand _Hand;
        #endregion Private Variables

        #region Private Methods
        private bool CompareWithStack(Card maxKnown, CardStack stack)
        {
            if (maxKnown.Value - stack.TopCard.Value > 2)
            {
                _KnownCards.Remove(maxKnown);
                _Hand.RemoveCard(maxKnown);

                var newCard = stack.RemoveCard();
                _Hand.AddCard(newCard);
                _KnownCards.Add(newCard);

                _CardCollections.Discard.AddCard(maxKnown);

                return true;
            }
            return false;
        }

        private void TakeFromCoveredStack()
        {
            var removedCard = _UnknownCards[0];
            _UnknownCards.RemoveAt(0);

            _Hand.RemoveCard(removedCard);
            _CardCollections.Discard.AddCard(removedCard);

            var newCard = _CardCollections.Draw.RemoveCard();
            _Hand.AddCard(newCard);
            _KnownCards.Add(newCard);
        }

        private void DiscardFromCovered()
        {
            var discardedCard = _CardCollections.Draw.RemoveCard();
            _CardCollections.Discard.AddCard(discardedCard);
        }
        #endregion Private Methods
    }
}
