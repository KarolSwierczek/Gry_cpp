namespace cpp.Sen.Gameplay
{
    using Presets;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;
    using Zenject;

    public sealed class GameSetupController : MonoBehaviour
    {
        #region Unity Methods
        private void Awake()
        {
            _PlayerHands = new PlayerHand[4] { _P1Hand, _P2Hand, _P3Hand, _P4Hand };
            _PlayerHandTransforms = new Transform[4] { _P1HandTransform, _P2HandTransform, _P3HandTransform, _P4HandTransform };
        }
        private void OnEnable()
        {
            _GameModeController.OnGameModeChanged += OnGameModeChanged;
        }

        private void OnDisable()
        {
            _GameModeController.OnGameModeChanged -= OnGameModeChanged;
        }
        #endregion Unity Methods

        #region Inspector Variables
        //todo: odin groups
        [SerializeField] private CardSpawnSettings _Settings;

        [SerializeField] private CardStackComponent _StackPrefab;
        [SerializeField] private PlayerHandComponent _HandPrefab;

        [SerializeField] private Transform _CardsParent;
        [SerializeField] private Transform _CoveredStackTransform;
        [SerializeField] private Transform _UncoveredStackTransform;

        [SerializeField] private Transform _P1HandTransform;
        [SerializeField] private Transform _P2HandTransform;
        [SerializeField] private Transform _P3HandTransform;
        [SerializeField] private Transform _P4HandTransform;

        [SerializeField] private Transform _InspectHandTransform;



        #endregion Inspector Variables

        #region Private Variables
        [Inject] private GameModeController _GameModeController;
        [Inject] private CardSpawner _Spawner;

        private CardStack _CoveredStack;
        private CardStack _UncoveredStack;
        private PlayerHand _P1Hand;
        private PlayerHand _P2Hand;
        private PlayerHand _P3Hand;
        private PlayerHand _P4Hand;
        private PlayerHand _InspectHand;


        private Transform[] _PlayerHandTransforms;
        private PlayerHand[] _PlayerHands;
        #endregion Private Variables

        #region Private Methods
        private IEnumerator<float> SetupCoroutine()
        {
            for(var i = 0; i < _GameModeController.NumOfPlayers; i++)
            {
                SpawnPlayerHand(i);
                yield return Timing.WaitForSeconds(_Settings.HandDelay);
            }

            SpawnCoveredStack();
            yield return Timing.WaitForSeconds(_Settings.StackDelay);
            SpawnUncoveredStack();
            yield return Timing.WaitForSeconds(_Settings.StackDelay);
            SpawnInspectHand();

            _GameModeController.Mode = GameModeController.GameMode.Game;
        }

        private void SpawnPlayerHand(int handIndex)
        {
            var cards = _Spawner.SpawnCards(_Settings.CardsPerPlayer, _CardsParent);
            _PlayerHands[handIndex] = new PlayerHand();

            var handComponent = Instantiate(_HandPrefab, _PlayerHandTransforms[handIndex]);
            handComponent.Initialize(_PlayerHands[handIndex]);

            _PlayerHands[handIndex].AddCards(cards);
        }

        private void SpawnCoveredStack()
        {
            var count = _Settings.CardsTotal - _GameModeController.NumOfPlayers * _Settings.CardsPerPlayer;
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

        private void SpawnInspectHand()
        {
            _InspectHand = new PlayerHand(true);

            var handComponent = Instantiate(_HandPrefab, _InspectHandTransform);
            handComponent.Initialize(_InspectHand);
        }

        private void OnGameModeChanged(object sender, GameModeController.OnGameModeChangedArgs args)
        {
            if(args.Mode == GameModeController.GameMode.Setup) { Timing.RunCoroutine(SetupCoroutine()); }
        }
        #endregion Private Methods
    }
}
