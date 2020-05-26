namespace cpp.Sen.Gameplay
{
    using Presets;
    using MEC;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public sealed class CardAnimationController
    {
        #region Public Methods
        public void Test()
        {
            Debug.Log("test");
        }

        public void MoveCard(CardComponent card, Vector3 targetPosition)
        {
            Timing.RunCoroutine(MoveCardCoroutine(card, targetPosition, _Settings.MovementSpeed));
        }

        public void FlipCard(CardComponent card)
        {
            Timing.RunCoroutine(FlipCardCoroutine(card, _Settings.FlipDuration));
        }

        public void MoveAndFlipCard(CardComponent card, Vector3 targetPosition)
        {
            Timing.RunCoroutine(MoveAndFlipCardCoroutine(card, targetPosition, _Settings.MovementSpeed, _Settings.FlipDuration));
        }
        #endregion Public Methods

        #region Private Variables
        [Inject] private readonly AnimationSettings _Settings;
        #endregion Private Variables

        #region Private Methods
        private IEnumerator<float> MoveCardCoroutine(CardComponent card, Vector3 targetPostion, float speed)
        {
            var time = 0f;
            var startPosition = card.transform.position;
            var duration = (targetPostion - startPosition).magnitude / speed;

            card.OnAnimationStarted();

            while (time <= duration)
            {
                var t = _Settings.MoveCurve.Evaluate(time / duration);
                card.transform.position = Vector3.Lerp(startPosition, targetPostion, t);
                time += Time.deltaTime;

                yield return Timing.WaitForOneFrame;
            }

            card.OnAnimationEnded();
        }

        private IEnumerator<float> FlipCardCoroutine(CardComponent card, float duration)
        {
            var time = 0f;
            var startRotation = card.transform.rotation;
            var targetRotation = Quaternion.LookRotation(card.transform.forward, -card.transform.up);

            card.OnAnimationStarted();

            while (time <= duration)
            {
                var t = _Settings.FlipCurve.Evaluate(time / duration);
                card.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                time += Time.deltaTime;

                yield return Timing.WaitForOneFrame;
            }

            card.OnAnimationEnded();
        }

        private IEnumerator<float> MoveAndFlipCardCoroutine(CardComponent card, Vector3 targetPosition, float movementSpeed, float flipDuration)
        {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(MoveCardCoroutine(card, targetPosition, movementSpeed)));
            Timing.RunCoroutine(FlipCardCoroutine(card, flipDuration));
        }
        #endregion Private Methods
    }
}
