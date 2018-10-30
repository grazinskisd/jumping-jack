using UnityEngine;

namespace JumpingJack
{
    public class PlayerAudioController: MonoBehaviour
    {
        public AudioSource AudioSource;
        public Player Player;
        public GameController GameController;
        public AudioSettings Settings;

        private AudioClip _lastClip;
        private bool _isPlayingFinal;

        private void Start()
        {
            AudioSource.volume = Settings.Volume;
            Player.OnEndCurrent += (sender) => Stop();
            Player.OnJump += (sender) => Play(Settings.Jump);
            Player.OnRunLeft += (sender) => Play(Settings.Walk);
            Player.OnRunRight += (sender) => Play(Settings.Walk);
            Player.OnBadJump += (sender) => Play(Settings.BadJump);
            Player.OnStun += (sender) => Play(Settings.Stun);
            Player.OnHitHazard += (sender) => PlayShot(Settings.Hazard);
            Player.OnFall += (sender) => Play(Settings.Fall);

            GameController.OnWin += () => PlayFinal(Settings.Win);
            GameController.OnLose += () => PlayFinal(Settings.Lose);
        }

        public void BadJumpEnd()
        {
            Play(Settings.Stun);
        }

        public void JumpFinished()
        {
            Stop();
        }

        public void OnStandStraight()
        {
            PlayShot(Settings.StandStraight);
        }

        public void OnStandSide()
        {
            PlayShot(Settings.StandSide);
        }

        private void Play(AudioClip clip)
        {
            if (!_isPlayingFinal)
            {
                if (_lastClip == null || _lastClip != clip)
                {
                    _lastClip = clip;
                    AudioSource.clip = clip;
                    AudioSource.loop = true;
                    AudioSource.Play();
                }
            }
        }

        private void PlayFinal(AudioClip clip)
        {
            _isPlayingFinal = true;
            AudioSource.Stop();
            AudioSource.clip = clip;
            AudioSource.loop = false;
            AudioSource.Play();
        }

        private void Stop()
        {
            if (!_isPlayingFinal)
            {
                _lastClip = null;
                AudioSource.clip = null;
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
