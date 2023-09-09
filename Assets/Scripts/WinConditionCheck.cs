using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class WinConditionCheck : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        private int _gemsOnScene;

        public static WinConditionCheck Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }
         
        public void GemCount()
        {
            GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
            _gemsOnScene = gems.Length;
            Debug.Log(_gemsOnScene);
            if (_gemsOnScene == 1)
            {
                _winPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}