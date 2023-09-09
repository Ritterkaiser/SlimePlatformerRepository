using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
    {
    public class Wizard : Entity
    {
        public Animator animator;
        private Collider2D collider2D;
        [SerializeField] private GameObject _bolt;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private float _startTimeBetweenShots;
        float _timeBetweenShots;
        private bool _canShoot;
        public bool inRange;

        private void Start()
        {
            _lives = 5;
            animator = GetComponent<Animator>();
            collider2D = GetComponent<Collider2D>();
            _timeBetweenShots = _startTimeBetweenShots;  
        }

        private void Update()
        {
            if (inRange && _timeBetweenShots < 0)
            {
                _canShoot = true;
                _timeBetweenShots = _startTimeBetweenShots;
            }

            else
            {
                _timeBetweenShots -= Time.deltaTime;
                _canShoot = false;
            }

            if (_canShoot)
            {
                Instantiate(_bolt, _shotPoint.position, _shotPoint.rotation);
                animator.SetBool("canAttack", true);
            }
            else
            {
                animator.SetBool("canAttack", false);
            }

        }
        public override void Die()
        {
            collider2D.isTrigger = true;
            animator.SetTrigger("death");
            Destroy(this.gameObject, 10f);
            _timeBetweenShots = 100f;
        }

    }
}
