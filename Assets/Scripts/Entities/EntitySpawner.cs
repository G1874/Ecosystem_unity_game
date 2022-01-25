using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class EntitySpawner : MonoBehaviour
    {
        public SpawnSettings spawnSettings;
        public EntityMap entityMap = new EntityMap();
        TreeMap treeMap;
        EdiblePlantMap plantMap;
        int mapSize;
        Vector3[, ] tileCenters;
        bool[, ] walkable;
        bool coroutineIsRunnig = false;
        void Start(){
            entityMap.GetInfo();
            mapSize = entityMap.mapSize;
            tileCenters = entityMap.tileCenters;
            walkable = entityMap.walkable;
            treeMap = new TreeMap(mapSize);
            plantMap = new EdiblePlantMap(mapSize);

            if(SpawnOnStart)
                initialSpawn(treeMap, plantMap);
        }

        void Update(){
            StartCoroutine(grassGrowing(plantMap));
        }

        void initialSpawn(TreeMap treeMap, EdiblePlantMap plantMap){
            int x;
            int y;
            bool[, ] animalSpawnMap;

            for(int i=0; i<spawnSettings.treePopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y]){
                    GameObject.Instantiate(spawnSettings.TreePrefab, entityMap.tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
                    treeMap.addTree(x, y);
                    walkable[x, y] = false;
                }
                else{
                    i--;
                } 
            }

            for(int i=0; i<spawnSettings.initialEdiblePlantPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y] && !plantMap.plantMap[x, y]){
                    GameObject.Instantiate(spawnSettings.PlantPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
                    plantMap.addPlant(x, y);
                }
                else{
                    i--;
                } 
            }

            animalSpawnMap = walkable;
            for(int i=0; i<spawnSettings.deerPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(animalSpawnMap[x, y]){
                    GameObject.Instantiate(spawnSettings.DeerPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(0, 0, 0));
                    animalSpawnMap[x, y] = false;
                }
                else{
                    i--;
                } 
            }

            for(int i=0; i<spawnSettings.wolfPopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(animalSpawnMap[x, y]){
                    GameObject.Instantiate(spawnSettings.WolfPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(0, 0, 0));
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
            if(plantMap.plantNumber() <= spawnSettings.maxPlantPopulation)
                for(int i=0; i<spawnSettings.plantGrowRate; i++){
                    x = Random.Range(0, mapSize-1);
                    y = Random.Range(0, mapSize-1);
                    if(walkable[x, y] && !plantMap.plantMap[x, y]){
                        GameObject.Instantiate(spawnSettings.PlantPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
                        plantMap.addPlant(x, y);
                    }
                    else{
                        i--;
                    } 
                }
            yield return new WaitForSeconds(spawnSettings.plantGrowTime);
            coroutineIsRunnig = false;
        }
    }
}