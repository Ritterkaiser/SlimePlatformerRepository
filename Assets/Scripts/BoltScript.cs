using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class BoltScript : MonoBehaviour
    {
        public float _speed;
        Rigidbody2D rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = transform.right * _speed;
            Destroy(this.gameObject, 5f);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == Player.Instance.gameObject)
            {
                Player.Instance.GetDamage();
                Destroy(this.gameObject);
            }
            if (collision.gameObject.CompareTag("LevelBlock"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}