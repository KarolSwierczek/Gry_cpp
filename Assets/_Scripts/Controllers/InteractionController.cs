using System.Collections.Generic;

namespace cpp.Sen.Gameplay
{
    public sealed class InteractionController
    {
        #region Public Types
        public enum CardCollectionType
        {
            Draw,
            Discard,
            Inspect,
            Player,
            NPC,
        }
        #endregion Public Types

        #region Public Methods
        public void Initialize(PlayerHand inspectHand, CardStack coveredStack, CardStack uncoveredStack)
        {
            _Inspect = inspectHand;
            _Draw = coveredStack;
            _Discard = uncoveredStack;

            PopulateRuleBook();
        }

        public void OnInteraction(Card card, ICardCollection source)
        {
            if (!_CurrentInteractionRule.CanInteract(source.Type)) { return; }

            _CurrentInteractionRule.Interact(card, source, _Inspect, _Draw, _Discard);
            _CurrentInteractionRule = _RuleBook[_CurrentInteractionRule.NextRule];
        }
        #endregion Public Methods

        #region Private Variables
        private IInteractionRule _CurrentInteractionRule;

        private PlayerHand _Inspect;
        private CardStack _Discard;
        private CardStack _Draw;

        private Dictionary<InteractionRules.InteractionRuleType, IInteractionRule> _RuleBook = new Dictionary<InteractionRules.InteractionRuleType, IInteractionRule>();
        #endregion Private Variables

        #region Private Methods
        private void PopulateRuleBook()
        {
            _RuleBook.Add(InteractionRules.InteractionRuleType.Default, new InteractionRules.DefaultRule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDiscard, new InteractionRules.FromDiscardRule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDraw, new InteractionRules.FromDrawRule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDrawSpecial, new InteractionRules.FromDrawSpecialRule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromPeek1, new InteractionRules.FromPeek1Rule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromSwap2, new InteractionRules.FromSwap2Rule());
            _RuleBook.Add(InteractionRules.InteractionRuleType.FromDraw2, new InteractionRules.FromDraw2Rule());

            _CurrentInteractionRule = _RuleBook[InteractionRules.InteractionRuleType.Default];
        }
        #endregion Private Methods
    }
}
