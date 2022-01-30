using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Entities{
    public static class EntityMap
    {
        public static int mapSize;
        public static bool[, ] walkable;
        public static Vector3[, ] tileCenters;
        public static Vector3[] waterTiles;
        public static Dictionary<Vector3, bool> WalkableMap = new Dictionary<Vector3, bool>();
        public static bool[, ] treeMap;
        public static bool[, ] plantMap;
        public static List<GameObject> plants = new List<GameObject>();
        public static List<GameObject> wolfs = new List<GameObject>();
        public static List<GameObject> deer = new List<GameObject>();


        public static void CreateWalkableMap(){
            for(int y=0; y<mapSize; y++){
                for(int x=0; x<mapSize; x++){
                    Vector3 key = new Vector3(tileCenters[x, y].x, 0, tileCenters[x, y].z);
                    if(walkable[x, y])
                        WalkableMap.Add(key, true);
                    else
                        WalkableMap.Add(key, false);
                }
                // map border
                Vector3 key1 = new Vector3(tileCenters[0, y].x - 1, 0, tileCenters[0, y].z);
                WalkableMap.Add(key1, false);
                key1 = new Vector3(tileCenters[mapSize-1, y].x + 1, 0, tileCenters[mapSize-1, y].z);
                WalkableMap.Add(key1, false);
            }
            //map border
            for(int x=0; x<mapSize; x++){
                    Vector3 key2 = new Vector3(tileCenters[x, 0].x, 0, tileCenters[x, 0].z - 1);
                    WalkableMap.Add(key2, false);
                    key2 = new Vector3(tileCenters[x, mapSize-1].x, 0, tileCenters[x, mapSize-1].z + 1);
                    WalkableMap.Add(key2, false);
            }
            WalkableMap.Add(new Vector3(tileCenters[0, 0].x - 1, 0, tileCenters[0, 0].z - 1), false);
            WalkableMap.Add(new Vector3(tileCenters[0, mapSize-1].x - 1, 0, tileCenters[0, mapSize-1].z + 1), false);
            WalkableMap.Add(new Vector3(tileCenters[mapSize-1, 0].x + 1, 0, tileCenters[mapSize-1, 0].z - 1), false);
            WalkableMap.Add(new Vector3(tileCenters[mapSize-1, mapSize-1].x + 1, 0, tileCenters[mapSize-1, mapSize-1].z + 1), false);
        }

        public static void GetInfo(){
            GameObject generateMap = GameObject.Find("Generate Map");
            Terrain.MapGenerator mapGenerator = generateMap.GetComponent<Terrain.MapGenerator>();
            mapSize = mapGenerator.mapSize;
            walkable = mapGenerator.meshData.walkable;
            tileCenters = mapGenerator.meshData.tileCenters;

            treeMap = new bool[mapSize, mapSize];
            plantMap = new bool[mapSize, mapSize];
            for (int y=0; y<mapSize; y++){
                for (int x=0; x<mapSize; x++){
                    treeMap[x, y] = false;
                    plantMap[x, y] = false;
                }
            }
            int n = 0;
            for (int y=0; y<mapSize; y++){
                for (int x=0; x<mapSize; x++){
                    if(!walkable[x, y])
                    n++;      
                }
            }
            waterTiles = new Vector3[n];
            int i = 0;
            for (int y=0; y<mapSize; y++){
                for (int x=0; x<mapSize; x++){
                    if(!walkable[x, y]){
                        waterTiles[i] = tileCenters[x, y];
                        i++;
                    }    
                }
            }
        }

        public static void addTree(int x, int y){
            treeMap[x, y] = true;
        }
        public static int treeNumber(){
            int n = 0;
            for (int y=0; y<treeMap.GetLength(0); y++) 
                for (int x=0; x<treeMap.GetLength(1); x++)
                    if(treeMap[x, y])
                        n++;
            return n;
        }

        public static void addPlant(int x, int y){
            plantMap[x, y] = true;
        }
        public static int plantNumber(){
            int n = 0;
            for (int y=0; y<plantMap.GetLength(0); y++) 
                for (int x=0; x<plantMap.GetLength(1); x++)
                    if(plantMap[x, y])
                        n++;
            return n;
        } 
    }

    // public class TreeMap{       
    //     static bool[, ] treeMap;
    //     public TreeMap(int mapSize){
    //         treeMap = new bool[mapSize, mapSize];
    //         for (int y=0; y<mapSize; y++) 
    //             for (int x=0; x<mapSize; x++)
    //                 treeMap[x, y] = false;
    //     }
    //     public void addTree(int x, int y){
    //         treeMap[x, y] = true;
    //     }
    //     public bool isTree(int x, int y){
    //         return treeMap[x, y];
    //     }
    //     public int treeNumber(){
    //         int n = 0;
    //         for (int y=0; y<treeMap.GetLength(0); y++) 
    //             for (int x=0; x<treeMap.GetLength(1); x++)
    //                 if(treeMap[x, y])
    //                     n++;
    //         return n;
    //     }
    // }

    // public class EdiblePlantMap{
    //     public bool[, ] plantMap;
    //     public EdiblePlantMap(int mapSize){
    //         plantMap = new bool[mapSize, mapSize];
    //         for (int y=0; y<mapSize; y++) 
    //             for (int x=0; x<mapSize; x++)
    //                 plantMap[x, y] = false;
    //     }
    //     public void addPlant(int x, int y){
    //         plantMap[x, y] = true;
    //     }
    //     public int plantNumber(){
    //         int n = 0;
    //         for (int y=0; y<plantMap.GetLength(0); y++) 
    //             for (int x=0; x<plantMap.GetLength(1); x++)
    //                 if(plantMap[x, y])
    //                     n++;
    //         return n;
    //     } 
    // }
}