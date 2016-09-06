using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrassGenerator : MonoBehaviour {

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    MapSquare[,] map;
    public float grassPlaneLenght;
    public float grassPlaneHeight;
    int grassChunkSizeX;
    int grassChunkSizeY;
    public int grassChunkSize;
    public MeshGenerator gen;
    public MapGenerator mapGen;
    int index = 0;
    bool isSecond = false;
    int k = 0;
    void Start()
    {
        grassChunkSizeX = grassChunkSize;
        grassChunkSizeY = grassChunkSize;
    }
    public void GetPositions(List<MapSquare> positions)
    {
        GenerateGrassMesh(positions);
    }
    void Reset()
    {
        vertices.Clear();
        triangles.Clear();
        for (int x = 0; x < mapGen.width; x++)
        {
            for (int y = 0; y < mapGen.height; y++)
            {
                map[x, y] = new MapSquare(Vector3.zero, x, y, false);
            }
        }

        //TODO: Destroy the grass gameObject
    }
    void GenerateGrassMesh(List<MapSquare> positions)
    {
        /*int posX = 1;
        int posY = 1;*/
        
        map = new MapSquare[mapGen.width, mapGen.height];

        float margin = (1 - grassPlaneLenght)/ 2;
        float heightMargin = (1 - grassPlaneHeight) / 2;
        Reset();

        float chunkSizeXFloat = (mapGen.width * 1.0f) / grassChunkSize;
        float chunkSizeYFloat = (mapGen.height * 1.0f) / grassChunkSize;
        int chunkSizeX = Mathf.RoundToInt(Mathf.Ceil(chunkSizeXFloat));
        int chunkSizeY = Mathf.RoundToInt(Mathf.Ceil(chunkSizeYFloat));

        /*  int modChunkX = mapGen.width % grassChunkSize;
          int modChunkY = mapGen.height % grassChunkSize;*/

        // Loop and project positions on the map;

        for (int i = 0; i < positions.Count; i++)
        {
            map[positions[i].x, positions[i].y].isFree = true;
            map[positions[i].x, positions[i].y].position = positions[i].position;
        }
        int config = 0;
        
        //Calculate vertices
        for (int i = 0; i < chunkSizeX ; i++)
        {
            for (int j= 0; j < chunkSizeY ; j++)
            {
                List<Vector3> vertex = new List<Vector3>();
                List<int> tris = new List<int>();
                vertex.Clear();
                tris.Clear();
                if (i == chunkSizeX - 1 && j != chunkSizeY - 1)
                {
                    config = 1;
                    
                }
                else if(j == chunkSizeY - 1 && i != chunkSizeX - 1)
                {
                    config = 2;
                }
                else if(i == chunkSizeX - 1 && j == chunkSizeY - 1)
                {
                    config = 3;
                }
                else
                {
                    config = 0;
                    
                }
                switch (config){
                    case 0:
                        grassChunkSizeX = grassChunkSize;
                        grassChunkSizeY = grassChunkSize;
                        break;
                    case 1:
                        grassChunkSizeX = map.GetLength(1) - ((chunkSizeY - 1) * grassChunkSize);
                        grassChunkSizeY = grassChunkSize;
                        break;
                    case 2:
                        grassChunkSizeY = map.GetLength(0) - ((chunkSizeX - 1) * grassChunkSize);
                        //print(grassChunkSizeY);
                        grassChunkSizeX = grassChunkSize;
                        break;
                    case 3:
                         grassChunkSizeX = map.GetLength(1) - ((chunkSizeX - 1) * grassChunkSize);
                         grassChunkSizeY = map.GetLength(0) - ((chunkSizeY - 1 ) * grassChunkSize);
                        break;
                }
                for (int x= i * grassChunkSize ; x < i* grassChunkSize + grassChunkSizeX; x++)
                {
                    for (int y= j* grassChunkSize; y < j * grassChunkSize + grassChunkSizeY; y++)
                    {
                        if(map[x,y].isFree == true)
                        {
                             MapSquare n = map[x,y];
                             vertex.Add(new Vector3(n.position.x + margin, n.position.y + (1 - 2 * heightMargin), n.position.z));
                             vertex.Add(new Vector3(n.position.x + (1 - margin), n.position.y + (1 - 2 * heightMargin), n.position.z));
                             vertex.Add(new Vector3(n.position.x + (1 - margin), n.position.y, n.position.z));
                             vertex.Add(new Vector3(n.position.x + margin, n.position.y, n.position.z));
                        }
                        else
                        {

                        }
                    }
                }



                tris = CalculateTriangles(vertex);
                CreatePlane(vertex, tris);
                
                //Create Object
                //Apply Mesh

            }
            
        }
                
                //Calculate Triangles

                //How many times it has to triangulate
                
     }

            

    List<int> CalculateTriangles(List<Vector3> vertexToTri)
    {
        List<int> tris = new List<int>();
        tris.Clear();
        int amount = 0;
        if (vertexToTri.Count % 2 == 0)
        {
            amount = vertexToTri.Count * 3 / 2;
        }
        else
        {
            amount = Mathf.RoundToInt(Mathf.Floor(vertexToTri.Count * 3 / 2 - 1));

        }
        isSecond = false;
        k = 0;
        index = 0;
        for (int i = 0; i < amount; i++)
        {
            tris.Add(Triangulate(i));
        }
        
        return tris;
    }
    int Triangulate(int iteration)
    {
        if (iteration % 3 == 0 && index != 0)
        {
            isSecond = !isSecond;
            k = 0;
            if (isSecond)
            {
                index -= 3;
            }

        }
        if (isSecond && k == 1)
        {
            index++;
        }
        k++;
        index++;
        
        return index - 1 ;
    }
    void  CreatePlane(List<Vector3> meshVertices, List<int> meshTriangles)
            {
                GameObject grassObject = new GameObject("Grass");
                MeshFilter mf = grassObject.AddComponent<MeshFilter>();
               // MeshRenderer mr = grassObject.AddComponent<MeshRenderer>();
                Mesh mesh = new Mesh();
                mesh.Clear();



                mesh.vertices = meshVertices.ToArray();
                mesh.triangles = meshTriangles.ToArray();
                mf.mesh = mesh;
                
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
    }
}
