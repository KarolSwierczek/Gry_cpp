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
        public void AllignCard(CardComponent card, Vector3 forwardDirection)
        {
            card.transform.rotation = Quaternion.LookRotation(forwardDirection, card.transform.up);
        }

        public void MoveCard(CardComponent card, Vector3 targetPosition, Vector3 forwardDirection)
        {
            Timing.RunCoroutine(MoveCardCoroutine(card, targetPosition, forwardDirection, _Settings.MovementSpeed));
        }

        public void FlipCard(CardComponent card)
        {
            Timing.RunCoroutine(FlipCardCoroutine(card, _Settings.FlipDuration));
        }

        public void MoveAndFlipCard(CardComponent card, Vector3 targetPosition, Vector3 forwardDirection)
        {
            Timing.RunCoroutine(MoveAndFlipCardCoroutine(card, targetPosition, forwardDirection, _Settings.MovementSpeed, _Settings.FlipDuration));
        }
        #endregion Public Methods

        #region Private Variables
        [Inject] private readonly AnimationSettings _Settings;
        #endregion Private Variables

        #region Private Methods
        private IEnumerator<float> MoveCardCoroutine(CardComponent card, Vector3 targetPostion, Vector3 forwardDirection, float speed)
        {
            var time = 0f;
            var startRotation = card.transform.rotation;
            var startPosition = card.transform.position;

            var duration = (targetPostion - startPosition).magnitude / speed;
            var targetRotation = Quaternion.LookRotation(forwardDirection, card.transform.up);

            card.OnAnimationStarted();

            while (time <= duration)
            {
                var t = _Settings.MoveCurve.Evaluate(time / duration);

                card.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                card.transform.position = Vector3.Lerp(startPosition, targetPostion, t);

                time += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }

            card.transform.rotation = targetRotation;
            card.transform.position = targetPostion;

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

            card.transform.rotation = targetRotation;

            card.OnAnimationEnded();
        }

        private IEnumerator<float> MoveAndFlipCardCoroutine(CardComponent card, Vector3 targetPosition, Vector3 forwardDirection, float movementSpeed, float flipDuration)
        {
            Timing.RunCoroutine(MoveCardCoroutine(card, targetPosition, forwardDirection, movementSpeed));
            yield return Timing.WaitForSeconds(1f); //todo: rethink this animation and move delay to settings
            Timing.RunCoroutine(FlipCardCoroutine(card, flipDuration));
        }
        #endregion Private Methods
    }
}
