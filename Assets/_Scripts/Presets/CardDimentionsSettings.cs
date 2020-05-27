namespace cpp.Sen.Presets
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CardDimentionsSettings", menuName = "Sen/CardDimentionsSettings", order = 3)]
    public sealed class CardDimentionsSettings : ScriptableObject
    {


        #region Public Variables
        public float Width => _Width;
        public float Girth => _Girth;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private float _Width;
        [SerializeField] private float _Girth;
        #endregion Inspector Variables
    }
}