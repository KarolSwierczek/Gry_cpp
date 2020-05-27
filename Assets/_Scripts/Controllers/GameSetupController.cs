namespace cpp.Sen.Gameplay
{
    using Presets;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;
    using Zenject;

    public sealed class GameSetupController : MonoBehaviour
    {
        //todo: get from main menu
        private const int _Players = 4;
        //private const int _Rounds = 10;

        //todo: dimention settings and spawn setting separate
        private const int _CardsPerPlayer = 4;
        private const int _CardsTotal = 54;

        private void OnEnable()
        {
            _GameModeController.OnGameModeChanged += OnGameModeChanged;
        }

        private void OnDisable()
        {
            _GameModeController.OnGameModeChanged -= OnGameModeChanged;
        }

        [SerializeField] private CardSpawnSettings _Settings;

        [SerializeField] private CardStackComponent _StackPrefab;
        //[SerializeField] private PlayerHandComponent _HandPrefab;

        [SerializeField] private Transform _CardsParent;
        [SerializeField] private Transform _CoveredStackTransform;
        [SerializeField] private Transform _UncoveredStackTransform;
        //[SerializeField] private Transform _P1HandTransform;
        //[SerializeField] private Transform _P2HandTransform;
        //[SerializeField] private Transform _P3HandTransform;
        //[SerializeField] private Transform _P4HandTransform;

        [Inject] private GameModeController _GameModeController;
        [Inject] private CardSpawner _Spawner;

        private CardStack _CoveredStack;
        private CardStack _UncoveredStack;
        //private PlayerHand _P1Hand;
        //private PlayerHand _P2Hand;
        //private PlayerHand _P3Hand;
        //private PlayerHand _P4Hand;

        private IEnumerator<float> SetupCoroutine()
        {
            SpawnCoveredStack();
            yield return Timing.WaitForSeconds(_Settings.CollectionDelay);
            SpawnUncoveredStack();
            yield return Timing.WaitForSeconds(_Settings.CollectionDelay);

            _GameModeController.Mode = GameModeController.GameMode.Game;
        }

        private void SpawnCoveredStack()
        {
            var count = _CardsTotal - _Players * _CardsPerPlayer;
            var cards = _Spawner.SpawnCards(count, _CardsParent);
            _CoveredStack = new CardStack(true);

            var stackComponent = Instantiate(_StackPrefab, _CoveredStackTransform);
            stackComponent.Initialize(_CoveredStack);

            _CoveredStack.AddCards(cards);
        }

        private void SpawnUncoveredStack()
        {
            _UncoveredStack = new CardStack(false);

            var stackComponent = Instantiate(_StackPrefab, _UncoveredStackTransform);
            stackComponent.Initialize(_UncoveredStack);

            _UncoveredStack.AddCard(_CoveredStack.RemoveCard());
        }

        private void OnGameModeChanged(object sender, GameModeController.OnGameModeChangedArgs args)
        {
            if(args.Mode == GameModeController.GameMode.Game) { Timing.RunCoroutine(SetupCoroutine()); }
        }
    }
}
