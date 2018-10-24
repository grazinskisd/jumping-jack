﻿using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/HoleSettings")]
    public class HoleSettings : ScriptableObject
    {
        public int NumberOfHolesAtStart;
        public int MaxHoleCount;
        public PositionsScriptable Heights;
        public float LeftEnd;
        public float RightEnd;
        [Tooltip("Distance before end to spawn the next hole")]
        public float SpawnDistance;
        public float MoveSpeed;
    }
}
