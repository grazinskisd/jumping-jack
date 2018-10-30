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
        private List<Pair> _positions;

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
            float maxRightPosition = Settings.ObjectSettings.RightEnd - Settings.ObjectSettings.SpawnDistance;
            float increment = (2 * maxRightPosition) / HazardSettings.PositionsPerPlatform;
            int heights = Settings.Heights.Positions.Length - 1;
            _positions = new List<Pair>();

            for (int i = 0; i <= heights; i++)
            {
                for (int j = 1; j <= HazardSettings.PositionsPerPlatform; j++)
                {
                    _positions.Add(new Pair()
                    {
                        Height = i,
                        Position = Vector2.right * (-maxRightPosition + increment * j)
                    });
                }
            }

            Shuffle(_positions);
            _positions = _positions.GetRange(0, GetNumberOfObjectsOnStart());
        }

        public void Shuffle<T>(IList<T> list)
        {
            System.Random rnd = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        protected override AutoMotion SpawnRandomObject()
        {
            var pair = _positions[_currentIndex];
            _currentIndex++;
            return SpawnObject(_direction, pair.Height, pair.Position);
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

        protected override Vector2 GetRandomPosition()
        {
            return base.GetRandomPosition() + HazardSettings.PositionOffset;
        }

        protected override Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return base.GetNextSpawnPosition(sender) + HazardSettings.PositionOffset;
        }

        private class Pair
        {
            public int Height;
            public Vector2 Position;
        }
    }
}
