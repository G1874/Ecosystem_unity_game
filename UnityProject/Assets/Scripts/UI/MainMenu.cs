using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UI{
    public class MainMenu : MonoBehaviour
    {
        public void StartSimulation(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame(){
            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}