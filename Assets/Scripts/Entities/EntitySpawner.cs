using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] GameObject TreePrefab;
        [SerializeField] GameObject PlantPrefab;
        [SerializeField] GameObject DeerPrefab;
        [SerializeField] GameObject WolfPrefab;
        public int treePopulation = 50;
        public int deerPopulation = 30;
        public int wolfPopulation = 10;
        public int initialEdiblePlantPopulation = 50;
        public float plantGrowTime = 10f;
        public int plantGrowRate = 2;
        public int maxPlantPopulation = 100;
        public bool SpawnOnStart = false;
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

            for(int i=0; i<treePopulation; i++){
                x = Random.Range(0, mapSize-1);
                y = Random.Range(0, mapSize-1);
                if(walkable[x, y]){
                    GameObject.Instantiate(TreePrefab, entityMap.tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
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
                    GameObject.Instantiate(PlantPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
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
                    GameObject.Instantiate(DeerPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(0, 0, 0));
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
                    GameObject.Instantiate(WolfPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(0, 0, 0));
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
                        GameObject.Instantiate(PlantPrefab, entityMap.tileCenters[x, y], Quaternion.Euler(-89.98f, 0, 0));
                        plantMap.addPlant(x, y);
                    }
                    else{
                        i--;
                    } 
                }
            yield return new WaitForSeconds(plantGrowTime);
            coroutineIsRunnig = false;
        }
    }
}