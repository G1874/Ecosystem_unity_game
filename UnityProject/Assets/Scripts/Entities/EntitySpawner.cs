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
        public int decorativesPopulation = 50;
        public int initialDeerPopulation = 30;
        public int initialWolfPopulation = 10;
        public int initialEdiblePlantPopulation = 50;
        public float plantGrowTime = 10f;
        public int plantGrowRate = 2;
        public int maxPlantPopulation = 100;
        public bool SpawnOnStart = false;
        int mapSize;
        Vector3[, ] tileCenters;
        bool[, ] walkable;
        bool[, ] treeMap;
        bool[, ] plantMap;
        bool coroutineIsRunnig = false;
        public bool changeStatsInEditor;
        
        void Start(){
            if(!changeStatsInEditor){
                decorativesPopulation = EnvironmentStats.decorativesPopulation;
                initialDeerPopulation = DeerStats.initialDeerPopulation;
                initialWolfPopulation = WolfStats.initialWolfPopulation;
                initialEdiblePlantPopulation = EnvironmentStats.initialEdiblePlantPopulation;
                plantGrowTime = EnvironmentStats.plantGrowTime;
                plantGrowRate = EnvironmentStats.plantGrowRate;
                maxPlantPopulation = EnvironmentStats.maxPlantPopulation;
            }

            EntityMap.GetInfo();
            mapSize = EntityMap.mapSize;
            tileCenters = EntityMap.tileCenters;
            walkable = EntityMap.walkable;
            treeMap = EntityMap.treeMap;
            plantMap = EntityMap.plantMap;

            if(SpawnOnStart)
                initialSpawn();
            
            EntityMap.CreateWalkableMap();
            EntityMap.CreateCoord();

            DebugingTool();
        }

        void Update(){
            if(!coroutineIsRunnig)
                StartCoroutine(grassGrowing());
        }

        void initialSpawn(){
            int x;
            int y;
            int n;
            bool[, ] animalSpawnMap;
            
            for(int i=0; i<decorativesPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y]){
                    n = Random.Range(0, 10);
                    if(n>=0 && n<2)
                        GameObject.Instantiate(RockPrefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(-89.98f, (float)Random.Range(-179, 180), 0));
                    else if(n>=2 && n<6)
                        GameObject.Instantiate(Tree1Prefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(-89.98f, (float)Random.Range(-179, 180), 0));
                    else
                        GameObject.Instantiate(Tree2Prefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(-89.98f, (float)Random.Range(-179, 180), 0));

                    EntityMap.addTree(x, y);
                    walkable[x, y] = false;
                }
                else{
                    i--;
                } 
            }

            for(int i=0; i<initialEdiblePlantPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y] && !plantMap[x, y]){
                    GameObject newPlant = (GameObject)Instantiate(EdiblePlantPrefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(-89.98f, 0, 0));
                    EntityMap.addPlant(x, y);
                    EntityMap.plants.Add(newPlant);
                }
                else{
                    i--;
                } 
            }

            animalSpawnMap = walkable;
            for(int i=0; i<initialDeerPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(animalSpawnMap[x, y]){
                    GameObject newDeer = (GameObject)Instantiate(DeerPrefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(0, 0, 0));
                    animalSpawnMap[x, y] = false;
                    EntityMap.deer.Add(newDeer);
                }
                else{
                    i--;
                } 
            }

            for(int i=0; i<initialWolfPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(animalSpawnMap[x, y]){
                    GameObject newWolf = (GameObject)Instantiate(WolfPrefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(0, 0, 0));
                    animalSpawnMap[x, y] = false;
                    EntityMap.wolfs.Add(newWolf);
                }
                else{
                    i--;
                } 
            }
        }

        IEnumerator grassGrowing(){
            if(coroutineIsRunnig)
                yield break;
            coroutineIsRunnig = true;
            //Debug.Log(EntityMap.plantNumber());
            int x;
            int y;
            if(EntityMap.plantNumber() <= maxPlantPopulation)
                for(int i=0; i<plantGrowRate; i++){
                    x = Random.Range(0, mapSize-1);
                    y = Random.Range(0, mapSize-1);
                    if(walkable[x, y] && !plantMap[x, y]){
                        GameObject newPlant = (GameObject)Instantiate(EdiblePlantPrefab, new Vector3(EntityMap.tileCenters[x, y].x, 0, EntityMap.tileCenters[x, y].z), Quaternion.Euler(-89.98f, 0, 0));
                        EntityMap.addPlant(x, y);
                        EntityMap.plants.Add(newPlant);
                    }
                    else{
                        i--;
                    } 
                }
            yield return new WaitForSeconds(plantGrowTime);
            coroutineIsRunnig = false;
        }

        void DebugingTool(){
            // for(int y=0; y<mapSize; y++)
            //     for(int x=0; x<mapSize; x++)
            //         if(!walkable[x, y])
            //             GameObject.Instantiate(DebugingCube, EntityMap.tileCenters[x, y] + new Vector3(0, 2f, 0), Quaternion.Euler(0, 0, 0));

            // for(int i=0; i<EntityMap.waterTiles.Length; i++)
            //     GameObject.Instantiate(DebugingCube, EntityMap.waterTiles[i] + new Vector3(0, 2f, 0), Quaternion.Euler(0, 0, 0));

            // for(int y=0; y<mapSize; y++){
            //     for(int x=0; x<mapSize; x++){
            //         int[] arg = EntityMap.Coord[tileCenters[x, y]];
            //         if(arg[0] == x && arg[1] == y){
            //             Debug.Log(x + "  " + y);
            //         }
            //         else
            //         {
            //             Debug.Log("Problem");
            //             break;
            //         }
            //     }
            // }

            for(int y=0; y<mapSize; y++)
                for(int x=0; x<mapSize; x++)
                    Debug.Log(tileCenters[x, y]);
        }
    }
}