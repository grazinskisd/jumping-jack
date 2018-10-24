using UnityEngine;

namespace JumpingJack
{
    public delegate void HoleEventHandler(Hole sender);

    public class Hole: MonoBehaviour
    {
        public HoleSettings Settings;
        [Tooltip("1: Right; -1: Left")]
        public int Direction;
        public int CurrentHeightIndex;

        public event HoleEventHandler OnSpawnPointReached;
        public event HoleEventHandler OnDestroyReached;

        private bool _isNextSpawned;

        private void Start()
        {
            _isNextSpawned = false;
        }

        private void Update()
        {
            transform.Translate(Vector3.right * Direction * Settings.MoveSpeed * Time.deltaTime);

            if(!_isNextSpawned && transform.position.x * Direction >= Settings.RightEnd - Settings.SpawnDistance)
            {
                _isNextSpawned = true;
                IssueEvent(OnSpawnPointReached);
            }

            if(transform.position.x * Direction >= Settings.RightEnd)
            {
                IssueEvent(OnDestroyReached);
                Destroy(gameObject);
            }
        }

        private void IssueEvent(HoleEventHandler eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue(this);
            }
        }
    }
}
