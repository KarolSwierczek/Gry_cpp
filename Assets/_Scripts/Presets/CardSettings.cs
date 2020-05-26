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
            public GameObject Prefab;
        }
        #endregion Public Types

        #region Public Methods
        public GameObject GetCardPrefab(int value)
        {
            return _Cards.Find(x => x.Value == value).Prefab;
        }

        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private List<CardPreset> _Cards;
        #endregion Inspector Variables
    }
}