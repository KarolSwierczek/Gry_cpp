namespace cpp.Sen.Presets
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CardSpawnSettings", menuName = "Sen/CardSpawn", order = 4)]
    public sealed class CardSpawnSettings : ScriptableObject
    {
        #region Public Variables
        public float Width => _Width;
        public float Girth => _Girth;
        public float CardDelay => _CardSpawnDelay;
        public float StackDelay => _StackSpawnDelay;
        public float HandDelay => _HandSpawnDelay;
        public int CardsPerPlayer => _CardsPerPlayer;
        public int CardsTotal => _CardsTotal;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private float _Width;
        [SerializeField] private float _Girth;
        [SerializeField] private float _CardSpawnDelay;
        [SerializeField] private float _StackSpawnDelay;
        [SerializeField] private float _HandSpawnDelay;
        [SerializeField] private int _CardsPerPlayer;
        [SerializeField] private int _CardsTotal;
        #endregion Inspector Variables
    }
}