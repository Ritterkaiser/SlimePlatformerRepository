using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WalkingMonster : Entity
    {
        private float _speed = 3.5f;
        private Vector3 _direction;
        private SpriteRenderer _sprite;
        private Collider2D collider2D;
        private Animator animator;
        public LayerMask Level;

        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
        }
        private void Start()
        {
            animator = GetComponent<Animator>();
            collider2D = GetComponent<Collider2D>();
            _lives = 1;
            _direction = transform.right;
        }
        private void Update()
        {
            Move();
        }
        private void Move()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * _direction.x * 0.55f, 0.03f, Level);

            if (colliders.Length > 0) 
                _direction *= -1f;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, Time.deltaTime);
            _sprite.flipX = _direction.x > 0.0f;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_lives > 0 && collision.gameObject == Player.Instance.gameObject)
            {
                Player.Instance.GetDamage();
                _lives--;
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