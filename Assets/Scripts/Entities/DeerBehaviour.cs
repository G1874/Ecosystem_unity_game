using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class DeerBehaviour : AnimalBehaviour
    {
        public bool changeStatsInEditor;
        bool coroutineIsRunnig1 = false;
        bool coroutineIsRunnig2 = false;
        bool coroutineIsRunnig3 = false;
        GameObject nearestPredator;
        float speed;
        bool InAction = false;
        int pathStep;
        

        IEnumerator scanSurroundings(){
            coroutineIsRunnig1 = true;
            if(hunger<=11)
                nearestEdible = scanSurroundingsForObject(EntityMap.plants, visibleDistance);
            if(thirst<=11)
                nearestWaterTile = scanSurroundingsForTile(EntityMap.waterTiles, visibleDistance);
            nearestPredator = scanSurroundingsForObject(EntityMap.wolfs, visibleDistance);
            yield return new WaitForSeconds(0.25f);
            coroutineIsRunnig1 = false;
        }

        IEnumerator Eat(){
            coroutineIsRunnig2 = true;
            yield return new WaitForSeconds(1f);
            EntityMap.plants.Remove(nearestEdible);
            Destroy(nearestEdible);
            hunger = hunger + 10;
            InAction = false;
            coroutineIsRunnig2 = false;
        }

        IEnumerator Drink(){
            coroutineIsRunnig3 = true;
            yield return new WaitForSeconds(1f);
            thirst = thirst + 10;
            InAction = false;
            coroutineIsRunnig3 = false;
        }

        void goToFood(){
            if(!InAction && !onTheMove){
                Path = findFood(transform.position, nearestEdible);
                speed = casualMovementSpeed;
                pathStep = 0;
                InAction = true;
            }
            else if(InAction && !onTheMove && Path!=null){
                Rotate(Path[pathStep] - transform.position);
                if(Path[pathStep] == nearestEdible.transform.position){
                    if(!coroutineIsRunnig2)
                        StartCoroutine(Eat());
                }
                else
                {
                    Target = Path[pathStep];
                    pathStep++;
                    onTheMove = true;
                }        
            }
            else if(InAction && Path==null)
            {
                casualMovement();
                speed = casualMovementSpeed;
                InAction = false;
            }
        }

        void goToWater(){
            if(!InAction && !onTheMove){
                Path = findWater(transform.position, nearestWaterTile);
                speed = casualMovementSpeed;
                pathStep = 0;
                InAction = true;
            }
            else if(InAction && !onTheMove && Path!=null){
                Rotate(new Vector3((Path[pathStep] - transform.position).x, 0, (Path[pathStep] - transform.position).z));
                if(Path[pathStep] == nearestWaterTile){
                    if(!coroutineIsRunnig3)
                        StartCoroutine(Drink());
                }
                else
                {
                    Target = Path[pathStep];
                    pathStep++;
                    onTheMove = true;
                }
            }
            else if(InAction && Path==null)
            {
                casualMovement();
                speed = casualMovementSpeed;
                InAction = false;
            }
        }

        void DecideOnAction(){
            if(nearestPredator == null){
                if(thirst<=10 && nearestWaterTile.y!=100f){
                    goToWater();
                }
                else if(hunger<=10 && nearestEdible!=null){
                    goToFood();
                }
                else{
                    casualMovement();
                    speed = casualMovementSpeed;
                }
            }
            else
            {
                InAction = false;
            }
        }

        void Awake()
        {
            InAction = false;
            if(!changeStatsInEditor){
                visibleDistance = DeerStats.visibleDistance;
                casualMovementSpeed = DeerStats.casualMovementSpeed;
                fastMovementSpeed = DeerStats.fastMovementSpeed;
                valueChangeRate = DeerStats.valueChangeRate;
                hunger = DeerStats.hunger;
                thirst = DeerStats.thirst;
                urgeToReproduce = DeerStats.urgeToReproduce;
                vitality = DeerStats.vitality;
                stamina = DeerStats.stamina;
            }
        }

        void Update()
        {   
            if(!coroutine2IsRunnig)
                StartCoroutine(changeSurvivalParameters());
            if(hunger <= 0 || thirst <= 0)
                Death();
            if(!coroutineIsRunnig1)
                StartCoroutine(scanSurroundings());
            DecideOnAction();
            Move(speed);
        }
    }
}