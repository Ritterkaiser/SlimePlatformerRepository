using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class GemScript : MonoBehaviour
    {
        private Animator animator;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        public static GemScript Instance { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                animator.SetTrigger("collected");
            }
        }
        private void Destroy()
        {
            Destroy(this.gameObject);
            WinConditionCheck.Instance.GemCount();
        }
    }
}