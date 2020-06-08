namespace cpp.Sen.Gameplay
{
    public interface IInteractionRule
    {
        bool CanInteract(InteractionController.CardCollectionType type);
        void Interact(Card card, ICardCollection source);

        InteractionRules.InteractionRuleType NextRule { get; }
    }
}