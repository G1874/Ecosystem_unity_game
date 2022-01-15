using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//klasa odpowiedzialna za generacje siatki mapy
namespace Terrain{
    public class MeshGenerator{
        public static MeshData GenerateTerrainMesh(int mapSize, float[, ] heightMap, MapGenerator.TerrainType[] regions, float waterDepth, float edgeDepth){
            
            int numTilesPerLine = Mathf.CeilToInt(mapSize);
            float min = -numTilesPerLine / 2f;
            
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            Vector2[] nswe = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
            int[][] sideVertIndexByDir = { new int[] { 0, 1 }, new int[] { 3, 2 }, new int[] { 2, 0 }, new int[] { 1, 3 } };

            MeshData meshData = new MeshData(mapSize);

            for(int y=0; y<numTilesPerLine; y++){
                for(int x=0; x<numTilesPerLine; x++){
                    Vector2 uv = RegionUv(heightMap[x,y], regions);
                    uvs.AddRange (new Vector2[] { uv, uv, uv, uv });

                    bool isWaterTile = uv.x == 0f;
                    bool isLandTile = !isWaterTile;
                    
                    //Vertices
                    int vertexIndex = vertices.Count;
                    float height = (isWaterTile) ? -waterDepth : 0;
                    Vector3 nw = new Vector3 (min + x, height, min + y + 1);
                    Vector3 ne = nw + Vector3.right;
                    Vector3 sw = nw - Vector3.forward;
                    Vector3 se = sw + Vector3.right;
                    Vector3[] tileVertices = {nw, ne, sw, se};
                    vertices.AddRange(tileVertices);
                    
                    //Triangles
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 3);
                    triangles.Add(vertexIndex + 2);

                    bool isEdgeTile = x == 0 || x == numTilesPerLine - 1 || y == 0 || y == numTilesPerLine;
                    if(isLandTile || isEdgeTile){
                        for(int i=0; i<nswe.Length; i++){
                            int neighbourX = x + (int)nswe[i].x;
                            int neighbourY = y + (int)nswe[i].y;
                            bool neighbourIsOutOfBounds = neighbourX < 0 || neighbourX >= numTilesPerLine || neighbourY < 0 || neighbourY >= numTilesPerLine;
                            bool neighbourIsWater = false;
                            if(!neighbourIsOutOfBounds){
                                float neighbourHeight = heightMap[neighbourX, neighbourY];
                                neighbourIsWater = neighbourHeight <= regions[0].height;
                            }

                            if(neighbourIsOutOfBounds || (isLandTile && neighbourIsWater)){
                                float depth = waterDepth;
                                if(neighbourIsOutOfBounds){
                                    depth = (isWaterTile) ? edgeDepth : edgeDepth + waterDepth;
                                }
                                vertexIndex = vertices.Count;
                                int edgeVertIndexA = sideVertIndexByDir[i][0];
                                int edgeVertIndexB = sideVertIndexByDir[i][1];
                                vertices.Add(tileVertices[edgeVertIndexA]);
                                vertices.Add(tileVertices[edgeVertIndexA] + Vector3.down * depth);
                                vertices.Add(tileVertices[edgeVertIndexB]);
                                vertices.Add(tileVertices[edgeVertIndexB] + Vector3.down * depth);

                                uvs.AddRange(new Vector2[] {uv, uv, uv, uv,});
                                int[] sideTriIndices = {vertexIndex, vertexIndex + 1, vertexIndex + 2, vertexIndex + 1, vertexIndex + 3, vertexIndex + 2};
                                triangles.AddRange(sideTriIndices);
                            }

                        }
                    }
                
                meshData.walkable[x, y] = isLandTile;
                meshData.tileCenters[x, y] = nw + new Vector3(0.5f, 0, -0.5f); 
                }
            }
            meshData.Add(vertices.ToArray(), triangles.ToArray(), uvs.ToArray());
            return meshData;
        }

        static Vector2 RegionUv(float height, MapGenerator.TerrainType[] regions){
            int regionIndex = 0;
            float regionStartHeight = 0;
            for(int i=0; i<regions.Length; i++){
                if(height <= regions[i].height){
                    regionIndex = i;
                    break;
                }
                regionStartHeight = regions[i].height;
            }

            MapGenerator.TerrainType region = regions[regionIndex];
            float sample = Mathf.InverseLerp(regionStartHeight, region.height, height);
            sample = (int)(sample * region.Steps) / (float)Mathf.Max(region.Steps, 1);

            Vector2 uv = new Vector2(regionIndex, sample);
            return uv;
        }
    }


    public class MeshData{

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;
        public Vector3[, ] tileCenters;
        public bool[, ] walkable;

        public MeshData(int mapSize){
            tileCenters = new Vector3[mapSize, mapSize];
            walkable = new bool[mapSize, mapSize];
        }

        public void Add(Vector3[] vertices, int[] triangles, Vector2[] uvs){
            this.vertices = vertices;
            this.triangles = triangles;
            this.uvs = uvs;
        }

        public Mesh CreateMesh(){
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}