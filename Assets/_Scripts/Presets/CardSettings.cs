namespace cpp.Sen.Presets
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CardSettings", menuName = "Sen/CardSettings", order = 2)]
    public sealed class CardSettings : ScriptableObject
    {
        #region Public Types
        [System.Serializable]
        public sealed class CardPreset
        {
            public int Value;
            public Sprite Graphic;
        }
        #endregion Public Types

        #region Public Methods
        public Sprite GetCardGraphic(int value)
        {
            return _Cards.Find(x => x.Value == value).Graphic;
        }

        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private List<CardPreset> _Cards;
        #endregion Inspector Variables
    }
}