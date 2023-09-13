using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer
{
    public class WinConditionCheck : MonoBehaviour
    {
        private int _gemsOnScene;
        private int _gemsToWin;
        public Text GemsToWinText;

        public static WinConditionCheck Instance { get; set; }

        private void Awake()
        {
            Instance = this;
            GemsToWinText.text = _gemsToWin.ToString() + " gem left";
        }
         
        public void GemCount()
        {
            GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
            _gemsOnScene = gems.Length;
            _gemsToWin = _gemsOnScene - 1;
            GemsToWinText.text = _gemsToWin.ToString() + " gem left";
            Debug.Log(_gemsOnScene);
            if (_gemsOnScene <= 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}