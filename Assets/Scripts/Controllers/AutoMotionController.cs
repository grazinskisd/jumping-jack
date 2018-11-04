using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class AutoMotionController: MonoBehaviour
    {
        public AutoMotion ObjectPrefab;
        public Player Player;
        public PlayerAnimatorEvents AnimatorEvents;
        public AutoMotionControllerSettings Settings;

        protected List<AutoMotion> _objects;
        protected int _direction;
        protected int _objectCount;

        private float _timeScale;

        protected virtual void Start()
        {
            _objects = new List<AutoMotion>();
            _objectCount = GetNumberOfObjectsOnStart();
            SetupTimeScaleEvents();
            for (int i = 0; i < _objectCount; i++)
            {
                SpawnStartObject();
            }
        }

        private void SetupTimeScaleEvents()
        {
            ResetTimeScale();
            Player.OnJump += SlowDown;
            Player.OnFall += SlowDown;
            Player.OnBadJump += SlowDown;
            AnimatorEvents.OnFallEnd += ResetTimeScale;
            AnimatorEvents.OnJumpEnd += ResetTimeScale;
            AnimatorEvents.OnBadJumpEnd += ResetTimeScale;
        }

        private void ResetTimeScale()
        {
            _timeScale = Settings.DefaultTimeScale;
            SetTimeScaleForCurrentObjects();
        }

        private void SlowDown(Player sender)
        {
            _timeScale = Settings.SlowedTimeScale;
            SetTimeScaleForCurrentObjects();
        }

        private void SetTimeScaleForCurrentObjects()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].TimeScale = _timeScale;
            }
        }

        protected virtual void SpawnStartObject()
        {
            AutoMotion newObject = SpawnRandomObject();
            SetupNewObject(newObject);
            UpdateDirection();
        }

        protected virtual AutoMotion SpawnRandomObject()
        {
            return SpawnObject(_direction, GetStartHeight(), GetRandomPosition());
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
            newObject.OnDestroyReached += RemoveObject;
            newObject.OnSpawnPointReached += OnSpawnPointReached;
        }

        protected virtual void OnSpawnPointReached(AutoMotion sender)
        {
            int heightIndex = GetBoundedHeightIndex(sender.CurrentHeightIndex - sender.Direction);
            var obj = SpawnObject(sender, sender.Direction, heightIndex, GetNextSpawnPosition(sender));
            SetupNewObject(obj);
        }

        protected virtual Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return Vector2.right * Settings.ObjectSettings.RightEnd * -1 * sender.Direction;
        }

        private void RemoveObject(AutoMotion sender)
        {
            sender.OnDestroyReached -= RemoveObject;
            sender.OnSpawnPointReached -= OnSpawnPointReached;
            _objects.Remove(sender);
        }

        protected virtual int GetBoundedHeightIndex(int heightIndex)
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

        protected virtual AutoMotion SpawnObject(int direction, int heightIndex, Vector2 position)
        {
            return SpawnObject(ObjectPrefab, direction, heightIndex, position);
        }

        protected virtual AutoMotion SpawnObject(AutoMotion prototype, int direction, int heightIndex, Vector2 position)
        {
            var obj = Instantiate(prototype);
            obj.TimeScale = _timeScale;
            obj.gameObject.name = prototype.gameObject.name;
            obj.Direction = direction;
            obj.Settings = Settings.ObjectSettings;
            obj.CurrentHeightIndex = heightIndex;
            obj.transform.SetParent(transform);
            obj.transform.position = position + Vector2.up * Settings.Heights.Positions[heightIndex].y;
            return obj;
        }
    }
}
