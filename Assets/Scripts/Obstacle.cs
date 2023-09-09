using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Obstacle : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == Player.Instance.gameObject)
            {
                Player.Instance.GetDamage();
            }
        }
    }
}