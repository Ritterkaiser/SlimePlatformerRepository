using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer
{
    public class MenuScript : MonoBehaviour
    {
        public void PlayPressed()
        {
            SceneManager.LoadScene("GameScene");
            Destroy(GameObject.Find("Audio Source"));
        }
        public void SettingsPressed()
        {

        }
        public void ExitPressed()
        {
            Application.Quit();

            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}