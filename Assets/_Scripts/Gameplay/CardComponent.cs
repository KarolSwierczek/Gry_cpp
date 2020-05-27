namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;

    public sealed class CardComponent : MonoBehaviour, IInteractable
    {
        #region Public Methods
        public void Initialize(Card card)
        {
            _Card = card;

            _Card.OnCardAlligned += OnCardAlligned;
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
            _Card.OnCardAlligned -= OnCardAlligned;
            _Card.OnCardMoved -= OnCardMoved;
            _Card.OnCardFlipped -= OnCardFlipped;
        }
        #endregion Unity Methods

        #region Private Variables
        private Card _Card;
        [Inject] private CardAnimationController _Animator;
        #endregion Private Variables

        #region Private Methods
        private void OnCardAlligned(object sender, Card.OnCardAllignedArgs args)
        {
            _Animator.AllignCard(this, args.ForwardDirection);
        }

        private void OnCardMoved(object sender, Card.OnCardMovedArgs args)
        {
            var targetPosition = args.TargetPosition;
            var forwardDirection = args.ForwardDirection;

            if (args.Flip)
            {
                _Animator.MoveAndFlipCard(this, targetPosition, forwardDirection);
            }
            else
            {
                _Animator.MoveCard(this, targetPosition, forwardDirection);
            }
        }

        private void OnCardFlipped(object sender, Card.OnCardFlippedArgs args)
        {
            _Animator.FlipCard(this);
        }

        void IInteractable.Interact()
        {
            _Card.Interact();
        }
        #endregion Private Methods
    }
}
