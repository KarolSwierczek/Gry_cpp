namespace cpp.Sen.Presets
{
    using System.Collections.Generic;
    using UnityEngine;
    using Gameplay;

    [CreateAssetMenu(fileName = "CardSettings", menuName = "Sen/CardSettings", order = 2)]
    public sealed class CardSettings : ScriptableObject
    {
        #region Public Types
        [System.Serializable]
        public sealed class CardPreset
        {
            public int ID;
            public int Value;
            public Card.CardType Type;
            public int Count;
            public GameObject Prefab;
        }
        #endregion Public Types

        #region Public Methods
        public CardPreset GetCardPreset(int id)
        {
            return _Cards.Find(x => x.ID == id);
        }

        public List<int> CardIdList => GetCardIdList();
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private List<CardPreset> _Cards;
        #endregion Inspector Variables

        #region Private Methods
        private List<int> GetCardIdList()
        {
            var result = new List<int>();

            foreach(var card in _Cards)
            {
                for(var i = 0; i < card.Count; i++)
                {
                    result.Add(card.ID);
                }
            }

            return result;
        }
        #endregion Private Methods
    }
}