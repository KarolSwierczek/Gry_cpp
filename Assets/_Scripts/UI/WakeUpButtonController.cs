namespace cpp.Sen.Gameplay
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public sealed class WakeUpButtonController : MonoBehaviour
    {
        #region Unity Methods
        private void OnEnable()
        {
            _Button = GetComponent<Button>();

            _Button.onClick.AddListener(OnClick);
            _GameLoop.OnPlayerStartTurn += OnPlayerTurnStart;
            _GameLoop.OnPlayerEndTurn += OnPlayerTurnEnd;
        }

        private void OnDisable()
        {
            _Button.onClick.RemoveListener(OnClick);
            _GameLoop.OnPlayerStartTurn -= OnPlayerTurnStart;
            _GameLoop.OnPlayerEndTurn -= OnPlayerTurnEnd;
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private GameLoopController _GameLoop;
        #endregion Inspector Variables

        #region Private Variables
        private Button _Button;
        #endregion Private Variables

        #region Private Methods
        private void OnPlayerTurnStart(object sender, GameLoopController.OnPlayerStartTurnArgs args)
        {
            _Button.image.enabled = true;
        }

        private void OnPlayerTurnEnd(object sender, GameLoopController.OnPlayerEndTurnArgs args)
        {
            _Button.image.enabled = false;
        }

        private void OnClick()
        {
            _GameLoop.OnWakeUp();
            _Button.image.enabled = false;
        }
        #endregion Private Methods
    }
}
