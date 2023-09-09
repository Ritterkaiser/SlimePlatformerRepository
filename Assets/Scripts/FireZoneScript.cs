using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public class FireZoneScript : MonoBehaviour
    {
        private Wizard enemyParent;

        private void Awake()
        {
            enemyParent = GetComponentInParent<Wizard>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                enemyParent.inRange = true;
            }
        }
    }
}