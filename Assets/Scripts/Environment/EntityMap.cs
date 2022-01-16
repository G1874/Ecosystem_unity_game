using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entities{
    public class EntityMap
    {
        public int mapSize;
        public bool[, ] walkable;
        public Vector3[, ] tileCenters;
        public Dictionary<Vector3, bool> WalkableMap = new Dictionary<Vector3, bool>();
        
        public void CreateWalkableMap(){
            for(int y=0; y<mapSize; y++){
                for(int x=0; x<mapSize; x++){
                    Vector3 key = new Vector3(tileCenters[x, y].x, 0, tileCenters[x, y].z);
                    if(walkable[x, y])
                        WalkableMap.Add(key, true);
                    else
                        WalkableMap.Add(key, false);
                }
                // map border
                Vector3 key1 = new Vector3(tileCenters[0, y].x - 1, 0, tileCenters[0, y].z - 1);
                WalkableMap.Add(key1, false);
                key1 = new Vector3(tileCenters[mapSize-1, y].x + 1, 0, tileCenters[mapSize-1, y].z + 1);
                WalkableMap.Add(key1, false);
            }
            //map border
            for(int x=1; x<mapSize-1; x++){
                    Vector3 key2 = new Vector3(tileCenters[x, 0].x - 1, 0, tileCenters[x, 0].z - 1);
                    WalkableMap.Add(key2, false);
                    key2 = new Vector3(tileCenters[x, mapSize-1].x + 1, 0, tileCenters[x, mapSize-1].z + 1);
                    WalkableMap.Add(key2, false);
            }
        }

        public void GetInfo(){
            GameObject generateMap = GameObject.Find("Generate Map");
            Terrain.MapGenerator mapGenerator = generateMap.GetComponent<Terrain.MapGenerator>();
            mapSize = mapGenerator.mapSize;
            walkable = mapGenerator.meshData.walkable;
            tileCenters = mapGenerator. meshData.tileCenters;
        }
    }

    public class TreeMap{       
        bool[, ] treeMap;
        public TreeMap(int mapSize){
            treeMap = new bool[mapSize, mapSize];
            for (int y=0; y<mapSize; y++) 
                for (int x=0; x<mapSize; x++)
                    treeMap[x, y] = false;
        }
        public void addTree(int x, int y){
            treeMap[x, y] = true;
        }
        public bool isTree(int x, int y){
            return treeMap[x, y];
        }
        public int treeNumber(){
            int n = 0;
            for (int y=0; y<treeMap.GetLength(0); y++) 
                for (int x=0; x<treeMap.GetLength(1); x++)
                    if(treeMap[x, y])
                        n++;
            return n;
        }

    }
}