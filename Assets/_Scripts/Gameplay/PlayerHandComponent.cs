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
            _Hand.OnHandChanged += OnHandChanged;
        }
        #endregion Public Methods

        #region Unity Methods
        private void OnDestroy()
        {
            _Hand.OnCardsAdded -= OnCardsAdded;
            _Hand.OnHandChanged -= OnHandChanged;
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private CardSpawnSettings _Settings;
        #endregion Inspector Variables

        #region Private Variables
        private PlayerHand _Hand;
        private CoroutineHandle _RefreshCoroutine;
        #endregion Private Variables

        #region Private Methods
        private void OnHandChanged(object sender, PlayerHand.OnHandChangedArgs args)
        {
            if(_RefreshCoroutine != null) { Timing.KillCoroutines(_RefreshCoroutine); }
            _RefreshCoroutine = Timing.RunCoroutine(RefreshHandCoroutine(args.CurrentCards));
        }

        private void OnCardsAdded(object sender, PlayerHand.OnCardsAddedArgs args)
        {
            foreach(var card in args.Cards) { card.AllignCard(transform.forward); }
        }

        private Vector3 GetCardPosition(int cardIndex)
        {
            if (_Hand.Count <= cardIndex) { throw new System.ArgumentException("Card index out of range!"); }

            var deltaX = (0.5f * (1-_Hand.Count) + cardIndex) * _Settings.Width;

            return transform.position + transform.right * deltaX;
        }

        private IEnumerator<float> RefreshHandCoroutine(List<Card> cards)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                card.MoveCard(GetCardPosition(i), transform.forward, card.IsCovered == _Hand.IsInspect);

                yield return Timing.WaitForSeconds(_Settings.CardDelay);
            }
        }
        #endregion Private Methods
    }
}
