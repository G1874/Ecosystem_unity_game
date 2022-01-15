using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//klasa odpowiedzialna za generacje textury mapy
namespace Terrain{
    public static class TextureGenerator{

        public static Texture2D TextureFromColourMap(Color[] colourMap, int size){
            Texture2D texture = new Texture2D(size, size);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colourMap);
            texture.Apply();
            return texture;
        }
    }
}