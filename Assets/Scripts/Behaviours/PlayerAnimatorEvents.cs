using UnityEngine;

namespace JumpingJack
{
    public delegate void AnimationEventHandler();

    public class PlayerAnimatorEvents : MonoBehaviour
    {
        public event AnimationEventHandler OnHeadBump;
        public event AnimationEventHandler OnBadJumpEnd;
        public event AnimationEventHandler OnStandStraight;
        public event AnimationEventHandler OnStandSide;

        public void HeadBump()
        {
            IssueEvent(OnHeadBump);
        }

        public void BadJumpEnd()
        {
            IssueEvent(OnBadJumpEnd);
        }

        public void StandStraight()
        {
            IssueEvent(OnStandStraight);
        }

        public void StandSide()
        {
            IssueEvent(OnStandSide);
        }

        private void IssueEvent(AnimationEventHandler animationEvent)
        {
            if(animationEvent != null)
            {
                animationEvent();
            }
        }
    }
}
