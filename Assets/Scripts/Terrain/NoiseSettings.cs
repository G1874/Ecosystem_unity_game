using System.Collections;
using System.Collections.Generic; 
using UnityEngine;


//ustawienia szumu perlina do zmiany w inspektorze
namespace Terrain{
[System.Serializable]
    public class NoiseSettings{
        public float scale = 25;
        public int octaves = 4;
        [Range(0, 1)]
        public float persistance = 0.5f;
        public float lacunarity = 2;
        public int seed;
        public Vector2 offset;
    }
}