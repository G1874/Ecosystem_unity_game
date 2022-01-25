using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class DeerBehaviour : AnimalBehaviour
    {
        bool coroutineIsRunnig = false;

        void findEdibles(){
            
        }

        void scanForPredators(){

        }

        IEnumerator scanSurroundings(){
            coroutineIsRunnig = true;


            yield return new WaitForSeconds(1f);
            coroutineIsRunnig = false;
        }

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