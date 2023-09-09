using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
    {
    public class Entity : MonoBehaviour
    {
        protected int _lives;
        public virtual void GetDamage()
        {
            _lives--;
            if (_lives < 1)
                Die();
        }

        public virtual void Die()
        {
            Destroy(this.gameObject);
        }
    }
}