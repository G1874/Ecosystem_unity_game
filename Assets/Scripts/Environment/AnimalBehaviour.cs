using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class AnimalBehaviour : MonoBehaviour
    {
        public float movementSpeed = 3f;
        public float rotationSpeed = 0.2f;
        EntityMap entityMap = new EntityMap();
        bool needsUpdate;
        bool objectRotated;
        Vector3 newDirection;
        Vector3[] Directions = new Vector3[]{new Vector3(1f, 0, 0),new  Vector3(-1f, 0, 0),new  Vector3(0, 0, 1f),new  Vector3(0, 0, -1f),new  Vector3(1f, 0, 1f),new  Vector3(1f, 0, -1f),new  Vector3(-1f, 0, 1f),new  Vector3(1f, 0, -1f)};
        public int[] stopInterval = {1, 3};
        bool onTheMove = false;
        bool coroutineIsRunnig = false;


        int weightedRandomDirection(){
            int randomDirection = Random.Range(0, 8);
            return randomDirection;
        }

        void getNewDirection(Dictionary<Vector3, bool> WalkableMap){
            if(needsUpdate){
                newDirection = Directions[weightedRandomDirection()];
                Vector3 newTranslation = transform.position + newDirection;
                Vector3 key = new Vector3((Mathf.Floor(newTranslation.x) + 0.5f), 0, (Mathf.Floor(newTranslation.z) + 0.5f));
                while(!WalkableMap[key]){
                        newDirection = Directions[weightedRandomDirection()];
                        newTranslation = transform.position + newDirection;
                        key = new Vector3((Mathf.Floor(newTranslation.x) + 0.5f), 0, (Mathf.Floor(newTranslation.z) + 0.5f));
                }
                needsUpdate = false;
                objectRotated = false;
            }
            else{
                Vector3 newTranslation = transform.position + transform.forward;
                Vector3 key = new Vector3((Mathf.Floor(newTranslation.x) + 0.5f), 0, (Mathf.Floor(newTranslation.z) + 0.5f));
                if(!WalkableMap[key])
                    needsUpdate = true;
                else
                    newDirection = new Vector3(0, 0, 0);
            }
        }

        // public void RotateObject(){
        //     if(newDirection != Vector3.Zero)
        //         var targetRotation = Quaternion.LookRotation(newDirection);
        //     Quaternion
        // }

        // IEnumerator RotateObject(float inTime){
        //     if(newDirection != Vector3.zero){
        //         var targetRotation = Quaternion.LookRotation(newDirection);
        //         for(float t = 0f; t < 1; t += Time.deltaTime/inTime){
        //             transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
        //             yield return null;
        //         }
        //     }
        //     objectRotated = true;
        // }


        // public bool CanMove(Dictionary<Vector3, bool> WalkableMap){
        //     Vector3 newTranslation = transform.position + transform.forward;
        //     Vector3 key = new Vector3((Mathf.Floor(newTranslation.x) + 0.5f), 0, (Mathf.Floor(newTranslation.z) + 0.5f));
        //     if(WalkableMap[key])
        //         return true;
        //     else
        //         return false;
        // }

        void casualMovement(){
            StartCoroutine(StopRandomly());
            if(onTheMove)
                Move();
        }

        IEnumerator StopRandomly(){
            if(coroutineIsRunnig)
                yield break;
            coroutineIsRunnig = true;

            yield return new WaitForSeconds((float)Random.Range(stopInterval[0], stopInterval[1]));
            onTheMove = !onTheMove;
            coroutineIsRunnig = false;
        }
        
        void Move(){
            getNewDirection(entityMap.WalkableMap);
            if(!objectRotated && !needsUpdate){
                // RotateObject(0.1f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                objectRotated = true;
            }
            else if(objectRotated && !needsUpdate)
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, movementSpeed * Time.deltaTime);
        }

        void Start()
        {
            needsUpdate = false;
            objectRotated = true;
            entityMap.GetInfo();
            entityMap.CreateWalkableMap();
        }

        
        void Update()
        {   
            casualMovement();

        }
    }
}
