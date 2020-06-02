namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;
    using MEC;
    using Sirenix.OdinInspector;

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

        public void OnAnimationStarted(CoroutineHandle handle)
        {
            _Card.InAnimation = true;
            AnimationHandle = handle;
        }

        public void OnAnimationEnded()
        {
            _Card.InAnimation = false;
        }
        #endregion Public Methods

        #region Public Variables
        public CoroutineHandle AnimationHandle { get; private set; }
        [ShowInInspector, ReadOnly]
        public bool IsCovered => _Card.IsCovered; //todo: ugly
        #endregion Public Variables

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
