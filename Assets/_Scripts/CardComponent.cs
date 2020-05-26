namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;

    public sealed class CardComponent : MonoBehaviour
    {
        #region Public Methods
        public void Initialize(Card card)
        {
            _Card = card;

            _Card.OnCardMoved += OnCardMoved;
            _Card.OnCardFlipped += OnCardFlipped;
        }

        public void OnAnimationStarted()
        {
            _Card.InAnimation = true;
        }

        public void OnAnimationEnded()
        {
            _Card.InAnimation = false;
        }
        #endregion Public Methods

        #region Unity Methods
        private void OnDestroy()
        {
            _Card.OnCardMoved -= OnCardMoved;
            _Card.OnCardFlipped -= OnCardFlipped;
        }
        #endregion Unity Methods

        #region Private Variables
        private Card _Card;
        [Inject] private CardAnimationController _Animator;
        #endregion Private Variables

        #region Private Methods
        private void OnCardMoved(object sender, Card.OnCardMovedArgs args)
        {
            var targetPosition = args.TargetPosition;
            if (args.Flip)
            {
                _Animator.MoveAndFlipCard(this, targetPosition);
            }
            else
            {
                _Animator.MoveCard(this, targetPosition);
            }
        }

        private void OnCardFlipped(object sender, Card.OnCardFlippedArgs args)
        {
            _Animator.FlipCard(this);
        }
        #endregion Private Methods
    }
}
