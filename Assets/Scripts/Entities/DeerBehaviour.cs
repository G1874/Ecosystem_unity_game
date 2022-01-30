using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class DeerBehaviour : AnimalBehaviour
    {
        bool coroutineIsRunnig = false;
        GameObject nearestEdible;
        GameObject nearestPredator;
        Vector3 nearestWaterTile;
        bool objectRotated2 = false;

        

        IEnumerator scanSurroundings(){
            coroutineIsRunnig = true;
            if(hunger<=11)
                nearestEdible = scanSurroundingsForObject(EntityMap.plants, visibleDistance);
            if(thirst<=11)
                nearestWaterTile = scanSurroundingsForTile(EntityMap.waterTiles, visibleDistance);
            nearestPredator = scanSurroundingsForObject(EntityMap.wolfs, visibleDistance);
            yield return new WaitForSeconds(0.25f);
            coroutineIsRunnig = false;
        }

        void findFood(){
            if(!onTheMove){
                Target = nearestEdible.transform.position;
                onTheMove = true;
            }
        }

        void findWater(){
            if(!onTheMove && transform.position!=nearestWaterTile){
                // Vector3 path = GetPath(transform.position, nearestWaterTile);
                // if(path.y!=100f)
                // {
                //     transform.rotation = Quaternion.LookRotation(path);
                //     Target = transform.position + path;    
                //     onTheMove = true;
                // }
                if(!objectRotated2){
                    transform.rotation = Quaternion.LookRotation(nearestWaterTile - transform.position);
                    objectRotated2 = true;
                    }
                else if(objectRotated2){
                    Target = nearestWaterTile;
                    onTheMove = true;
                }
                // Vector3 path = transform.position + transform.forward;
                // transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Floor(path.x) + 0.5f, 0, Mathf.Floor(path.z) + 0.5f));
                // Target = transform.position + new Vector3(Mathf.Floor(path.x) + 0.5f, 0, Mathf.Floor(path.z) + 0.5f);
            }
        }

        void Start()
        {

        }

        void Update()
        {   
            if(!coroutine2IsRunnig)
                StartCoroutine(changeSurvivalParameters());
            //if(hunger <= 0 || thirst <= 0)
            //    Death();
            if(!coroutineIsRunnig)
                StartCoroutine(scanSurroundings());
            if(nearestPredator == null){
                if(thirst<=10 && nearestWaterTile.y!=100f){
                    findWater();
                    Move(casualMovementSpeed);
                    Debug.Log("looking");
                    Debug.Log(nearestWaterTile);
                }
                else if(hunger<=10 && nearestEdible!=null){
                    findFood();
                    Move(casualMovementSpeed);
                }
                else{
                    casualMovement();
                    Move(casualMovementSpeed);
                }
            }
            else{
                casualMovement();
                Move(casualMovementSpeed);
            }
        }
    }
}