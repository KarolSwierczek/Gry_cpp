namespace cpp.Sen.Gameplay 
{
    using UnityEngine;
    using Zenject;
    using Presets;

    public class CardAnimatorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CardAnimator>().AsSingle().WithArguments(_Settings);
        }

        [SerializeField] private AnimationSettings _Settings;
    }
}