namespace cpp.Sen.Gameplay
{
    using Zenject;

    public sealed class GameModeControllerInstaller : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameModeController>().AsSingle().NonLazy();
        }
        #endregion Public Methods


    }
}