using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class WolfBehaviour : AnimalBehaviour
    {
        public bool changeStatsInEditor;
        void Awake()
        {
            if(!changeStatsInEditor)
            {
                visibleDistance = WolfStats.visibleDistance;
                casualMovementSpeed = WolfStats.casualMovementSpeed;
                fastMovementSpeed = WolfStats.fastMovementSpeed;
                valueChangeRate = WolfStats.valueChangeRate;
                hunger = WolfStats.hunger;
                thirst = WolfStats.thirst;
                urgeToReproduce = WolfStats.urgeToReproduce;
                vitality = WolfStats.vitality;
                stamina = WolfStats.stamina;
            }
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