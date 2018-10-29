using UnityEngine;

namespace JumpingJack
{
    public class PlayerAnimationController: MonoBehaviour
    {
        public Animator Animator;
        public Player Player;

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
        }

        private void Trigger(Triggers trigger)
        {

            if (_lastTrigger != trigger)
            {
                Debug.Log(trigger.ToString());
                _lastTrigger = trigger;
                Animator.SetTrigger(trigger.ToString());
            }
        }

        public void BadJumpEnd()
        {
            Trigger(Triggers.Stun);
        }
    }

    public enum Triggers
    {
        Jump,
        Stand,
        BadJump,
        RunLeft,
        RunRight,
        Stun
    }
}
