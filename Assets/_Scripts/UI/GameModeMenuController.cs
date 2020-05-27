namespace cpp.Sen.UI
{
    using Gameplay;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using System.Linq;

    public sealed class GameModeMenuController : SerializedMonoBehaviour
    {
        #region Public Types
        [System.Serializable]
        public sealed class GameModeSettings
        {
            public int Rounds { get; }
            public int Players { get; }

            public GameModeSettings(int rounds, int players)
            {
                Rounds = rounds;
                Players = players;
            }
        }
        #endregion Public Types

        #region Public Methods
        public void OnStartGameClicked()
        {
            //todo: maybe get reference to active toggles from groups
            //todo: make custom toggles with a value
            var activeRounds = _RoundsToggleValues.Keys.FirstOrDefault(x => x.isOn);
            var activePlayers = _PlayersToggleValues.Keys.FirstOrDefault(x => x.isOn);

            var gameModeSettings = new GameModeSettings(_RoundsToggleValues[activeRounds], _PlayersToggleValues[activePlayers]);
            _GameModeController.SetRoundsAndPlayers(gameModeSettings);

            _GameModeController.Mode = GameModeController.GameMode.Setup;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")] private Dictionary<Toggle, int> _RoundsToggleValues;
        [SerializeField, FoldoutGroup("References")] private Dictionary<Toggle, int> _PlayersToggleValues;
        #endregion Inspector Variables

        #region Private Variables
        [Inject] private GameModeController _GameModeController;
        #endregion Private Variables
    }
}
