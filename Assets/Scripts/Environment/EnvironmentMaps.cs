using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Environment{
    public class EnvironmentMaps
    {
        public int mapSize;
        public bool[, ] walkable;
        public Vector3[, ] tileCenters;
        public Dictionary<float, bool> WalkableMap = new Dictionary<float, bool>();
        
        // // public void CreateWalkableMap(){
        // //     for(int y=0; y<mapSize; y++){
        // //         for(int x=0; x<mapSize; x++){
        // //             float key = tileCenters[x, y].x * 1000 + tileCenters[x, y].z;
        // //             if(walkable[x, y])
        // //                 WalkableMap.Add(key, true);
        // //             else
        // //                 WalkableMap.Add(key, false);
        // //         }
        // //     }
        // }

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