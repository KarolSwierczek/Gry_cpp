namespace cpp.Sen.Gameplay
{
    using Zenject;

    public sealed class GameModeControllerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameModeController>().AsSingle().NonLazy();
        }
    }
}