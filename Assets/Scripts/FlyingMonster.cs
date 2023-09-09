using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Platformer
{
    public class FlyingMonster : Entity
    {
        private SpriteRenderer sprite;
        [SerializeField] private AIPath aiPath;

        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            _lives = 1;
        }
        private void Update()
        {
            sprite.flipX = aiPath.desiredVelocity.x <= 0.01f;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == Player.Instance.gameObject)
            {
                Player.Instance.GetDamage();
            }
        }
    }
}