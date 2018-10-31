using UnityEngine;

namespace JumpingJack
{
    public delegate void AnimationEventHandler();

    public class PlayerAnimationController: MonoBehaviour
    {
        public Animator Animator;
        public Player Player;

        public event AnimationEventHandler OnHeadBump;

        private Triggers _lastTrigger;

        private void Start()
        {
            _lastTrigger = Triggers.Stand;
            Trigger(Triggers.Stand);
            Player.OnJump += (sender) => Trigger(Triggers.Jump);
            Player.OnEndCurrent += (sender) => Trigger(Triggers.Stand);
            Player.OnRunLeft += (sender) => Trigger(Triggers.RunLeft);
            Player.OnRunRight += (sender) => Trigger(Triggers.RunRight);
            Player.OnBadJump += (sender) => Trigger(Triggers.BadJump);
            Player.OnStun += (sender) => Trigger(Triggers.Stun);
            Player.OnFall += (sender) => Trigger(Triggers.Fall);
        }

        private void Trigger(Triggers trigger)
        {
            if (_lastTrigger != trigger)
            {
                _lastTrigger = trigger;
                Animator.SetTrigger(trigger.ToString());
            }
        }

        public void BadJumpEnd()
        {
            Trigger(Triggers.Stun);
        }

        public void JumpFinished()
        {
            Trigger(Triggers.Stand);
        }

        public void HeadBump()
        {
            IssueEvent(OnHeadBump);
        }

        public void IssueEvent(AnimationEventHandler animationEvent)
        {
            if(animationEvent != null)
            {
                animationEvent();
            }
        }
    }

    public enum Triggers
    {
        Jump,
        Stand,
        BadJump,
        RunLeft,
        RunRight,
        Stun,
        Fall
    }
}
