using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;

namespace Tests{
    public class TestSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject CubePrefab;
        void Start(){
            GameObject generateMap = GameObject.Find("Generate Map");
            MapGenerator mapGenerator = generateMap.GetComponent<MapGenerator>();
            for(int y=0; y<mapGenerator.mapSize; y++){
                for(int x=0; x<mapGenerator.mapSize; x++){
                    if(mapGenerator.meshData.walkable[x, y])
                    GameObject.Instantiate(CubePrefab, mapGenerator.meshData.tileCenters[x, y] + new Vector3(0, 1, 0), transform.rotation);
                }
            }
        }
        void Update()
        {
        
        }
    }
}