using UnityEngine;

namespace JumpingJack
{
    public class PlayerAudioController: MonoBehaviour
    {
        public AudioSource AudioSource;
        public Player Player;
        public PlayerAnimatorEvents AnimationEvents;
        public GameController GameController;
        public AudioSettings Settings;

        private bool _isPlayingFinal;

        private void Start()
        {
            Player.OnStand += (sender) => Stop();
            Player.OnJump += (sender) => Play(Settings.Jump);
            Player.OnMove += (sender) => Play(Settings.Walk);
            Player.OnBadJump += (sender) => Play(Settings.BadJump);
            Player.OnStun += (sender) => Play(Settings.Stun);
            Player.OnHitHazard += (sender) => PlayShot(Settings.Hazard);
            Player.OnFall += (sender) => Play(Settings.Fall);

            AnimationEvents.OnStandStraight += () => PlayShot(Settings.StandStraight);
            AnimationEvents.OnStandSide += () => PlayShot(Settings.StandSide);

            GameController.OnWin += () => PlayFinal(Settings.Win);
            GameController.OnLose += () => PlayFinal(Settings.Lose);
        }

        private void Play(AudioClip clip)
        {
            if (!_isPlayingFinal)
            {
                AudioSource.clip = clip;
                AudioSource.loop = true;
                AudioSource.Play();
            }
        }

        private void PlayFinal(AudioClip clip)
        {
            AudioSource.Stop();
            AudioSource.clip = clip;
            AudioSource.loop = false;
            AudioSource.Play();
            _isPlayingFinal = true;
        }

        private void Stop()
        {
            if (!_isPlayingFinal)
            {
                AudioSource.Stop();
            }
        }

        private void PlayShot(AudioClip clip)
        {
            if (!_isPlayingFinal)
            {
                AudioSource.PlayOneShot(clip);
            }
        }
    }
}
