using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour {

    public const float maxViewDst = 30;
    public int seed = 3;
    public Transform viewer;
    public Material groundMat;
    private static EndlessTerrain _endlessTerrain;
    public static EndlessTerrain endlessTerrain
    {
        get;private set;
    }

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksVisible;
    GameObject meshObject;
    

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
    
    void Awake()
    {
        if(endlessTerrain != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        endlessTerrain = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        chunkSize = 31;
        chunksVisible = Mathf.RoundToInt(maxViewDst / chunkSize);
    }
    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }
    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            if (!terrainChunksVisibleLastUpdate[i].IsVisible())
            {
                terrainChunksVisibleLastUpdate[i].SetVisible(false);
            }

        }
        terrainChunksVisibleLastUpdate.Clear();

        int chunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int chunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int y = -chunksVisible; y <= chunksVisible; y++)
        {
            for (int x = -chunksVisible; x <= chunksVisible; x++)
            {
                Vector2 viewedChunkCoord = new Vector2(chunkCoordX + x, chunkCoordY + y);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    

                    if (terrainChunkDictionary[viewedChunkCoord].IsObjectVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,chunkSize));
                }
            }
        }
        
    }

    public class TerrainChunk {

        public GameObject prefab;
        GameObject meshObject;
        GameObject wallObject;
        GameObject environment;
        Vector2 position;
        Bounds bounds;

        public TerrainChunk(Vector2 coord,int size)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size );
            Vector3 position3 = new Vector3(position.x, 0, position.y);
            //Create object

            meshObject = new GameObject();
            meshObject.transform.position = position3;
            wallObject = new GameObject();
            wallObject.transform.position = meshObject.transform.position;
            wallObject.transform.SetParent(meshObject.transform);
            
            meshObject.AddComponent<MeshRenderer>();
            meshObject.AddComponent<MeshFilter>();
            meshObject.AddComponent<MeshCollider>();
            meshObject.name = "Chunk";
            MeshGenerator meshGen = meshObject.AddComponent<MeshGenerator>();
            Chunk createdChunk = meshObject.AddComponent<Chunk>();

            wallObject.name = "Extrusion";
            wallObject.AddComponent<MeshRenderer>();
            wallObject.AddComponent<MeshFilter>();
            createdChunk.CreateInstance(coord);
            

           /* meshObject = Instantiate(prefab);
            meshObject.transform.position = position3;
            meshObject.transform.localScale = Vector3.one * size / 10f;*/
            SetVisible(false);
        }
        
        public void UpdateTerrainChunk()
        {
            float viewerDst = bounds.SqrDistance(viewerPosition);
            bool visible = viewerDst <= maxViewDst * maxViewDst;
            if (meshObject.activeSelf == visible)
            {

            }
            else
            {
                SetVisible(visible);
            }
            
        }
        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
            
        }
        public bool IsObjectVisible()
        {
            return meshObject.activeSelf;
        }
        public bool IsVisible()
        {
            float viewerDst = bounds.SqrDistance(viewerPosition);
            bool visible = viewerDst <= maxViewDst * maxViewDst;
            return visible;
        }
    }
}

