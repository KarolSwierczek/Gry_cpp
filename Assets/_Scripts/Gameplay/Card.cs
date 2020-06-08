namespace cpp.Sen.Gameplay
{
    using System;
    using UnityEngine;

    public sealed class Card : IComparable<Card>
    {
        #region Public Types
        public enum CardType
        {
            Normal,
            Peek,
            Draw,
            Swap,
        }

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

        public sealed class OnInteractionArgs : EventArgs
        {
            public Card Card { get; }

            public OnInteractionArgs(Card card)
            {
                Card = card;
            }
        }
        #endregion Public Types

        #region Public Variables
        public int Value { get; }
        public CardType Type { get; }
        public bool InAnimation { get; set; }
        public bool IsCovered { get; private set; }
        public bool Interactable { get; set; }

        public event EventHandler<OnCardAllignedArgs> OnCardAlligned;
        public event EventHandler<OnCardMovedArgs> OnCardMoved;
        public event EventHandler<OnCardFlippedArgs> OnCardFlipped;
        public event EventHandler<OnInteractionArgs> OnInteraction;
        #endregion Public Variables

        #region Public Methods
        public Card(int value, CardType type)
        {
            Value = value;
            Type = type;
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

        public void Interact()
        {
            if (!Interactable) { return; }
            OnInteraction?.Invoke(this, new OnInteractionArgs(this));
        }

        public int CompareTo(Card other)
        {
            return Value.CompareTo(other.Value);
        }
        #endregion Public Methods
    }
}