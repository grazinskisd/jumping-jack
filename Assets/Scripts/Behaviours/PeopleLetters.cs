using UnityEngine;

namespace JumpingJack
{
    public delegate void PeopleLettersEventHandler();
    public class PeopleLetters: MonoBehaviour
    {
        public event PeopleLettersEventHandler OnLettersRaising;
        public event PeopleLettersEventHandler OnAllRaised;

        public void LettersRaising()
        {
            IssueEvent(OnLettersRaising);
        }

        public void AllRaised()
        {
            IssueEvent(OnAllRaised);
        }

        private void IssueEvent(PeopleLettersEventHandler peopleEvent)
        {
            if(peopleEvent != null)
            {
                peopleEvent();
            }
        }
    }
}
