namespace cpp.Sen.Gameplay
{
    using Zenject;

    public sealed class CardCollectionsInstaller : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CardCollections>().AsSingle().NonLazy();
        }
        #endregion Public Methods


    }
}