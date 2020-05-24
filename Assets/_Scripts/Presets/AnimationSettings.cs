namespace cpp.Sen.Presets
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AnimationSettings", menuName = "Sen/AnimationSettings", order = 1)]
    public sealed class AnimationSettings : ScriptableObject
    {
        #region Public Variables
        public AnimationCurve MoveCurve => _MoveCurve;
        public AnimationCurve FlipCurve => _FlipCurve;
        public float MovementSpeed => _MovementSpeed;
        public float FlipDuration => _FlipDuration;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private AnimationCurve _MoveCurve;
        [SerializeField] private AnimationCurve _FlipCurve;

        [SerializeField] private float _MovementSpeed;
        [SerializeField] private float _FlipDuration;
        #endregion Inspector Variables
    }
}