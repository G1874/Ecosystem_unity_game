using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class AnimalBehaviour : MonoBehaviour
    {
        protected bool needsUpdate;
        protected bool objectRotated;
        protected Vector3 newDirection;
        protected Vector3[] Directions = new Vector3[]{new Vector3(1f, 0, 0),new  Vector3(-1f, 0, 0),new  Vector3(0, 0, 1f),new  Vector3(0, 0, -1f),new  Vector3(1f, 0, 1f),new  Vector3(1f, 0, -1f),new  Vector3(-1f, 0, 1f),new  Vector3(1f, 0, -1f)};
        public int[] stopInterval = {1, 3};
        protected bool onTheMove = false;
        protected bool coroutine1IsRunnig = false;
        protected bool coroutine2IsRunnig = false;


        public float casualMovementSpeed = 5f;
        public float fastMovementSpeed = 10f;
        public float valueChangeRate = 5f;
        public int hunger = 25;
        public int thirst = 25;
        public int urgeToReproduce = 35;
        public int vitality = 100;
        public int stamina = 100;

        protected int weightedRandomDirection(){
            int randomDirection = Random.Range(0, 8);
            return randomDirection;
        }

        protected Vector3[] findShortestPath(){
            return null;
        }

        protected void getNewDirection(Dictionary<Vector3, bool> WalkableMap){
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

        protected IEnumerator changeSurvivalParameters(){
            coroutine2IsRunnig = true;
            hunger--;
            thirst--;
            urgeToReproduce--;
            vitality--;
            yield return new WaitForSeconds(valueChangeRate);
            coroutine2IsRunnig = false;
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

        protected void casualMovement(){
            if(!coroutine1IsRunnig)
            StartCoroutine(StopRandomly());
            if(onTheMove)
                Move();
        }

        protected IEnumerator StopRandomly(){
            coroutine1IsRunnig = true;

            yield return new WaitForSeconds((float)Random.Range(stopInterval[0], stopInterval[1]));
            onTheMove = !onTheMove;
            needsUpdate = true;
            coroutine1IsRunnig = false;
        }
        
        protected void Move(){
            getNewDirection(EntityMap.WalkableMap);
            if(!objectRotated && !needsUpdate){
                transform.rotation = Quaternion.LookRotation(newDirection);
                objectRotated = true;
            }
            else if(objectRotated && !needsUpdate){
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, casualMovementSpeed * Time.deltaTime);
            }
        }

        protected void Death(){
            Destroy(gameObject);
        }
    }
}
