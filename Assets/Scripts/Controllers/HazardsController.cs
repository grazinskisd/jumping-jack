using System.Collections;
using UnityEngine;

namespace JumpingJack
{
    public class HazardsController: AutoMotionController
    {
        public HazardsSettings HazardSettings;
        public int HazardMovingDirection;
        public Vector2 HazardPositionOffset;
        [Tooltip("Delay for spawning hazard on the bottom level")]
        public float MinDelayForSpawn;
        public float MaxDelayForSpawn;

        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        protected override void Start()
        {
            _direction = HazardMovingDirection;
            base.Start();
        }

        protected override AutoMotion SpawnObject(int direction, int heightIndex, Vector2 position)
        {
            var hazard = InstantiateRandomHazard();
            SetRandomColor(hazard);
            hazard.Direction = direction;
            hazard.Settings = Settings.ObjectSettings;
            hazard.CurrentHeightIndex = heightIndex;
            hazard.transform.SetParent(transform);
            hazard.transform.position = position + Vector2.up * Settings.Heights.Positions[heightIndex].y;
            return hazard;
        }

        private void SetRandomColor(AutoMotion hazard)
        {
            var renderer = hazard.GetComponent<SpriteRenderer>();
            renderer.color = HazardSettings.Colors[Random.Range(0, HazardSettings.Colors.Length)];
        }

        private AutoMotion InstantiateRandomHazard()
        {
            return Instantiate(HazardSettings.Hazards[Random.Range(0, HazardSettings.Hazards.Length)]);
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
            return _prefService.GetInt(Prefs.Hazards);
        }

        protected override Vector2 GetRandomPosition()
        {
            return base.GetRandomPosition();
        }

        protected override Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return base.GetNextSpawnPosition(sender) + HazardPositionOffset;
        }
    }
}
