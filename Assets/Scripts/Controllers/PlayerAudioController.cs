using UnityEngine;

namespace JumpingJack
{
    public class PlayerAudioController: MonoBehaviour
    {
        public AudioSource AudioSource;
        public Player Player;
        public AudioSettings Settings;

        private AudioClip _lastClip;

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
            if (_lastClip == null || _lastClip != clip)
            {
                _lastClip = clip;
                AudioSource.clip = clip;
                AudioSource.loop = true;
                AudioSource.Play();
            }
        }

        private void Stop()
        {
            _lastClip = null;
            AudioSource.clip = null;
            AudioSource.Stop();
        }

        private void PlayShot(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }
    }
}
