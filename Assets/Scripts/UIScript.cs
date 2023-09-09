using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class UIScript : MonoBehaviour
    {

        [SerializeField]
        GameObject Panel;

        private void Awake()
        {
            Panel.SetActive(false);
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                SetPause();
        }
        public void SetPause()
        {
                Panel.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Pause Pressed");
        }

        public void ExitPressed()
        {
            SceneManager.LoadScene("MenuScene");
        }
        public void ResumePressed()
        {
            Panel.SetActive(false);
            Time.timeScale = 1;
        }

        public void RestartPressed()
        {
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1;
        }
    }
}
