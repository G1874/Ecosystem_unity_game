using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSimulation : MonoBehaviour
{
    public void End(){
        Time.timeScale = 0f;
    }
}
