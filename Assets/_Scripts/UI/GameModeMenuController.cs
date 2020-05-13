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

        public void OnStartGameClicked()
        {
            //todo: maybe get reference to active toggles from groups
            //todo: make custom toggles with a value
            var activeRounds = _RoundsToggleValues.Keys.FirstOrDefault(x => x.isOn);
            var activePlayers = _PlayersToggleValues.Keys.FirstOrDefault(x => x.isOn);

            _GameModeController.Mode = GameModeController.GameMode.Game;
            //todo: send settings to gameplay controller
            //return new GameModeSettings(_RoundsToggleValues[activeRounds], _PlayersToggleValues[activePlayers]);
        }

        [SerializeField, FoldoutGroup("References")] private Dictionary<Toggle, int> _RoundsToggleValues;
        [SerializeField, FoldoutGroup("References")] private Dictionary<Toggle, int> _PlayersToggleValues;

        [Inject] private GameModeController _GameModeController;
    }
}
