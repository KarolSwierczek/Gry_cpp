namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Presets;
    using System.Collections.Generic;
    using Zenject;

    public sealed class CardSpawner
    {
        #region Public Methods
        public List<Card> SpawnCards(int count, Transform parent)
        {
            if(_AvailableCardList == null)
            {
                RepopulateAvailableCardList();
            }

            if(count > _AvailableCardList.Count) { throw new System.ArgumentException("Trying to spawn " + count + " cards but there's only " + _AvailableCardList.Count + " cards available!"); }

            var result = new List<Card>();

            for(var i = 0; i < count; i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, _AvailableCardList.Count);
                var cardId = _AvailableCardList[randomIndex];
                
                result.Add(SpawnCard(_Settings.GetCardPreset(cardId), parent));
                _AvailableCardList.RemoveAt(randomIndex);
            }

            return result;
        }

        public void DestroyCards()
        {
            for(var i = _CurrentCardComponents.Count -1; i >= 0; i--)
            {
                Object.Destroy(_CurrentCardComponents[i].gameObject);
            }

            _CurrentCardComponents.Clear();
        }

        public void RepopulateAvailableCardList()
        {
            _AvailableCardList = new List<int>();
            _AvailableCardList.AddRange(_Settings.CardIdList);
        }
        #endregion Public Methods

        #region Private Variables
        [Inject] private readonly CardSettings _Settings;
        private List<int> _AvailableCardList;
        private List<CardComponent> _CurrentCardComponents = new List<CardComponent>();
        #endregion Private Variables

        #region Private Methods
        private Card SpawnCard(CardSettings.CardPreset preset, Transform parent)
        {
            var card = new Card(preset.Value, preset.Type);
            var cardComponent = UnityEngine.Object.Instantiate(preset.Prefab, parent).GetComponent<CardComponent>();
            cardComponent.Initialize(card);
            _CurrentCardComponents.Add(cardComponent);

            return card;
        }
        #endregion Private Methods
    }
}
