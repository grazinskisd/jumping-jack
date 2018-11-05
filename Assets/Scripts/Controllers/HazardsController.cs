using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class HazardsController: AutoMotionController
    {
        public HazardsSettings HazardSettings;

        private PlayerPrefsService _prefService;

        private int _currentIndex;
        private List<Pair<int, Vector2>> _positions;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        protected override void Start()
        {
            _direction = HazardSettings.MovingDirection;
            PopulateRandomPositionsList();
            base.Start();
        }

        private void PopulateRandomPositionsList()
        {
            _positions = GetAllPossiblePositionsForHazards();
            ListUtils.Shuffle(_positions);
            _positions = _positions.GetRange(0, GetNumberOfObjectsOnStart());
        }

        private List<Pair<int, Vector2>> GetAllPossiblePositionsForHazards()
        {
            float maxRightPosition = Settings.ObjectSettings.RightEnd - Settings.ObjectSettings.SpawnDistance;
            float increment = (2 * maxRightPosition) / HazardSettings.PositionsPerPlatform;
            int heights = Settings.Heights.Positions.Length - 1;
            var list = new List<Pair<int, Vector2>>();

            for (int i = 0; i <= heights; i++)
            {
                for (int j = 1; j <= HazardSettings.PositionsPerPlatform; j++)
                {
                    list.Add(new Pair<int, Vector2>()
                    {
                        First = i,
                        Second = Vector2.right * (-maxRightPosition + increment * j)
                    });
                }
            }
            return list;
        }

        protected override AutoMotion SpawnRandomObject()
        {
            var pair = _positions[_currentIndex];
            _currentIndex++;
            return SpawnObject(_direction, pair.First, pair.Second + HazardSettings.PositionOffset);
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
            var renderer = hazard.GetComponentInChildren<SpriteRenderer>();
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
                StartCoroutine(SpawnObjectDelayed(sender, HazardSettings.DelayForSpawn));
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

        protected override Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return base.GetNextSpawnPosition(sender) + HazardSettings.PositionOffset;
        }
    }
}
