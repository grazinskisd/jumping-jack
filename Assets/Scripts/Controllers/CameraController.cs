using System.Collections;
using UnityEngine;

namespace JumpingJack
{
    public class CameraController: MonoBehaviour
    {
        public Camera Camera;
        public Player Player;
        public PlayerAnimationController AnimationController;
        public CameraSettings Settings;

        private void Start()
        {
            AnimationController.OnHeadBump += () => FlashBackground(Settings.BadJumpBackground);
            Player.OnHitHazard += (sender) => FlashBackground(Settings.HazardHitBackground);
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
    }
}