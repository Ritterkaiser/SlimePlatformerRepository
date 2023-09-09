using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Worm : Entity
    {
        private Animator animator;
        private Collider2D collider2D;
        private void Start()
        {
            animator = GetComponent<Animator>();
            collider2D = GetComponent<Collider2D>();
            _lives = 10;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_lives > 0 && collision.gameObject == Player.Instance.gameObject)
            {
                Player.Instance.GetDamage();
            }

            if (_lives < 1)
                Die();
        }
        public override void Die()
        {
            collider2D.isTrigger = true;
            animator.SetTrigger("death");
        }
        public void DestroyObject()
        {
            Destroy(this.gameObject);
        }
    }
}