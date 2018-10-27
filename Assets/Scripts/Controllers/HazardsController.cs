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

        protected override int GetStartHeight()
        {
            return Random.Range(1, Settings.Heights.Positions.Length-1);
        }

        protected override AutoMotion SpawnObject(int direction, int heightIndex, Vector2 position)
        {
            var obj = SpawnObject(GetRandomHazard(), direction, heightIndex, position);
            SetRandomColor(obj);
            return obj;
        }

        private void SetRandomColor(AutoMotion hazard)
        {
            var renderer = hazard.GetComponent<SpriteRenderer>();
            renderer.color = HazardSettings.Colors[Random.Range(0, HazardSettings.Colors.Length)];
        }

        private AutoMotion GetRandomHazard()
        {
            return HazardSettings.Hazards[Random.Range(0, HazardSettings.Hazards.Length)];
        }

        protected override void OnSpawnPointReached(AutoMotion sender)
        {
            if (sender.CurrentHeightIndex == Settings.Heights.Positions.Length - 2)
            {
                StartCoroutine(SpawnObjectDelayed(sender, Random.Range(MinDelayForSpawn, MaxDelayForSpawn)));
            }
            else
            {
                base.OnSpawnPointReached(sender);
            }
        }

        protected override int GetBoundedHeightIndex(int heightIndex)
        {
            return base.GetBoundedHeightIndex(heightIndex) % (Settings.Heights.Positions.Length-1);
        }

        private IEnumerator SpawnObjectDelayed(AutoMotion sender, float delayForSpawn)
        {
            int heightIndex = GetBoundedHeightIndex(sender.CurrentHeightIndex - sender.Direction);
            var obj = SpawnObject(sender, sender.Direction, heightIndex, GetNextSpawnPosition(sender));
            SetupNewObject(obj);
            obj.gameObject.SetActive(false);
            yield return new WaitForSeconds(delayForSpawn);
            obj.gameObject.SetActive(true);
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
