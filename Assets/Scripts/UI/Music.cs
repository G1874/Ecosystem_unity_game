using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI{
    public class Music : MonoBehaviour
    {
        void Awake(){
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}