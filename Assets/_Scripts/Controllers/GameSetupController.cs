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
        [Inject] private InteractionController _Interaction;
        [Inject] private CardCollections _CardCollections;

        private PlayerHand _P1Hand;
        private PlayerHand _P2Hand;
        private PlayerHand _P3Hand;
        private PlayerHand _P4Hand;

        private CardStack _Draw;
        private CardStack _Discard;
        private PlayerHand _Inspect;

        private Transform[] _PlayerHandTransforms;
        private PlayerHand[] _PlayerHands;
        #endregion Private Variables

        #region Private Methods
        private IEnumerator<float> SetupCoroutine()
        {
            _PlayerHands = new PlayerHand[_GameModeController.NumOfPlayers];

            for (var i = 0; i < _GameModeController.NumOfPlayers; i++)
            {
                SpawnPlayerHand(i, i==0);
                yield return Timing.WaitForSeconds(_Settings.HandDelay);
            }

            SpawnDrawStack();
            yield return Timing.WaitForSeconds(_Settings.StackDelay);

            SpawnDiscardStack();
            yield return Timing.WaitForSeconds(_Settings.StackDelay);

            SpawnInspectHand();

            _CardCollections.Initialize(_Draw, _Discard, _Inspect, _PlayerHands);
            _Interaction.Initialize();

            _GameModeController.Mode = GameModeController.GameMode.Game;
        }

        private void SpawnPlayerHand(int handIndex, bool isPlayer = false)
        {
            var cards = _Spawner.SpawnCards(_Settings.CardsPerPlayer, _CardsParent);
            var cardCollectionType = isPlayer ? InteractionController.CardCollectionType.Player : InteractionController.CardCollectionType.CPU;
            _PlayerHands[handIndex] = new PlayerHand(_Interaction, cardCollectionType);

            var handComponent = Instantiate(_HandPrefab, _PlayerHandTransforms[handIndex]);
            handComponent.Initialize(_PlayerHands[handIndex]);

            _PlayerHands[handIndex].AddCards(cards);
        }

        private void SpawnDrawStack()
        {
            var count = _Settings.CardsTotal - _GameModeController.NumOfPlayers * _Settings.CardsPerPlayer;
            var cards = _Spawner.SpawnCards(count, _CardsParent);
            _Draw = new CardStack(_Interaction, InteractionController.CardCollectionType.Draw);

            var stackComponent = Instantiate(_StackPrefab, _CoveredStackTransform);
            stackComponent.Initialize(_Draw);

            _Draw.AddCards(cards);
        }

        private void SpawnDiscardStack()
        {
            _Discard = new CardStack(_Interaction, InteractionController.CardCollectionType.Discard);

            var stackComponent = Instantiate(_StackPrefab, _UncoveredStackTransform);
            stackComponent.Initialize(_Discard);

            _Discard.AddCard(_Draw.RemoveCard());
        }

        private void SpawnInspectHand()
        {
            _Inspect = new PlayerHand(_Interaction, InteractionController.CardCollectionType.Inspect);

            var handComponent = Instantiate(_HandPrefab, _InspectHandTransform);
            handComponent.Initialize(_Inspect);
        }

        private void OnGameModeChanged(object sender, GameModeController.OnGameModeChangedArgs args)
        {
            if(args.Mode == GameModeController.GameMode.Setup) { Timing.RunCoroutine(SetupCoroutine()); }
        }
        #endregion Private Methods
    }
}
