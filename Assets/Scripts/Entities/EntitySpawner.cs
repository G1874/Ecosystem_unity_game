using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField]
        GameObject TreePrefab;
        [SerializeField]
        GameObject PlantPrefab;
        [SerializeField]
        GameObject DeerPrefab;
        [SerializeField]
        GameObject WolfPrefab;
        public int treePopulation = 50;
        public int plantPopulation = 70;
        public int deerPopulation = 30;
        public int wolfPopulation = 10;

        public bool SpawnOnStart = false;
        EntityMap entityMap = new EntityMap();
        int mapSize;
        Vector3[, ] tileCenters;
        bool[, ] walkable;
        void Start(){
            entityMap.GetInfo();
            mapSize = entityMap.mapSize;
            tileCenters = entityMap.tileCenters;
            walkable = entityMap.walkable;
            TreeMap treeMap = new TreeMap(mapSize);

            if(SpawnOnStart)
                initialSpawn(treeMap);
        }


        void initialSpawn(TreeMap treeMap){
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
    }
}