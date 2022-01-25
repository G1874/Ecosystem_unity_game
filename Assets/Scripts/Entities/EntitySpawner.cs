using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] GameObject Tree1Prefab;
        [SerializeField] GameObject Tree2Prefab;
        [SerializeField] GameObject RockPrefab;
        [SerializeField] GameObject EdiblePlantPrefab;
        [SerializeField] GameObject DeerPrefab;
        [SerializeField] GameObject WolfPrefab;
        [SerializeField] GameObject DebugingCube;
        public int treePopulation = 50;
        public int deerPopulation = 30;
        public int wolfPopulation = 10;
        public int initialEdiblePlantPopulation = 50;
        public float plantGrowTime = 10f;
        public int plantGrowRate = 2;
        public int maxPlantPopulation = 100;
        public bool SpawnOnStart = false;
        TreeMap treeMap;
        EdiblePlantMap plantMap;
        int mapSize;
        Vector3[, ] tileCenters;
        bool[, ] walkable;
        bool coroutineIsRunnig = false;
        void Start(){
            EntityMap.GetInfo();
            mapSize = EntityMap.mapSize;
            tileCenters = EntityMap.tileCenters;
            walkable = EntityMap.walkable;
            treeMap = new TreeMap(mapSize);
            plantMap = new EdiblePlantMap(mapSize);

            if(SpawnOnStart)
                initialSpawn(treeMap, plantMap);
            
            EntityMap.CreateWalkableMap();

            //DebugingTool();
        }

        void Update(){
            StartCoroutine(grassGrowing(plantMap));
        }

        void initialSpawn(TreeMap treeMap, EdiblePlantMap plantMap){
            int x;
            int y;
            int n;
            bool[, ] animalSpawnMap;

            for(int i=0; i<treePopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y]){
                    n = Random.Range(0, 10);
                    if(n>=0 && n<2)
                        GameObject.Instantiate(RockPrefab, tileCenters[x, y], Quaternion.Euler(-89.98f, (float)Random.Range(-179, 180), 0));
                    else if(n>=2 && n<6)
                        GameObject.Instantiate(Tree1Prefab, tileCenters[x, y], Quaternion.Euler(-89.98f, (float)Random.Range(-179, 180), 0));
                    else
                        GameObject.Instantiate(Tree2Prefab, tileCenters[x, y], Quaternion.Euler(-89.98f, (float)Random.Range(-179, 180), 0));

                    treeMap.addTree(x, y);
                    walkable[x, y] = false;
                }
                else{
                    i--;
                } 
            }

            for(int i=0; i<initialEdiblePlantPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y] && !plantMap.plantMap[x, y]){
                    GameObject.Instantiate(EdiblePlantPrefab, tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
                    plantMap.addPlant(x, y);
                }
                else{
                    i--;
                } 
            }

            animalSpawnMap = walkable;
            for(int i=0; i<deerPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(animalSpawnMap[x, y]){
                    GameObject.Instantiate(DeerPrefab, tileCenters[x, y], Quaternion.Euler(0, 0, 0));
                    animalSpawnMap[x, y] = false;
                }
                else{
                    i--;
                } 
            }

            for(int i=0; i<wolfPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(animalSpawnMap[x, y]){
                    GameObject.Instantiate(WolfPrefab, tileCenters[x, y], Quaternion.Euler(0, 0, 0));
                    animalSpawnMap[x, y] = false;
                }
                else{
                    i--;
                } 
            }
        }

        IEnumerator grassGrowing(EdiblePlantMap plantMap){
            if(coroutineIsRunnig)
                yield break;
            coroutineIsRunnig = true;
            Debug.Log(plantMap.plantNumber());
            int x;
            int y;
            if(plantMap.plantNumber() <= maxPlantPopulation)
                for(int i=0; i<plantGrowRate; i++){
                    x = Random.Range(0, mapSize-1);
                    y = Random.Range(0, mapSize-1);
                    if(walkable[x, y] && !plantMap.plantMap[x, y]){
                        GameObject.Instantiate(EdiblePlantPrefab, tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
                        plantMap.addPlant(x, y);
                    }
                    else{
                        i--;
                    } 
                }
            yield return new WaitForSeconds(plantGrowTime);
            coroutineIsRunnig = false;
        }

        void DebugingTool(){
            for(int y=0; y<mapSize; y++)
                for(int x=0; x<mapSize; x++)
                    if(!walkable[x, y])
                        GameObject.Instantiate(DebugingCube, new Vector3(0, 2f, 0) + tileCenters[x, y], Quaternion.Euler(0, 0, 0));
        }
    }
}