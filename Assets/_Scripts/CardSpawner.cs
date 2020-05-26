namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Presets;
    using MEC;
    using System.Collections.Generic;

    public class CardSpawner : MonoBehaviour
    {
        [SerializeField] private CardSettings _Settings;

        private void Start()
        {
            Timing.RunCoroutine(SpawnStackCoroutine());
        }

        private Card SpawnCard(int value)
        {
            var card = new Card(value);
            var cardComponent = Instantiate(_Settings.GetCardPrefab(value), transform).GetComponent<CardComponent>();
            cardComponent.Initialize(card);

            return card;
        }

        private IEnumerator<float> SpawnStackCoroutine()
        {
            //todo
            yield return Timing.WaitForSeconds(0.5f);
            for (var i = 0; i < 10; i++)
            {
                var card = SpawnCard(i);
                card.MoveCard(new Vector3(0f, 1f + i / 50f, 0f));
                yield return Timing.WaitForSeconds(0.2f);
            }
        }
    }
}
