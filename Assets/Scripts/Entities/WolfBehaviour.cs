using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class WolfBehaviour : AnimalBehaviour
    {
        void Start()
        {
            needsUpdate = false;
            objectRotated = true;
        }

        
        void Update()
        {   
            casualMovement();
            if(!coroutine2IsRunnig)
                StartCoroutine(changeSurvivalParameters());
            if(hunger <= 0)
                Death();
        }
    }
}