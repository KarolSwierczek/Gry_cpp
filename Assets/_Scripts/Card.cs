namespace cpp.Sen.Gameplay
{
    using System;
    using UnityEngine;

    public sealed class Card
    {
        #region Public Types
        public sealed class OnCardAllignedArgs : EventArgs
        {
            public Vector3 ForwardDirection { get; }

            public OnCardAllignedArgs(Vector3 forwardDirection)
            {
                ForwardDirection = forwardDirection;
            }
        }

        public sealed class OnCardMovedArgs : EventArgs
        {
            public Vector3 TargetPosition { get; }
            public Vector3 ForwardDirection { get; }
            public bool Flip { get; }

            public OnCardMovedArgs(Vector3 targetPosition, Vector3 forwardDirection, bool flip)
            {
                TargetPosition = targetPosition;
                ForwardDirection = forwardDirection;
                Flip = flip;
            }
        }

        public sealed class OnCardFlippedArgs : EventArgs
        {
        }
        #endregion Public Types

        #region Public Variables
        public int Value { get; }
        public bool InAnimation { get; set; }
        public bool IsCovered { get; private set; }

        public event EventHandler<OnCardAllignedArgs> OnCardAlligned;
        public event EventHandler<OnCardMovedArgs> OnCardMoved;
        public event EventHandler<OnCardFlippedArgs> OnCardFlipped;
        #endregion Public Variables

        #region Public Methods
        public Card(int value)
        {
            Value = value;
            IsCovered = true;
        }

        public void AllignCard(Vector3 forward)
        {
            OnCardAlligned?.Invoke(this, new OnCardAllignedArgs(forward));
        }

        public void MoveCard(Vector3 position, Vector3 forward, bool flip = false)
        {
            if (flip) { IsCovered = !IsCovered; }
            OnCardMoved?.Invoke(this, new OnCardMovedArgs(position, forward, flip));
        }

        public void FlipCard()
        {
            IsCovered = !IsCovered;
            OnCardFlipped?.Invoke(this, new OnCardFlippedArgs());
        }
        #endregion Public Methods
    }
}