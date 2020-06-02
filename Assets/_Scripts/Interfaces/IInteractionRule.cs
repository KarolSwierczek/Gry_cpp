namespace cpp.Sen.Gameplay
{
    public interface IInteractionRule
    {
        bool CanInteract(InteractionController.CardCollectionType type);
        void Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard);

        InteractionRules.InteractionRuleType NextRule { get; }
    }
}