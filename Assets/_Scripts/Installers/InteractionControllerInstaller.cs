namespace cpp.Sen.Gameplay
{
    using Zenject;

    public sealed class InteractionControllerInstaller : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InteractionController>().AsSingle().NonLazy();
        }
        #endregion Public Methods


    }
}