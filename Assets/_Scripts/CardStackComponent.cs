namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Presets;
    using MEC;

    public sealed class CardStackComponent : MonoBehaviour
    {
        #region Public Methods
        public void Initialize(CardStack stack)
        {
            _Stack = stack;

            _Stack.OnCardsAdded += OnCardsAdded;
            _Stack.OnCardAdded += OnCardAdded;
            _Stack.OnCardRemoved += OnCardRemoved;
        }
        #endregion Public Methods

        #region Unity Methods
        private void OnDestroy()
        {
            _Stack.OnCardsAdded -= OnCardsAdded;
            _Stack.OnCardAdded -= OnCardAdded;
            _Stack.OnCardRemoved -= OnCardRemoved;
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private CardDimentionsSettings _Settings;
        #endregion Inspector Variables

        #region Private Variables
        private CardStack _Stack;
        #endregion Private Variables

        #region Private Methods
        private void OnCardsAdded(object sender, CardStack.OnCardsAddedArgs args)
        {
            foreach(var card in args.Cards)
            {
                var flip = card.IsCovered != _Stack.IsCovered;
                card.MoveCard(GetNextCardPosition(), transform.forward, flip);
            }
        }

        private void OnCardAdded(object sender, CardStack.OnCardAddedArgs args)
        {
            var flip = args.Card.IsCovered != _Stack.IsCovered;
            args.Card.MoveCard(GetNextCardPosition(), transform.forward, flip);
        }

        private void OnCardRemoved(object sender, CardStack.OnCardRemovedArgs args)
        {
        }

        private Vector3 GetNextCardPosition(int numOfCardsAdded = 1)
        {
            if(_Stack.Count <= 0) { throw new System.Exception("Trying to get next card position, but the stack is empty!"); }
            if(_Stack.Count <= numOfCardsAdded) { throw new System.ArgumentException("Number of cards added is greater than number of cards in stack!"); }

            var deltaHeight = (_Stack.Count - numOfCardsAdded + 0.5f) * _Settings.Girth;

            return transform.position + new Vector3(0f, deltaHeight, 0f);
        }
        #endregion Private Methods
    }
}
