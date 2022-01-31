using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class AnimalBehaviour : MonoBehaviour
    {
        protected bool needsUpdate = true;
        protected bool objectRotated1 = false;
        protected bool objectRotated2 = false;
        protected Vector3 newDirection;
        protected Vector3[] Directions = new Vector3[]{new Vector3(1f, 0, 0),new  Vector3(-1f, 0, 0),new  Vector3(0, 0, 1f),new  Vector3(0, 0, -1f),new  Vector3(1f, 0, 1f),new  Vector3(1f, 0, -1f),new  Vector3(-1f, 0, 1f),new  Vector3(1f, 0, -1f)};
        Vector3 newRotation;
        protected Vector3 nearestWaterTile;
        protected GameObject nearestEdible;
        protected Vector3 Target;
        protected Vector3[] Path;
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
                objectRotated1 = false;
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

        protected void casualMovement(){
            if(!coroutine1IsRunnig)
                StartCoroutine(StopRandomly());
            if(!onTheMove && !Stop){
                getNewDirection(EntityMap.WalkableMap);
                if(!objectRotated1 && !needsUpdate){
                    Rotate(newDirection);
                    objectRotated1 = true;
                }
                else if(objectRotated1 && !needsUpdate){
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

        protected void Rotate(Vector3 direction){
            transform.rotation = Quaternion.LookRotation(direction);
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

        Vector3[] GetPath (int x, int y, int x2, int y2) {
            // bresenham line algorithm
            int w = x2 - x;
            int h = y2 - y;
            int absW = System.Math.Abs (w);
            int absH = System.Math.Abs (h);

            // Is neighbouring tile
            if (absW <= 1 && absH <= 1) {
                return null;
            }

            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) {
                dx1 = -1;
                dx2 = -1;
            } else if (w > 0) {
                dx1 = 1;
                dx2 = 1;
            }
            if (h < 0) {
                dy1 = -1;
            } else if (h > 0) {
                dy1 = 1;
            }

            int longest = absW;
            int shortest = absH;
            if (longest <= shortest) {
                longest = absH;
                shortest = absW;
                if (h < 0) {
                    dy2 = -1;
                } else if (h > 0) {
                    dy2 = 1;
                }
                dx2 = 0;
            }

            int numerator = longest >> 1;
            Vector3[] path = new Vector3[longest];
            for (int i = 1; i <= longest; i++) {
                numerator += shortest;
                if (numerator >= longest) {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                } else {
                    x += dx2;
                    y += dy2;
                }

                // If not walkable, path is invalid so return null
                // (unless is target tile, which may be unwalkable e.g water)
                if (i != longest && !Entities.EntityMap.walkable[x, y]) {
                    return null;
                }
                path[i - 1] = Entities.EntityMap.tileCenters[x, y];
            }
            return path;
        }

        protected void getNewRotation(Vector3 direction){
            
        }

        protected Vector3[] findWater(Vector3 currentPosition, Vector3 targetPosition){
            if(targetPosition.y == 100f)
                return null;
            int[] arg1 = EntityMap.Coord[new Vector3(currentPosition.x, 0, currentPosition.z)];
            int[] arg2 = EntityMap.Coord[new Vector3(targetPosition.x, 0, targetPosition.z)];
            Vector3[] Path = GetPath(arg1[0], arg1[1], arg2[0], arg2[1]);
            return Path;
        }

        protected Vector3[] findFood(Vector3 currentPosition, GameObject targetObject){
            if(targetObject == null)
                return null;
            int[] arg1 = EntityMap.Coord[new Vector3(currentPosition.x, 0, currentPosition.z)];
            int[] arg2 = EntityMap.Coord[new Vector3(targetObject.transform.position.x, 0, targetObject.transform.position.z)];
            Vector3[] Path = GetPath(arg1[0], arg1[1], arg2[0], arg2[1]);
            return Path;
        }
    }
}
