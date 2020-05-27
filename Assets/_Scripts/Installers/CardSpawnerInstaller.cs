namespace cpp.Sen.Gameplay
{
    using Zenject;
    using Presets;
    using UnityEngine;

    public class CardSpawnerInstaller : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CardSpawner>().AsSingle().WithArguments(_Settings);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private CardSettings _Settings;
        #endregion Inspector Variables
    }
} 