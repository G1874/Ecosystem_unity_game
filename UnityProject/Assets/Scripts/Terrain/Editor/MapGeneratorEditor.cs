using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//klasa odpowiada za customowe funkcje w inspektorze obiektu MapGenerator
namespace Terrain{
[CustomEditor (typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {

        public override void OnInspectorGUI(){
            MapGenerator mapGen = (MapGenerator)target;

            if(DrawDefaultInspector()){
                if(mapGen.autoUpdate)
                    mapGen.GenerateMap();
            }

            if(GUILayout.Button("Generate"))
                mapGen.GenerateMap();  
        }
    }
}