namespace cpp.Sen.Gameplay
{
    public sealed class CardCollections
    {
        #region Public Variables
        public CardStack Draw { get; private set; }
        public CardStack Discard { get; private set; }
        public PlayerHand Inspect { get; private set; }
        public bool IsInitialized { get; private set; }
        #endregion Public Variables

        #region Public Methods
        public void Initialize(CardStack draw, CardStack discard, PlayerHand inspect)
        {
            Draw = draw;
            Discard = discard;
            Inspect = inspect;
            IsInitialized = true;
        }
        #endregion Public Methods
    }
}
