using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class KeyScript : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Destroy(this.gameObject);
            }
        }

    }
}