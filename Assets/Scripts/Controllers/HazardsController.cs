using System.Collections;
using UnityEngine;

namespace JumpingJack
{
    public class HazardsController: AutoMotionController
    {
        public int HazardMovingDirection;
        public Vector2 HazardPositionOffset;
        [Tooltip("Delay for spawning hazard on the bottom level")]
        public float MinDelayForSpawn;
        public float MaxDelayForSpawn;

        protected override void Start()
        {
            _direction = HazardMovingDirection;
            base.Start();
        }

        protected override void OnSpawnPointReached(AutoMotion sender)
        {
            if (sender.CurrentHeightIndex == Settings.Heights.Positions.Length - 1)
            {
                StartCoroutine(SpawnObjectDelayed(sender, Random.Range(MinDelayForSpawn, MaxDelayForSpawn)));
            }
            else
            {
                base.OnSpawnPointReached(sender);
            }
        }

        private IEnumerator SpawnObjectDelayed(AutoMotion sender, float delayForSpawn)
        {
            yield return new WaitForSeconds(delayForSpawn);
            base.OnSpawnPointReached(sender);
        }

        protected override int GetNumberOfObjectsOnStart()
        {
            return PlayerPrefsService.GetInt(Prefs.Hazards);
        }

        protected override Vector2 GetRandomPosition()
        {
            return base.GetRandomPosition() + HazardPositionOffset;
        }

        protected override Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return base.GetNextSpawnPosition(sender) + HazardPositionOffset;
        }
    }
}
