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
                visibleDistance = Stats.Wolf.visibleDistance;
                casualMovementSpeed = Stats.Wolf.casualMovementSpeed;
                fastMovementSpeed = Stats.Wolf.fastMovementSpeed;
                valueChangeRate = Stats.Wolf.valueChangeRate;
                hunger = Stats.Wolf.hunger;
                thirst = Stats.Wolf.thirst;
                urgeToReproduce = Stats.Wolf.urgeToReproduce;
                vitality = Stats.Wolf.vitality;
                stamina = Stats.Wolf.stamina;
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