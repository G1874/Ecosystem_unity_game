using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//klasa odpowiedzialna za renderowanie textur i siatki terenu
namespace Terrain{
    public class MapDisplay : MonoBehaviour
    {

        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;


        public void DrawMesh(MeshData meshData, Texture2D texture){
            meshFilter.sharedMesh = meshData.CreateMesh();
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }
}
