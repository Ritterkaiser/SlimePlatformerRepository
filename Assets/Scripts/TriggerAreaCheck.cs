using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class TriggerAreaCheck : MonoBehaviour
    {
        private Bandit enemyParent;

        private void Awake()
        {
            enemyParent = GetComponentInParent<Bandit>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                enemyParent.target = collision.transform;
                enemyParent.inRange = true;
                enemyParent.hotZone.SetActive(true);
            }
        }
    }
}