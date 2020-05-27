namespace cpp.Sen.Gameplay
{
    public interface IInteractableCollection
    {
        void OnInteraction(object sender, Card.OnInteractionArgs args);
    }
}