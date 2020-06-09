namespace cpp.Sen.Gameplay
{
    using System;

    public sealed class CardCollections
    {
        #region Public Types
        public sealed class OnInitializedArgs : EventArgs
        {
            public PlayerHand[] PlayerHands { get; }

            public OnInitializedArgs(PlayerHand[] playerHands)
            {
                PlayerHands = playerHands;
            }
        }
        #endregion Public Types

        #region Public Variables
        public event EventHandler<OnInitializedArgs> OnInitialized;

        public CardStack Draw { get; private set; }
        public CardStack Discard { get; private set; }
        public PlayerHand Inspect { get; private set; }
        public PlayerHand[] PlayerHands { get; private set; }
        public bool IsInitialized { get; private set; }
        #endregion Public Variables

        #region Public Methods
        public void Initialize(CardStack draw, CardStack discard, PlayerHand inspect, PlayerHand[] playerHands)
        {
            Draw = draw;
            Discard = discard;
            Inspect = inspect;
            PlayerHands = playerHands;
            IsInitialized = true;

            OnInitialized?.Invoke(this, new OnInitializedArgs(PlayerHands));
        }

        public void Reset() //todo: refactor
        {
            Draw = null;
            Discard = null;
            Inspect = null;
            PlayerHands = null;
            IsInitialized = false;
        }
        #endregion Public Methods
    }
}
