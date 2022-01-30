using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class AnimalBehaviour : MonoBehaviour
    {
        protected bool needsUpdate = true;
        protected bool objectRotated = false;
        protected Vector3 newDirection;
        protected Vector3[] Directions = new Vector3[]{new Vector3(1f, 0, 0),new  Vector3(-1f, 0, 0),new  Vector3(0, 0, 1f),new  Vector3(0, 0, -1f),new  Vector3(1f, 0, 1f),new  Vector3(1f, 0, -1f),new  Vector3(-1f, 0, 1f),new  Vector3(1f, 0, -1f)};
        protected Vector3 Target;
        public int[] stopInterval = {1, 3};
        protected bool Stop = false;
        protected bool onTheMove = false;
        protected bool coroutine1IsRunnig = false;
        protected bool coroutine2IsRunnig = false;
        
        public float visibleDistance = 10f;
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
            if(!onTheMove && !Stop){
                getNewDirection(EntityMap.WalkableMap);
                if(!objectRotated && !needsUpdate){
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    objectRotated = true;
                }
                else if(objectRotated && !needsUpdate){
                    Target = transform.position + newDirection;    
                    onTheMove = true;
                }
            }
        }

        protected IEnumerator StopRandomly(){
            coroutine1IsRunnig = true;

            yield return new WaitForSeconds((float)Random.Range(stopInterval[0], stopInterval[1]));
            Stop = !Stop;
            needsUpdate = true;
            coroutine1IsRunnig = false;
        }
        
        protected void Move(float speed){
            if(onTheMove){
                transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);
                if(transform.position == Target)
                    onTheMove = false;
            }
        }

        protected void Death(){
            Destroy(gameObject);
        }

        protected GameObject scanSurroundingsForObject(List<GameObject> objects, float visibleDistance){
            float nearestDist = float.MaxValue;
            GameObject nearestObject = null;
            Vector3 currentPosition = transform.position;

            foreach(var element in objects){
                if(Vector3.Distance(element.transform.position, currentPosition) < nearestDist){
                    nearestDist = Vector3.Distance(element.transform.position, currentPosition);
                    nearestObject = element;
                }
            }
            if(nearestDist<=visibleDistance)
                return nearestObject;
            else
                return null;
        }

        protected Vector3 scanSurroundingsForTile(Vector3[] Tiles, float visibleDistance){
            float nearestDist = float.MaxValue;
            Vector3 nearestTile = new Vector3();
            Vector3 currentPosition = transform.position;

            foreach(var element in Tiles){
                if(Vector3.Distance(element, currentPosition) < nearestDist){
                    nearestDist = Vector3.Distance(element, currentPosition);
                    nearestTile = element;
                }
            }
            if(nearestDist<=visibleDistance)
                return nearestTile;
            else
                return new Vector3(0, 100f, 0);
        }

        

        // protected Vector3 GetPath (Vector3 currentPosition, Vector3 targetPosition) {
        //     if(currentPosition==targetPosition)
        //         return new Vector3(0, 100f, 0);
        //     Vector3 tileInLine = Quaternion.LookRotation(targetPosition) * new Vector3(1f, 0, 0);
        //     Vector3 path = transform.position + tileInLine;
        //     path = new Vector3(Mathf.Floor(path.x) + 0.5f, path.y, Mathf.Floor(path.z) + 0.5f);
        //     return path;
        // }
    }
}
