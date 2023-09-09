using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class Player : Entity
    {
        public Text HealthText;
        public Text GemText;

        [SerializeField] private int _gems = 0;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private int _lives = 75;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private int _jumpTimes = 0;

        [SerializeField] private Transform _firePointRight;
        [SerializeField] private Transform _firePointLeft;
        [SerializeField] private GameObject _PlayerBolt;
        [SerializeField] private float _startTimeBetweenShots;
        float _timeBetweenShots;
        private bool _canShoot;

        private bool _isGrounded = false;
        public bool _isFliped = false;
        public bool _isAttacking = false;
        public bool _isRecharged = true;
        public bool _isBlocking = false;
        public bool _isJumpAttacking = false;

        public Transform attackPosition;
        public float attackRange;
        public LayerMask enemy;
        public LayerMask Level;

        [SerializeField] private AudioSource _jumpSound;
        [SerializeField] private AudioSource _attackSound;
        [SerializeField] private AudioSource _keySound;
        [SerializeField] private AudioSource _gemSound;
        [SerializeField] private AudioSource _chestSound;

        [SerializeField] private GameObject _losePanel;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private SpriteRenderer sprite;

        public bool _greenKeyCollected = false;
        public bool _yellowKeyCollected = false;
        public bool _blackKeyCollected = false;
        public bool _redKeyCollected = false;

        public bool _greenChestOpened = false;
        public bool _yellowChestOpened = false;
        public bool _blackChestOpened = false;
        public bool _redChestOpened = false;

        private void Awake()
        {
            _isJumpAttacking = false;
            GemText.text = _gems.ToString();
            HealthText.text = _lives.ToString();
            Instance = this;
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            _isRecharged = true;
            _losePanel.SetActive(false);
            _timeBetweenShots = _startTimeBetweenShots;
        }
        public static Player Instance { get; set; }
        private States states
        {
            get { return (States)animator.GetInteger("State"); }
            set { animator.SetInteger("State", (int)value); }
        }

        private void FixedUpdate()
        {
            CheckGround();
        }
        private void Update()
        {
            
                
            if (_isGrounded && !_isAttacking && !_isBlocking && !_isJumpAttacking)
                states = States.Idle;
            if (Input.GetButton("Horizontal") && !_isAttacking && !_isBlocking)
                Run();
            if (Input.GetKeyDown("j") && !_isAttacking && _jumpTimes < 1 && !_isBlocking && !_isJumpAttacking)
                Jump();
            if (Input.GetKeyDown("k"))
            {
                Attack();

                if (!_isGrounded && !_isJumpAttacking && _yellowChestOpened)
                    JumpAttack();
            }
            if (Input.GetKeyDown("l") && _canShoot && !_isBlocking && !_isAttacking && _greenChestOpened)
            {
                if (_isFliped)
                {
                    Instantiate(_PlayerBolt, _firePointLeft.position, _firePointLeft.rotation);
                }
                else
                {
                    Instantiate(_PlayerBolt, _firePointRight.position, _firePointRight.rotation);
                }
                _timeBetweenShots = _startTimeBetweenShots;
            }
            if (_timeBetweenShots < 0)
            {
                _canShoot = true;
            }
            else
            {
                _timeBetweenShots -= Time.deltaTime;
                _canShoot = false;
            }
            if (Input.GetKey("s") && !_isAttacking && _isGrounded && _redChestOpened)
            {
                Debug.Log("Blocking");
                Block();
                _isBlocking = true;
            }
            else
            {
                _isBlocking = false;
            }

        }

        private void Run()
        {
            if (_isGrounded && !_isBlocking)
                states = States.Run;

            Vector3 rotation = transform.eulerAngles;
            Vector3 direction = transform.right * Input.GetAxis("Horizontal");

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, _speed * Time.deltaTime);

            sprite.flipX = direction.x < 0f;
            if (direction.x < 0f)
                _isFliped = true;
            else
                _isFliped = false;
        }
        private void Jump()
        {
            rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
            _jumpSound.Play();
            _jumpTimes++;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }
        private void CheckGround()
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f, Level);
            _isGrounded = collider.Length > 0;

            if (!_isGrounded && !_isJumpAttacking)
                states = States.Jump;

            if (_isGrounded)
            {
                _jumpTimes = 0;
                _isJumpAttacking = false;
            }
        }
        public override void GetDamage()
        {
            if (!_isBlocking && !_isJumpAttacking)
            {
                _lives -= 10;
                Debug.Log(_lives);
                HealthText.text = _lives.ToString();
            }
                if (_lives <= 0)
                {
                    Die();
                }
            
        }

        public override void Die()
        {
            _losePanel.SetActive(true);
            Time.timeScale = 0;
        }

        private void Attack()
        {
            if (_isGrounded && _isRecharged && !_isBlocking)
            {
                states = States.Attack;
                _isAttacking = true;
                _isRecharged = false;

                StartCoroutine(AttackAnimation());
                StartCoroutine(AttackCoolDown());
                _attackSound.Play();
            }  
        }

        private void JumpAttack()
        {
            if (!_isGrounded && !_isJumpAttacking)
            {
                states = States.JumpAttack;
                rigidbody.AddForce(transform.up * -_jumpForce, ForceMode2D.Impulse);
                _isJumpAttacking = true;
            }
        }

        private void OnAttack()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemy);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<Entity>().GetDamage();
                StartCoroutine(EnemyAttacked(colliders[i]));
            }
        }
        private IEnumerator AttackAnimation()
        {
            yield return new WaitForSeconds(0.45f);
            _isAttacking = false;
        }
        private IEnumerator AttackCoolDown()
        {
            yield return new WaitForSeconds(0.55f);
            _isRecharged = true;
        }

        private IEnumerator EnemyAttacked(Collider2D enemy)
        {
            SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
            enemyColor.color = new Color(1, 0.3515625f, 0.3515625f);
            yield return new WaitForSeconds(0.2f);
            enemyColor.color = new Color(1, 1, 1);
        }

        private void Block()
        {
            states = States.Blocking;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Gem")
            {
                _gems++;
                GemText.text = _gems.ToString();
                _gemSound.Play();
            }

            if (collision.gameObject.tag == "GreenKey")
            {
                _greenKeyCollected = true;
                _keySound.Play();
            }
            if (collision.gameObject.tag == "YellowKey")
            {
                _yellowKeyCollected = true;
                _keySound.Play();
            }
            if (collision.gameObject.tag == "GrayKey")
            {
                _blackKeyCollected = true;
                _keySound.Play();
            }
            if (collision.gameObject.tag == "RedKey")
            {
                _redKeyCollected = true;
                _keySound.Play();
            }

            if (collision.gameObject.tag == "GreenChest" && _greenKeyCollected && !_greenChestOpened)
            {
                _greenChestOpened = true;
                _chestSound.Play();
            }
            if (collision.gameObject.tag == "YellowChest" && _yellowKeyCollected && !_yellowChestOpened)
            {
                _yellowChestOpened = true;
                _chestSound.Play();
            }
            if (collision.gameObject.tag == "BlackChest" && _blackKeyCollected && !_blackChestOpened)
            {
                _blackChestOpened = true;
                _chestSound.Play();
            }
            if (collision.gameObject.tag == "RedChest" && _redKeyCollected && !_redChestOpened)
            {
                _redChestOpened = true;
                _chestSound.Play();
            }

        }
    }

    public enum States
    {
        Idle,
        Run,
        Jump,
        Attack,
        Blocking,
        JumpAttack
    }
}