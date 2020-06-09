namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;
    using MEC;
    using System.Collections.Generic;

    public sealed class GameLoopController : MonoBehaviour
    {
        #region Unity Methods
        private void OnEnable()
        {
            _CardCollections.OnInitialized += OnCardCollectionsInitialized;
            _Interaction.OnPlayerEndTurn += OnPlayerEndTurn;
        }

        private void OnDisable()
        {
            _CardCollections.OnInitialized -= OnCardCollectionsInitialized;
            _Interaction.OnPlayerEndTurn -= OnPlayerEndTurn;
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private InteractionController _Interaction;
        [Inject] private GameModeController _GameMode;
        [Inject] private CardCollections _CardCollections;

        private PlayerHand _Player;
        private PlayerHand[] _CPUPlayerHands;
        private CPUPlayer[] _CPUPlayers;
        #endregion Private Variables

        #region Private Methods
        private void OnCardCollectionsInitialized(object sender, CardCollections.OnInitializedArgs args)
        {
            var playerCount = args.PlayerHands.Length;

            _Player = args.PlayerHands[0];
            _CPUPlayerHands = new PlayerHand[playerCount - 1];
            _CPUPlayers = new CPUPlayer[playerCount - 1];

            for (var i = 0; i < playerCount -1; i++) 
            { 
                _CPUPlayerHands[i] = args.PlayerHands[i+1];
                _CPUPlayers[i] = new CPUPlayer(_CPUPlayerHands[i], _CardCollections);
            }
        }

        private void OnPlayerEndTurn(object sender, InteractionController.OnPlayerEndTurnArgs args)
        {          
            Timing.RunCoroutine(CPUTurnCoroutine());
        }

        private IEnumerator<float> CPUTurnCoroutine()
        {
            _Interaction.Locked = true;
            yield return Timing.WaitForSeconds(4f);

            foreach (var player in _CPUPlayers)
            {
                player.PlayTurn();
                yield return Timing.WaitForSeconds(4f); //todo: move to settings
            }

            _Interaction.Locked = false;
        }
        #endregion Private Methods
    }
}
