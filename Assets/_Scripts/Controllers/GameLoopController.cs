namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;
    using MEC;
    using System.Collections.Generic;
    using System;

    public sealed class GameLoopController : MonoBehaviour
    {
        #region PublicTypes
        public sealed class OnPlayerStartTurnArgs : EventArgs
        {
        }

        public sealed class OnPlayerEndTurnArgs : EventArgs
        {
        }

        public sealed class OnCPUStartTurnArgs : EventArgs
        {
            public CPUPlayer Player { get; }

            public OnCPUStartTurnArgs(CPUPlayer player)
            {
                Player = player;
            }
        }

        public sealed class OnCPUEndTurnArgs : EventArgs
        {
            public CPUPlayer Player { get; }

            public OnCPUEndTurnArgs(CPUPlayer player)
            {
                Player = player;
            }
        }
        #endregion Public Types

        #region Public Variables
        public event EventHandler<OnPlayerStartTurnArgs> OnPlayerStartTurn;
        public event EventHandler<OnPlayerEndTurnArgs> OnPlayerEndTurn;
        public event EventHandler<OnCPUStartTurnArgs> OnCPUStartTurn;
        public event EventHandler<OnCPUEndTurnArgs> OnCPUEndTurn;
        #endregion Public Variables

        #region Public Methods
        public void OnWakeUp()
        {
            //_GameStats.CountPoints(); w sumie to może być on game mode changed
            _GameMode.Mode = GameModeController.GameMode.WakeUp;
        }
        #endregion Public Methods

        #region Unity Methods
        private void OnEnable()
        {
            _CardCollections.OnInitialized += OnCardCollectionsInitialized;
            _Interaction.OnPlayerEndTurn += OnPlayerTurnEnd;
        }

        private void OnDisable()
        {
            _CardCollections.OnInitialized -= OnCardCollectionsInitialized;
            _Interaction.OnPlayerEndTurn -= OnPlayerTurnEnd;
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private InteractionController _Interaction;
        [Inject] private GameModeController _GameMode;
        [Inject] private CardCollections _CardCollections;

        private PlayerHand[] _CPUPlayerHands;
        private CPUPlayer[] _CPUPlayers;
        #endregion Private Variables

        #region Private Methods
        private void OnCardCollectionsInitialized(object sender, CardCollections.OnInitializedArgs args)
        {
            var playerCount = args.PlayerHands.Length;

            _CPUPlayerHands = new PlayerHand[playerCount - 1];
            _CPUPlayers = new CPUPlayer[playerCount - 1];

            for (var i = 0; i < playerCount -1; i++) 
            { 
                _CPUPlayerHands[i] = args.PlayerHands[i+1];
                _CPUPlayers[i] = new CPUPlayer(_CPUPlayerHands[i], _CardCollections);
            }
        }

        private void OnPlayerTurnEnd(object sender, InteractionController.OnPlayerEndTurnArgs args)
        {          
            Timing.RunCoroutine(CPUTurnCoroutine());
        }

        private IEnumerator<float> CPUTurnCoroutine()
        {
            OnPlayerEndTurn?.Invoke(this, new OnPlayerEndTurnArgs());
            _Interaction.Locked = true;
            yield return Timing.WaitForSeconds(4f);

            foreach (var player in _CPUPlayers)
            {
                OnCPUStartTurn?.Invoke(this, new OnCPUStartTurnArgs(player));

                player.PlayTurn();
                yield return Timing.WaitForSeconds(4f); //todo: move to settings

                OnCPUEndTurn?.Invoke(this, new OnCPUEndTurnArgs(player));
            }

            _Interaction.Locked = false;
            OnPlayerStartTurn?.Invoke(this, new OnPlayerStartTurnArgs());
        }
        #endregion Private Methods
    }
}
