using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace JumpingJack
{
    public class CameraController: MonoBehaviour
    {
        public Camera Camera;
        public Player Player;
        public PlayerAnimatorEvents AnimationEvents;
        public CameraSettings Settings;
        private Vector3 _cameraPosition;

        private void Start()
        {
            _cameraPosition = Camera.transform.position;
            AnimationEvents.OnHeadBump += () => FlashBackground(Settings.BadJumpBackground);
            Player.OnHitHazard += (sender) => FlashBackground(Settings.HazardHitBackground);
            Player.OnStun += (sender) => CameraShake();
        }

        private void FlashBackgroundWithHazardColor(AutoMotion hazard)
        {
            FlashBackground(hazard.GetComponent<SpriteRenderer>().color);
        }

        private void FlashBackground(Color color)
        {
            StartCoroutine(TimedBackgroundSwitch(color, Settings.FlashDuration));
        }

        private IEnumerator TimedBackgroundSwitch(Color color, float duration)
        {
            ChangeBackgroundColor(color);
            yield return new WaitForSeconds(duration);
            ChangeBackgroundColor(Settings.DefaultBackground);
        }

        private void ChangeBackgroundColor(Color color)
        {
            Camera.backgroundColor = color;
        }

        private void CameraShake()
        {
            Camera.DOShakePosition(Settings.ShakeDuration, Settings.ShakeStrength, Settings.ShakeVibrato)
                .OnComplete(() => Camera.transform.position = _cameraPosition);
        }
    }
}