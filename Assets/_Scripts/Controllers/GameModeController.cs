namespace cpp.Sen.Gameplay
{
    using System;
    using UI;

    public sealed class GameModeController
    {
        #region Public Types
        public enum GameMode
        {
            Menu,
            Setup,
            Game,
            WakeUp,
        }

        public sealed class OnGameModeChangedArgs
        {
            public GameMode Mode { get; }

            public OnGameModeChangedArgs(GameMode mode)
            {
                Mode = mode;
            }
        }
        #endregion Public Types

        #region Public Methods
        public void SetRoundsAndPlayers(GameModeMenuController.GameModeSettings menuSettings)
        {
            NumOfRounds = menuSettings.Rounds;
            NumOfPlayers = menuSettings.Players;
        }
        #endregion Public Methods

        #region Public Variables
        public GameMode Mode {
            get { return _Mode; }
            set
            {
                switch (value)
                {
                    case GameMode.Menu:
                        OnMenu();
                        break;
                    case GameMode.Game:
                        OnGame();
                        break;
                    case GameMode.WakeUp:
                        OnWakeUp();
                        break;
                }

                _Mode = value;
                OnGameModeChanged?.Invoke(this, new OnGameModeChangedArgs(_Mode));
            }
        }

        public int NumOfRounds { get; private set; }
        public int NumOfPlayers { get; private set; }


        public event EventHandler<OnGameModeChangedArgs> OnGameModeChanged;
        #endregion Public Variables

        #region Private Variables
        private GameMode _Mode;
        #endregion Private Variables

        #region Private Methods
        private void OnMenu() { }
        private void OnGame() { }
        private void OnWakeUp() { }
        #endregion Private Methods
    }
}
