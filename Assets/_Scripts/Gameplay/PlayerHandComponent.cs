namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Presets;
    using MEC;
    using System.Collections.Generic;

    public class PlayerHandComponent : MonoBehaviour
    {
        #region Public Methods
        public void Initialize(PlayerHand hand)
        {
            _Hand = hand;

            _Hand.OnCardsAdded += OnCardsAdded;
            _Hand.OnCardAdded += OnCardAdded;
            _Hand.OnCardRemoved += OnCardRemoved;
        }
        #endregion Public Methods

        #region Unity Methods
        private void OnDestroy()
        {
            _Hand.OnCardsAdded -= OnCardsAdded;
            _Hand.OnCardAdded -= OnCardAdded;
            _Hand.OnCardRemoved -= OnCardRemoved;
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private CardSpawnSettings _Settings;
        #endregion Inspector Variables

        #region Private Variables
        private PlayerHand _Hand;
        #endregion Private Variables

        #region Private Methods
        private void OnCardsAdded(object sender, PlayerHand.OnCardsAddedArgs args)
        {
            RefreshCardPositions(args.OtherCards);
            Timing.RunCoroutine(AddCardsCoroutine(args.Cards));
        }

        private void OnCardAdded(object sender, PlayerHand.OnCardAddedArgs args)
        {
            RefreshCardPositions(args.OtherCards);
            args.Card.MoveCard(GetNextCardPosition(), transform.forward, args.Card.IsCovered == _Hand.IsInspect);
        }

        private void OnCardRemoved(object sender, PlayerHand.OnCardRemovedArgs args)
        {
            RefreshCardPositions(args.OtherCards);
        }

        private Vector3 GetNextCardPosition(int numOfCardsAdded = 1)
        {
            if (_Hand.Count <= 0) { throw new System.Exception("Trying to get next card position, but the stack is empty!"); }
            if (_Hand.Count < numOfCardsAdded) { throw new System.ArgumentException("Number of cards added is greater than number of cards in stack!"); }

            var deltaX = (_Hand.Count * 0.5f - numOfCardsAdded + 0.5f) * _Settings.Width;

            return transform.position + transform.right * deltaX;
        }

        private IEnumerator<float> AddCardsCoroutine(List<Card> cards)
        {
            var count = cards.Count;

            foreach (var card in cards)
            {
                card.AllignCard(transform.forward);

                yield return Timing.WaitForOneFrame;

                card.MoveCard(GetNextCardPosition(count), transform.forward, card.IsCovered == _Hand.IsInspect);
                count--;

                yield return Timing.WaitForSeconds(_Settings.CardDelay - Time.deltaTime);
            }
        }

        private void RefreshCardPositions(List<Card> cards)
        {
            var count = cards.Count;

            foreach(var card in cards)
            {
                card.MoveCard(GetNextCardPosition(count), transform.forward, card.IsCovered == _Hand.IsInspect);
                count--;
            }
        }
        #endregion Private Methods
    }
}
