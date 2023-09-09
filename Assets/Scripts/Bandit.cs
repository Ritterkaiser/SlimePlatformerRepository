using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Bandit : Entity
    {
        public Animator animator;
        private Collider2D collider2D;


        public float attackDistance; //дальность на которой моб начинает атаку
        public float moveSpeed;
        public float timer;

        public Transform target;
        private Animator anim;
        private float distance;
        private bool attackMode;
        public bool inRange;
        private bool cooling;
        private float intTimer;
        public GameObject hotZone;
        public GameObject triggerArea;

        public Transform leftLimit;
        public Transform rightLimit;
        [SerializeField] private Transform hitBox;

        public LayerMask player;

        private void Awake()
        {
            _lives = 10;
            intTimer = timer;
            anim = GetComponent<Animator>();
            collider2D = GetComponent<Collider2D>();
            SelectTarget();
        }

        private void Update()
        {
            if (!attackMode)
                Move();

            if (inRange)
            {
                EnemyLogic();
            }
            if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("LightBandit_Attack"))
            {
                SelectTarget();
            }
        }
        void EnemyLogic()
        {
            distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance > attackDistance)
            {
                StopAttack();
            }
            else if (attackDistance >= distance && cooling == false)
            {
                Attack();
            }

            if (cooling)
            {
                Cooldown();
                anim.SetBool("Attack", false);
            }
        }

        void Move()
        {
            anim.SetBool("canWalk", true);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("LightBandit_Attack"))
            {
                Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }

        void Attack()
        {
            timer = intTimer;
            attackMode = true;

            anim.SetBool("canWalk", false);
            anim.SetBool("Attack", true);
        }
        private void OnAttack()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(hitBox.position, 0.8f, player);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<Entity>().GetDamage();
            }
        }

        void Cooldown()
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && cooling && attackMode)
            {
                cooling = false;
                timer = intTimer;
            }
        }

        void StopAttack()
        {
            cooling = false;
            attackMode = false;
            anim.SetBool("Attack", false);
        }
        public void TriggerCooling()
        {
            cooling = true;
        }
        private bool InsideOfLimits()
        {
            return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
        }


        public void SelectTarget()
        {
            float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
            float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);

            if (distanceToLeft > distanceToRight)
            {
                target = leftLimit;
            }
            else
            {
                target = rightLimit;
            }


            Flip();
        }
        public void Flip()
        {
            Vector3 rotation = transform.eulerAngles;
            if (target.position.x > transform.position.x)
            {
                rotation.y = 180;
            }
            else
            {
                Debug.Log("Twist");
                rotation.y = 0;
            }


            transform.eulerAngles = rotation;
        }


        public override void Die()
        {
            collider2D.isTrigger = true;
            animator.SetTrigger("death");
            anim.SetBool("canWalk", false);
            anim.SetBool("Attack", false);
        }
    }
}