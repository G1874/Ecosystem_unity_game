using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Tests{
    public class DebugingTool : MonoBehaviour
    {
        [SerializeField] GameObject DebugingCube;
        bool executing = false;
        int x1;
        int y1;
        int x2;
        int y2;
        GameObject targetCube;
        Transform targetTransform;
        Vector3 targetPosition;
        Vector3[] Path;

        void Start(){
            executing = false;
            targetCube = GameObject.Find("TargetCube");
            targetTransform = targetCube.GetComponent<Transform>();
            targetPosition = targetTransform.position;
        }

        void Update()
        {
            if(!executing){
                for(int y=0; y<Entities.EntityMap.mapSize; y++){
                    for(int x=0; x<Entities.EntityMap.mapSize; x++){
                        if(Entities.EntityMap.tileCenters[x, y].x == transform.position.x && Entities.EntityMap.tileCenters[x, y].z == transform.position.z){
                            Debug.Log(x + "  " + y);
                            x1 = x;
                            y1 = y;
                            break;
                        }
                    }
                }
                
                for(int y=0; y<Entities.EntityMap.mapSize; y++){
                    for(int x=0; x<Entities.EntityMap.mapSize; x++){
                        if(Entities.EntityMap.tileCenters[x, y].x == targetPosition.x && Entities.EntityMap.tileCenters[x, y].z == targetPosition.z){
                            Debug.Log(x + "  " + y);
                            x2 = x;
                            y2 = y;
                            break;
                        }
                    }
                }
                Path = PathFinder.GetPath(x1, y1, x2, y2);
                if(Path != null){
                    for (int i=0; i<Path.Length; i++)
                    {
                        GameObject.Instantiate(DebugingCube, Path[i], Quaternion.Euler(0, 0, 0));
                    }
                }
                else
                {
                    Debug.Log("path abstucted");
                }

                executing = true;
            }
        }
    }
}