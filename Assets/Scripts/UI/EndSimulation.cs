using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI{
    public class EndSimulation : MonoBehaviour
    {
        public void End(){
            Time.timeScale = 0f;
        }
    }
}