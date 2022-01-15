using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//klasa odpowiedzialna za generacje mapy szumu perlina
namespace Terrain{
    public static class HeightmapGenerator{

        public static float[, ] GenerateHeightmap(int mapSize, NoiseSettings noiseSettings){
            float[, ] heightMap = new float[mapSize, mapSize];
            System.Random prng = new System.Random(noiseSettings.seed);
            Vector2[] octaveOffsets = new Vector2[noiseSettings.octaves];
            
            for(int i=0; i<noiseSettings.octaves; i++){
                float offsetX = prng.Next(-100000, 100000) + noiseSettings.offset.x;
                float offsetY = prng.Next(-100000, 100000) + noiseSettings.offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }
            
            if(noiseSettings.scale<=0)
                noiseSettings.scale = 0.0001f;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfSize = mapSize / 2;

            for(int y=0; y<mapSize; y++){
                for(int x=0; x<mapSize; x++){
                    
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for(int i=0; i<noiseSettings.octaves; i++){ 
                        float sampleX = (x - halfSize) / noiseSettings.scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfSize) / noiseSettings.scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= noiseSettings.persistance;
                        frequency *= noiseSettings.lacunarity;
                    }

                    if(noiseHeight>maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if(noiseHeight<minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    heightMap[x, y] = noiseHeight;
                }
            }

            for(int y=0; y<mapSize; y++)
                for(int x=0; x<mapSize; x++)
                    heightMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heightMap[x, y]);


            return heightMap;
        }   
    }
}