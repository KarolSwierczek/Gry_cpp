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
            public int Count;
            public GameObject Prefab;
        }
        #endregion Public Types

        #region Public Methods
        public GameObject GetCardPrefab(int value)
        {
            return _Cards.Find(x => x.Value == value).Prefab;
        }

        public List<int> CardValueList => GetCardValueList();
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private List<CardPreset> _Cards;
        #endregion Inspector Variables

        #region Private Methods
        private List<int> GetCardValueList()
        {
            var result = new List<int>();

            foreach(var card in _Cards)
            {
                for(var i = 0; i < card.Count; i++)
                {
                    result.Add(card.Value);
                }
            }

            return result;
        }
        #endregion Private Methods
    }
}