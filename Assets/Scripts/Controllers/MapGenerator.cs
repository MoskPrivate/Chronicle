using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    #region newMapGen
    public enum DrawMode { NoiseMap, ColourMap };
    public DrawMode drawMode;
    public int width;
    public int height;
    public float noiseScale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offSet;
    //List<MapSquare> mapSquares = new List<MapSquare>();

    public float range;
    #endregion


   
    #region oldMapGen
   /* public const int chunkSize = 5;
    public int width;
    public int height;
    public string seed;
    public int seedNum = 10;
    public bool useRandomSeed;
    public int edgeThreshold;
    public int treeCount;
    public int rockCount;
    public LayerMask mask;
    public GameObject rock;
    public GameObject tree;
    public GameObject player;
    public GrassGenerator grassGen;
    public EnvironmentGenerator envGen;
    List<Vector3> spawnAblePositions;
    Queue<MapSquare> shuffledPositions;
    GameObject oldEnv;


    [Range(0, 100)]
    public int randomFillPercent;

    int[,] map;*/
    #endregion
    void Awake()
    {
        GenerateMap();
        /*spawnAblePositions = new List<Vector3>();
        mapSquares = new List<MapSquare>();*/
        
    }
    void Start()
    {
        //GenerateMap();
        

    }
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height,seed, noiseScale, octaves,persistance,lacunarity,offSet);
        Color[] colourMap = new Color[width * height];

        MapDisplay display = GetComponent<MapDisplay>();
        int borderSize = 3;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    float currentHeight = noiseMap[x, y];
                    MapSquare square = new MapSquare(new Vector3(x, 0, y), x, y, false);
                    if (currentHeight <= range)
                    {
                        colourMap[y * width + x] = Color.black;
                    }
                    else
                    {
                        colourMap[y * width + x] = Color.white;
                        square.isFree = true;
                    }
                }
                else
                {

                }
            }
        }
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
           display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, width, height));
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))         
        {
            //GenerateMap();
        }
    }
    #region oldMapGen
  /*  void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
        //MeshGenerator meshGen = GetComponent<MeshGenerator>();
        //meshGen.GenerateMesh(map, 1);
      //  meshGen.CalculateSpawnAbleArea();

    }
    void RandomFillMap()
    {
        if(useRandomSeed)
        {
            float randomNumber = 1/UnityEngine.Random.Range(0f, 1f);
            seed = randomNumber.ToString();
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x >= 0 && x <= edgeThreshold || x<= width-1 && x >= width - edgeThreshold || y >= 0 && y <= edgeThreshold || y <= height - 1 && y >= height - edgeThreshold)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 0 : 1;
                }
            }
        }
    }
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int NeighbourCount = GetNeighbourAirCount(x,y);
                if(NeighbourCount > 4)
                    map[x, y] = 1;
                else if(NeighbourCount <4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }
    int GetNeighbourAirCount(int posX, int posY)
    {
        int airCount = 0;
        for (int neighbourX = posX-1; neighbourX <= posX+1; neighbourX++)
        {
            for (int neighbourY = posY - 1; neighbourY <= posY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != posX || neighbourY != posY)
                    {
                        airCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    airCount++;
                }
                
            }
        }
        return airCount;
    }
    

    public void GetSpawnAbleList(MapSquare square)
    {

        spawnAblePositions.Add(square.position);
        mapSquares.Add(square);
        
    }
    public void ResetSpawnAbleList()
    {
        spawnAblePositions.Clear();
        mapSquares.Clear();
        SpawnPlayer();
    }
    public void GenerateObjects()
    {

        if (oldEnv != null)
        {

            Destroy(oldEnv);
        }
        shuffledPositions = new Queue<MapSquare>(Utility.ShuffleArray(mapSquares.ToArray(), 0));
        GameObject environment = new GameObject();
        environment.name = "Environment";
        oldEnv = environment;
        envGen.GenerateObjects(mapSquares,environment);

        //GenerateTrees(environment);
       // GenerateRocks(environment);
       // grassGen.GetPositions(mapSquares);

        
    }
    void SpawnPlayer()
    {
        //TODO: Player can spawn inside objects
        Vector3 spawnPlace = coordToPos(GetRandomVector3());
        player.transform.position = new Vector3(spawnPlace.x,1f,spawnPlace.z);
    }
    public Vector3 GetRandomVector3()
    {
        MapSquare randomPos = shuffledPositions.Dequeue();
        shuffledPositions.Enqueue(randomPos);
        mapSquares.Remove(randomPos);
        return randomPos.position;
    }
    Vector3 coordToPos(Vector3 vector)
    {
        Vector3 coord = new Vector3( 0.5f + vector.x, vector.y, 0.5f + vector.z);
        return coord;
    }*/
    #endregion
    /*void InstantiateMap()
    {
        if(map!= null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject instantiableObject = (map[x, y] == 1) ? waterTileObject : tileObject;
                    Vector3 position = transform.position + new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    GameObject tile = Instantiate(instantiableObject, position,Quaternion.identity) as GameObject;
                    tile.transform.parent = gameObject.transform;
                }
            }
        }
        
    }*/
}


