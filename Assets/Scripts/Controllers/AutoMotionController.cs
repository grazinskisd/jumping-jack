using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class AutoMotionController: MonoBehaviour
    {
        public AutoMotion ObjectPrefab;
        public AutoMotionControllerSettings Settings;

        protected List<AutoMotion> _objects;
        protected int _direction;
        protected int _objectCount;

        protected virtual void Start()
        {
            _objects = new List<AutoMotion>();
            _objectCount = GetNumberOfObjectsOnStart();
            for (int i = 0; i < _objectCount; i++)
            {
                AutoMotion newObject = SpawnObject(_direction, GetStartHeight(), GetRandomPosition());
                SetupNewObject(newObject);
                UpdateDirection();
            }
        }

        protected virtual int GetStartHeight()
        {
            return Random.Range(1, Settings.Heights.Positions.Length);
        }

        protected virtual int GetNumberOfObjectsOnStart()
        {
            return Settings.NumberOfObjectsOnStart;
        }

        protected virtual void UpdateDirection()
        {

        }

        protected virtual Vector2 GetRandomPosition()
        {
            return Vector2.right * GetRandomPositionOnPlane();
        }

        protected float GetRandomPositionOnPlane()
        {
            return Random.Range(-1.0f, 1.0f) * (Settings.ObjectSettings.RightEnd - Settings.ObjectSettings.SpawnDistance);
        }

        protected void SetupNewObject(AutoMotion newObject)
        {
            _objects.Add(newObject);
            newObject.OnSpawnPointReached += SpawnNext;
            newObject.OnDestroyReached += RemoveHole;
        }

        private void SpawnNext(AutoMotion sender)
        {
            int heightIndex = GetBoundedHeightIndex(sender.CurrentHeightIndex - sender.Direction);
            var hole = SpawnObject(sender.Direction, heightIndex, GetNextSpawnPosition(sender));
            SetupNewObject(hole);
        }

        protected virtual Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return Vector2.right * Settings.ObjectSettings.RightEnd * -1 * sender.Direction;
        }

        private void RemoveHole(AutoMotion sender)
        {
            sender.OnDestroyReached -= RemoveHole;
            sender.OnSpawnPointReached -= SpawnNext;
            _objects.Remove(sender);
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

        protected AutoMotion SpawnObject(int direction, int heightIndex, Vector2 position)
        {
            var hole = Instantiate(ObjectPrefab);
            hole.Direction = direction;
            hole.Settings = Settings.ObjectSettings;
            hole.CurrentHeightIndex = heightIndex;
            hole.transform.SetParent(transform);
            hole.transform.position = position + Vector2.up * Settings.Heights.Positions[heightIndex].y;
            return hole;
        }
    }
}
