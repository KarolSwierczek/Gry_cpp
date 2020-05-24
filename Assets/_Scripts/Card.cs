namespace cpp.Sen.Gameplay
{
    using System;
    using UnityEngine;

    public sealed class Card
    {
        #region Public Types
        public sealed class OnCardMovedArgs : EventArgs
        {
            public Vector3 TargetPosition { get; }
            public bool Flip { get; }

            public OnCardMovedArgs(Vector3 targetPosition, bool flip = false)
            {
                TargetPosition = targetPosition;
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

        public event EventHandler<OnCardMovedArgs> OnCardMoved;
        public event EventHandler<OnCardFlippedArgs> OnCardFlipped;
        #endregion Public Variables

        #region Public Methods
        public Card(int value)
        {
            Value = value;
        }
        #endregion Public Methods
    }
}