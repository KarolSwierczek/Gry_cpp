namespace cpp.Sen.Gameplay
{
    using Zenject;
    using Presets;
    using UnityEngine;

    public class CardAnimationControllerInstaller : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CardAnimationController>().AsSingle().WithArguments(_Settings);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private AnimationSettings _Settings;
        #endregion Inspector Variables
    }
}