using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class HolesController: MonoBehaviour
    {
        public Hole HolePrefab;
        public Player Player;
        public HoleSettings Settings;

        private List<Hole> _holes = new List<Hole>();
        private int _direction = 1;

        private void Start()
        {
            for (int i = 0; i < Settings.NumberOfHolesAtStart; i++)
            {
                Hole newHole = SpawnHole(_direction, Settings.StartHoleHeightIndex, 0);
                SetupNewHole(newHole);
                UpdateDirection();
            }

            Player.OnJump += SpawnRandomHole;
        }

        private void SpawnRandomHole(Player sender)
        {
            int holeIndex = Random.Range(0, Settings.Heights.Positions.Length);
            SetupNewHole(SpawnHole(_direction, holeIndex, GetRandomHolePosition()));
            UpdateDirection();
        }

        private float GetRandomHolePosition()
        {
            return Random.Range(-1, 1) * (Settings.RightEnd - Settings.SpawnDistance);
        }

        private void SetupNewHole(Hole newHole)
        {
            _holes.Add(newHole);
            newHole.OnSpawnPointReached += SpawnNext;
            newHole.OnDestroyReached += RemoveHole;
        }

        private void RemoveHole(Hole sender)
        {
            sender.OnDestroyReached -= RemoveHole;
            sender.OnSpawnPointReached -= SpawnNext;
            _holes.Remove(sender);
        }

        private void UpdateDirection()
        {
            _direction *= -1;
        }

        private void SpawnNext(Hole sender)
        {
            int heightIndex = GetBoundedHeightIndex(sender.CurrentHeightIndex - sender.Direction);
            float xPosition = Settings.LeftEnd * sender.Direction;
            var hole = SpawnHole(sender.Direction, heightIndex, xPosition);
            SetupNewHole(hole);
        }

        private int GetBoundedHeightIndex(int heightIndex)
        {
            if (heightIndex < 0)
            {
                heightIndex = Settings.Heights.Positions.Length - 1;
            }
            else if (heightIndex >= Settings.Heights.Positions.Length)
            {
                heightIndex = 0;
            }
            return heightIndex;
        }

        private Hole SpawnHole(int direction, int heightIndex, float xPosition)
        {
            var hole = Instantiate(HolePrefab);
            hole.transform.SetParent(transform);
            hole.Direction = direction;
            hole.Settings = Settings;
            hole.CurrentHeightIndex = heightIndex;
            hole.transform.position = new Vector3(xPosition, Settings.Heights.Positions[heightIndex].y, 0);
            return hole;
        }
    }
}
