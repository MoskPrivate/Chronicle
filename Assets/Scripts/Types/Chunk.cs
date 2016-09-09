using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

    public static Material mat;
    Vector2 pos;
    int chunkSize = 32;
    int seed = 2;
    float scale = 16.82f;
    int octaves = 4;
    float persistance = 0.2f;
    float lacunarity = 2;
    public Vector2 offSet;
    Vector2 lastOffset;
    float range = 0.49f;
    List<MapSquare> mapSquares;
    Queue<MapSquare> shuffledMapSquares;

    int[,] map;

    public void CreateInstance(Vector2 _pos)
    {
        pos = _pos;
        offSet = new Vector2(pos.x * 31, pos.y * 31);
        seed = EndlessTerrain.endlessTerrain.seed;
        GenerateMap();
        
        lastOffset = offSet;
        
    }
    void Update()
    {
        if(offSet != lastOffset)
        {
            GenerateMap();
            lastOffset = offSet;
            print("hey");
        }
    }
	void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, seed, scale, octaves, persistance, lacunarity, offSet);
        map = new int[chunkSize,chunkSize];
        mapSquares = new List<MapSquare>();

        int borderSize = 0;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if (x >= borderSize && x < chunkSize - borderSize && y >= borderSize && y < chunkSize - borderSize)
                {
                    Vector3 squarePos = new Vector3(pos.x * 31 - 15.5f + x,0,pos.y * 31 - 15.5f + y);
                    float currentHeight = noiseMap[x, y];
                    if (currentHeight <= range)
                    {
                        //0 = solid
                        //1 = air
                        map[x, y] = 0;
                        MapSquare square = new MapSquare(squarePos, x, y, true);
                        mapSquares.Add(square);
                        //mapSquares.Add(new MapSquare(new Vector3(x, 0, y), x, y, true));
                    }
                    else
                    {
                        map[x, y] = 1;
                        //MapSquare square = new MapSquare(squarePos, x, y, false);
                    }
                }
                else
                {
                    map[x, y] = 0;
                }
                
            }
        }
        if(GetComponent<MeshGenerator>() != null)
        {
            GetComponent<MeshGenerator>().GenerateMesh(map, 1);
        }
        GenerateObjects(mapSquares);
    }
    void GenerateObjects(List<MapSquare> _objMap)
    {
        EnvironmentManager envSettings = EnvironmentManager.environmentManager;
        int settingSize = envSettings.staticEntList.Count;
        
        for (int i = 0; i < settingSize; i++)
        {
            float randomNumber = Random.Range(0, 1);
            int num = Mathf.RoundToInt(randomNumber * 1000);
            shuffledMapSquares = new Queue<MapSquare>(Utility.ShuffleArray(mapSquares.ToArray(), num));
            for (int j = 0; j < envSettings.staticEntList[i].quantity; j++)
            {
                MapSquare square = shuffledMapSquares.Dequeue();
                shuffledMapSquares.Enqueue(square);
                mapSquares.Remove(square);
                GameObject instObj = Instantiate(envSettings.staticEntList[i].prefab, square.position, Quaternion.identity) as GameObject;
                instObj.transform.SetParent(gameObject.transform);
            }
            
        }
        if (offSet == Vector2.zero)
        {
            shuffledMapSquares = new Queue<MapSquare>(Utility.ShuffleArray(mapSquares.ToArray(), seed));
            Vector3 spawnPos = shuffledMapSquares.Dequeue().position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(spawnPos.x,player.transform.position.y,spawnPos.z);
        }

    }

}
public class MapSquare
{
    public Vector3 position;

    public int x;
    public int y;
    public bool isFree = false;
    public MapSquare(Vector3 _position, int _x, int _y, bool _isFree)
    {
        position = _position;
        x = _x;
        y = _y;
        isFree = _isFree;
    }
}
