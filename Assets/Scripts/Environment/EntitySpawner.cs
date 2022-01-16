using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities{
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField]
        GameObject TreePrefab;
        [SerializeField]
        GameObject DeerPrefab;
        public int population = 50;
        public bool SpawnOnStart = false;
        EntityMap entityMap = new EntityMap();

        void Start(){
            entityMap.GetInfo();
            TreeMap treeMap = new TreeMap(entityMap.mapSize);

            if(SpawnOnStart)
                initialSpawn(entityMap.mapSize, entityMap.tileCenters, entityMap.walkable, treeMap);
        }


        void initialSpawn(int mapSize, Vector3[, ] tileCenters, bool[, ] walkable, TreeMap treeMap){
            int x;
            int y;

            for(int i=0; i<population; i++){
                x = Random.Range(0, mapSize);
                y = Random.Range(0, mapSize);
                if(entityMap.walkable[x, y] && !treeMap.isTree(x, y)){
                    GameObject.Instantiate(TreePrefab, entityMap.tileCenters[x, y] + new Vector3(0, 0.01f, 0), Quaternion.Euler(-89.98f, 0, 0));
                    treeMap.addTree(x, y);
                }
                else{
                    i--;
                } 
            }
        }
    }
}