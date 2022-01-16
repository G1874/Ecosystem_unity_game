using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//główna klasa odpowiedzialna za generacje mapy
namespace Terrain{
    public class MapGenerator : MonoBehaviour
    {

        public enum DrawMode{ColourMap, NoiseMap, Mesh};
        public float waterDepth = 0.2f;
        public float edgeDepth = 0.2f;
        public int mapSize = 114;
        public NoiseSettings noiseSettings;
        public bool autoUpdate;
        public TerrainType[] regions;
        public MeshData meshData;

        void Awake(){
            GenerateMap();
            //debug
            
            // for(int y=0; y<mapSize; y++){
            //     for(int x=0; x<mapSize; x++){
            //         for(int j=y+1; j<mapSize; j++){
            //             for(int i=x+1; i<mapSize; i++){
            //                 if(meshData.tileCenters[x, y] == meshData.tileCenters[i, j]){
            //                     Debug.Log("meshData.tileCenters[x, y]" + "  x = " + x + "   y = " + y);
            //                     break;
            //                 }
            //             }
            //         }
            //     }
            // }
            // Debug.Log("no repeat");
        }

        //metoda generująca mapę, korzysta z klas MapDisplay, TextureGenerator, MeshGenerator i NoiseGenerator        
        public void GenerateMap(){
            float[, ] heightMap = HeightmapGenerator.GenerateHeightmap(mapSize, noiseSettings);
            
            Color[] colourMap = new Color[mapSize * mapSize];
            for(int y=0; y<mapSize; y++){
                for(int x=0; x<mapSize; x++){
                    float currentHeight = heightMap[x,y];
                    for(int i=0; i<regions.Length; i++){
                        if(currentHeight <= regions[i].height){
                            colourMap[y * mapSize + x] = regions[i].colour;
                            break;
                        }
                    }
                }
            }
            
            meshData = MeshGenerator.GenerateTerrainMesh(mapSize, heightMap, regions, waterDepth, edgeDepth);

            MapDisplay display = FindObjectOfType<MapDisplay>();
            display.DrawMesh(meshData, TextureGenerator.TextureFromColourMap(colourMap, mapSize));
        }

        void OnValidate(){
            if(mapSize < 1)
                mapSize = 1;
            if(noiseSettings.lacunarity < 1)
                noiseSettings.lacunarity = 1;
            if(noiseSettings.octaves < 0)
                noiseSettings.octaves = 0;
        }

    
        [System.Serializable]
        public struct TerrainType{
            public string name;
            public float height;
            public Color colour;
            public int Steps;
        }
    }
}