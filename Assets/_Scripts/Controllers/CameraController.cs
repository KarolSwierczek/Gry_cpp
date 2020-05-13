namespace cpp.Sen.Gameplay
{
    using Sirenix.OdinInspector;
    using Zenject;
    using UnityEngine;
    using MEC;
    using System.Collections.Generic;

    public sealed class CameraController : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")] private Transform _MenuTransform;
        [SerializeField, FoldoutGroup("References")] private Transform _GameTransform;

        [SerializeField, FoldoutGroup("Settings")] private float _CameraSpeed;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnEnable()
        {
            SnapToTransform(_MenuTransform);
            _GameModeController.OnGameModeChanged += OnGameModeChanged;
        }
        private void OnDisable()
        {
            _GameModeController.OnGameModeChanged -= OnGameModeChanged;
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private GameModeController _GameModeController;
        #endregion Private Variables

        #region Private Methods
        private void SnapToTransform(Transform target)
        {
            transform.SetPositionAndRotation(target.position, target.rotation);
        }

        private IEnumerator<float> AnimateToTransform(Transform target, float speed)
        {
            var t = 0f;
            var startPosition = transform.position;
            var startRotation = transform.rotation;
            var targetPosition = target.position;
            var targetRotation = target.rotation;

            var distance = (targetPosition - startPosition).magnitude;
            var duration = distance / speed;

            while(t <= duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, t/duration);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t/duration);

                yield return Timing.WaitForOneFrame;

                t += Time.deltaTime;
            }
        }

        private void OnGameModeChanged(object sender, GameModeController.OnGameModeChangedArgs args)
        {
            Timing.RunCoroutine(AnimateToTransform(args.Mode == GameModeController.GameMode.Menu ? _MenuTransform : _GameTransform, _CameraSpeed));
        }
        #endregion Private Methods
    }
}
