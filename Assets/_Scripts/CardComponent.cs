namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;

    public sealed class CardComponent : MonoBehaviour
    {
        #region Public Methods
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
        private void OnEnable()
        {
            _Card.OnCardMoved += OnCardMoved;
            _Card.OnCardFlipped += OnCardFlipped;
        }

        private void OnDisable()
        {
            _Card.OnCardMoved -= OnCardMoved;
            _Card.OnCardFlipped -= OnCardFlipped;
        }
        #endregion Unity Methods

        #region Private Variables
        private readonly Card _Card; //todo: probably should be public
        [Inject] private CardAnimator _Animator;
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
