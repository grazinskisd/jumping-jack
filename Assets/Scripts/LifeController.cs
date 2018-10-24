﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public delegate void LifeControllerEventHandler();

    public class LifeController: MonoBehaviour
    {
        public GameObject LifePrefab;
        public Player Player;

        public int StartLifeCount;
        public Vector2 StartPosition;
        public Vector2 LifePositionOffset;

        public event LifeControllerEventHandler OnZeroLives;

        private int _remainingLives;
        private List<GameObject> _lives;

        private void Start()
        {
            _lives = new List<GameObject>(StartLifeCount);
            _remainingLives = StartLifeCount;
            Player.OnGroundReached += DecrementLife;

            for (int i = 0; i < StartLifeCount; i++)
            {
                SpawnLife(StartPosition + i * LifePositionOffset);
            }
        }

        private void DecrementLife(Player sender)
        {
            var life = _lives[_lives.Count - 1];
            _lives.Remove(life);
            Destroy(life);
            _remainingLives--;
            if(_remainingLives == 0 && OnZeroLives != null)
            {
                OnZeroLives();
            }
        }

        private void SpawnLife(Vector2 position)
        {
            var life = Instantiate(LifePrefab);
            life.transform.SetParent(transform);
            life.transform.localPosition = position;
            _lives.Add(life);
        }
    }
}
