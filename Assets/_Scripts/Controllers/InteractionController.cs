namespace cpp.Sen.Gameplay
{
    using System.Collections.Generic;
    using Zenject;
    using System;

    public sealed class InteractionController
    {
        #region Public Types
        public enum CardCollectionType
        {
            Draw,
            Discard,
            Inspect,
            Player,
            CPU,
        }

        public sealed class OnPlayerEndTurnArgs : EventArgs
        {
        }
        #endregion Public Types

        #region Public Methods
        public void Initialize() //todo: messy
        {
            if (_IsInitialized) { return; }

            PopulateRuleBook();
            _IsInitialized = true;
        }

        public void OnInteraction(Card card, ICardCollection source)
        {
            if (!_IsInitialized || Locked) { return; }
            if (!_CurrentInteractionRule.CanInteract(source.Type)) { return; }

            _CurrentInteractionRule.Interact(card, source);

            if(_CurrentInteractionRule.NextRule == InteractionRules.InteractionRuleType.Default)
            {
                OnPlayerEndTurn?.Invoke(this, new OnPlayerEndTurnArgs());
            }

            _CurrentInteractionRule = _RuleBook[_CurrentInteractionRule.NextRule];            
        }
        #endregion Public Methods

        #region Public Variables
        public event EventHandler<OnPlayerEndTurnArgs> OnPlayerEndTurn;
        public bool Locked { get; set; }
        #endregion Public Variables

        #region Private Variables
        [Inject] private readonly CardCollections _CardCollections;

        private bool _IsInitialized;
        private IInteractionRule _CurrentInteractionRule;
        private Dictionary<InteractionRules.InteractionRuleType, IInteractionRule> _RuleBook = new Dictionary<InteractionRules.InteractionRuleType, IInteractionRule>();
        #endregion Private Variables

        #region Private Methods
        private void PopulateRuleBook()
        {
            _RuleBook.Add(InteractionRules.InteractionRuleType.Peek2, new InteractionRules.Peek2Rule(_CardCollections));
            _RuleBook.Add(InteractionRules.InteractionRuleType.Default, new InteractionRules.DefaultRule(_CardCollections));
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDiscard, new InteractionRules.FromDiscardRule(_CardCollections));
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDraw, new InteractionRules.FromDrawRule(_CardCollections));
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDrawSpecial, new InteractionRules.FromDrawSpecialRule(_CardCollections));
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromPeek1, new InteractionRules.FromPeek1Rule(_CardCollections));
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromSwap2, new InteractionRules.FromSwap2Rule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDraw2, new InteractionRules.FromDraw2Rule(_CardCollections));

            _CurrentInteractionRule = _RuleBook[InteractionRules.InteractionRuleType.Peek2];
        }
        #endregion Private Methods
    }
}
