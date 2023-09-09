using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class MusicScript : MonoBehaviour
    {
        private static MusicScript instance;

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(transform.gameObject);
            }
        }
    }
}