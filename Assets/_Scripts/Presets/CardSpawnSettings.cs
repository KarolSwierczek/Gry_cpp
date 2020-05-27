namespace cpp.Sen.Presets
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CardSpawnSettings", menuName = "Sen/CardSpawnSettings", order = 3)]
    public sealed class CardSpawnSettings : ScriptableObject
    {


        #region Public Variables
        public float Width => _Width;
        public float Girth => _Girth;
        public float Delay => _GroupSpawnDelay;
        public float CollectionDelay => _CollectionSpawnDelay;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private float _Width;
        [SerializeField] private float _Girth;
        [SerializeField] private float _GroupSpawnDelay;
        [SerializeField] private float _CollectionSpawnDelay;
        #endregion Inspector Variables
    }
}