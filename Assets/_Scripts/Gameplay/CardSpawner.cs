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
                _AvailableCardList = new List<int>();
                _AvailableCardList.AddRange(_Settings.CardValueList);
            }

            if(count > _AvailableCardList.Count) { throw new System.ArgumentException("Trying to spawn " + count + " cards but there's only " + _AvailableCardList.Count + " cards available!"); }

            var result = new List<Card>();

            for(var i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, _AvailableCardList.Count);
                result.Add(SpawnCard(_AvailableCardList[randomIndex], parent));
                _AvailableCardList.RemoveAt(randomIndex);
            }

            return result;
        }
        #endregion Public Methods

        #region Private Variables
        [Inject] private readonly CardSettings _Settings;
        private List<int> _AvailableCardList;
        #endregion Private Variables

        #region Private Methods
        private Card SpawnCard(int value, Transform parent)
        {
            var card = new Card(value);
            var cardComponent = Object.Instantiate(_Settings.GetCardPrefab(value), parent).GetComponent<CardComponent>();
            cardComponent.Initialize(card);

            return card;
        }
        #endregion Private Methods
    }
}
