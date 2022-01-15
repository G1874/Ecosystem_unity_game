using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment{
    public class AnimalBehaviour : MonoBehaviour
    {
        public float movementSpeed = 0.5f;
        public float rotationSpeed = 1f;
        EnvironmentMaps environmentMap = new EnvironmentMaps();
        float newDirection;
        float rotationTurn;
        bool needsUpdate;
        
        float weightedRandomDirection(){
            int randomNumber = Random.Range(-18, 18);
            float randomDirection;
            if(randomNumber != 0)
                randomDirection = (float)360/randomNumber;
            else
                randomDirection = 0;

            return randomDirection;
        }

        // void Direction(Dictionary<float, bool> WalkableMap){
        //     Vector3 newTranslation = new Vector3();

        //     if(needsUpdate){
        //         newDirection = weightedRandomDirection();
        //         newTranslation = Quaternion.Euler(0, transform.localEulerAngles.y + newDirection, 0) * transform.forward;
        //         float key = (Mathf.Floor(newTranslation.x) + 0.5f) * 1000 + (Mathf.Floor(newTranslation.z) + 0.5f);
        //         while(true){
        //             if(WalkableMap[key])
        //                 break;
        //             else{
        //                 newDirection = weightedRandomDirection();
        //                 newTranslation = Quaternion.Euler(0, transform.localEulerAngles.y + newDirection, 0) * transform.forward;
        //                 key = (Mathf.Floor(newTranslation.x) + 0.5f) * 1000 + (Mathf.Floor(newTranslation.z) + 0.5f);
        //             }
        //         }
        //         rotationTurn = Mathf.Sign(newDirection);
        //         needsUpdate = false;
        //     }
        // }

        public void Move(){
            if(newDirection != 0){
                float temporaryRotation = rotationSpeed * Time.deltaTime;
                newDirection = Mathf.Abs(newDirection - temporaryRotation);
                if(newDirection > 0){
                    transform.Rotate(0, rotationTurn * temporaryRotation, 0);
                }
                else{
                    temporaryRotation = temporaryRotation + newDirection;
                    transform.Rotate(0, rotationTurn * temporaryRotation , 0);
                    newDirection = 0;
                }
            }
            else{
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }    
        }

        void Start()
        {
            needsUpdate = true;
            // environmentMap.GetInfo();
            // environmentMap.CreateWalkableMap();
            // // foreach (KeyValuePair<float[], bool> kvp in environmentMap.WalkableMap)
            // // {
            // //     Debug.Log(kvp.Key[0] + " " + kvp.Key[1] + " is walkable = " + kvp.Value);
            // // }   
        }

        
        // void Update()
        // {
        //     environmentMap.GetInfo();
        //     environmentMap.CreateWalkableMap();
        //     Direction(environmentMap.WalkableMap);
        //     Move();
        // }
    }
}
