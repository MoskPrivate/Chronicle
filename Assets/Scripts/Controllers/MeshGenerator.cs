using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    GameObject walls;
    GameObject wallCollider;
    MeshCollider meshCollider;
    GrassGenerator grassGen;
    //MapGenerator gen;
    List<Vector3> placeAbleSquares;
    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();

    void Awake()
    {

    }
    public void GenerateMesh(int[,] map, float squareSize)
    {
        outlines.Clear();
        checkedVertices.Clear();


        squareGrid = new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        Material mat = new Material(Shader.Find("Standard"));
        Material wallMat = new Material(Shader.Find("Standard"));
        mat.color = new Color32(97,120,45,255);
        wallMat.color = new Color32(97, 120, 45, 255);
        //mat.color = new Color32(0, 0, 0, 255);

        if (gameObject.transform.childCount > 0)
        {
            walls = gameObject.transform.GetChild(0).gameObject;
            walls.GetComponent<MeshRenderer>().material = wallMat;

        }
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        
        //meshCollider.sharedMesh = mesh;
        //meshCollider.enabled = true;
        CreateWallMesh();
    }
    void CreateWallMesh()
    {
        CalculateMeshOutlines();
        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();
        Mesh wallColMesh = new Mesh();
        float wallHeight = 4;

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;
                wallVertices.Add(vertices[outline[i]]); //top left vertex
                wallVertices.Add(vertices[outline[i + 1]]); //top right vertex
                wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); //bottom left vertex
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); //bottom right vertex

                wallTriangles.Add(startIndex + 0);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);
            }
        }
        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        walls.GetComponent<MeshFilter>().mesh = wallMesh;
        
        wallColMesh.vertices = wallVertices.ToArray();
        wallColMesh.triangles = wallTriangles.ToArray();
        Vector3[] normals = wallColMesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        for (int m = 0; m < wallColMesh.subMeshCount; m++)
        {
            int[] triangles = wallColMesh.GetTriangles(m);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i + 0];
                triangles[i + 0] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            wallColMesh.SetTriangles(triangles, m);
        }
        //wallColMesh.normals = normals;
       // MeshCollider collider = wallCollider.gameObject.GetComponent<MeshCollider>();
       // collider.sharedMesh = wallColMesh;


    }
    void OnDrawGizmos()
    {
        /*foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                Vector3 pos2 = vertices[outline[i]]; //top left vertex
                Vector3 pos1 = vertices[outline[i + 1]];//top right vertex
                Gizmos.DrawWireSphere(pos1, 0.2f);
                Gizmos.DrawWireSphere(pos2, 0.2f);
            }
        }*/
    }
    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            //0 points
            case 0:
                break;

            //1 points
            case 1:
                // MeshFromPoints(square.bottomLeft, square.centreLeft, square.centreBottom);
                MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                break;
            case 2:
                MeshFromPoints(square.centreBottom, square.centreRight, square.bottomRight);
                break;
            case 4:
                MeshFromPoints(square.centreRight, square.centreTop, square.topRight);
                break;
            case 8:
                MeshFromPoints(square.centreTop, square.centreLeft, square.topLeft);
                break;

            //2 points
            case 3:
                MeshFromPoints(square.centreLeft, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 6:
                MeshFromPoints(square.centreBottom, square.centreTop, square.topRight, square.bottomRight);
                break;
            case 9:
                MeshFromPoints(square.centreTop, square.centreBottom, square.bottomLeft, square.topLeft);
                break;
            case 12:
                MeshFromPoints(square.centreRight, square.centreLeft, square.topLeft, square.topRight);
                break;
            case 5:
                MeshFromPoints(square.centreLeft, square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                break;
            case 10:
                MeshFromPoints(square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.bottomLeft, square.topLeft);
                break;

            //3 points
            case 7:
                MeshFromPoints(square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft, square.centreTop);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                MeshFromPoints(square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.topLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            //4 points
            case 15:
                MeshFromPoints(square.topRight, square.bottomRight, square.bottomLeft, square.topLeft);
               /* checkedVertices.Add(square.topLeft.vertexIndex);
                checkedVertices.Add(square.topRight.vertexIndex);
                checkedVertices.Add(square.bottomRight.vertexIndex);
                checkedVertices.Add(square.bottomLeft.vertexIndex);*/
                break;
        }
    }
    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);

    }
    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }

        }
    }
    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }
    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (triangleDictionary.ContainsKey(vertexIndexKey))
        {
            triangleDictionary[vertexIndexKey].Add(triangle);
        }
        else
        {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangleList);
        }
    }
    void CalculateMeshOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }
    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        bool isEnclosed = false;
        int startVertex = -1;
        if(outlines[outlineIndex].Count < 1)
        {
            startVertex = vertexIndex;
        }
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);
        if (nextVertexIndex != -1)
        {
            if(nextVertexIndex == startVertex)
            {
                isEnclosed = true;
            }
            FollowOutline(nextVertexIndex, outlineIndex);
        }
        else
        {
            if (!isEnclosed)
            {
                
                //outlines[outlineIndex].Reverse();
            }
            else
            {
               // outlines[outlineIndex].Reverse();
            }
            
        }
    }
    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];
        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];
                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    if (isOutlineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }

            }
        }
        return -1;
    }
    bool isOutlineEdge(int VertexA, int VertexB)
    {
        List<Triangle> trianglesContainingVertexA = triangleDictionary[VertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(VertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }
    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        int[] vertices;

        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;
            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }
        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }
    /*public void CalculateSpawnAbleArea()
    {
        List<Vector3> listToReturn = new List<Vector3>();
        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                if (squareGrid.squares[x, y].spawnAble)
                {
                    gen.GetSpawnAbleList(new MapSquare( squareGrid.squares[x, y].squarePos,x, y,true));
                    listToReturn.Add(squareGrid.squares[x, y].squarePos);

                }
            }
        }
        gen.GenerateObjects();
        gen.ResetSpawnAbleList();

    }*/
    public List<Vector3> GetSpawnAblePositions()
    {
        List<Vector3> positions = new List<Vector3>();
        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                if (squareGrid.squares[x, y].spawnAble)
                {
                    positions.Add(squareGrid.squares[x, y].squarePos);

                }
            }
        }
        return positions;
    }
    /*void OnDrawGizmos()
    {
        if (squareGrid != null)
        {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    Gizmos.color = (squareGrid.squares[x, y].topLeft.active) ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.position, Vector3.one * .4f);

                    Gizmos.color = (squareGrid.squares[x, y].topRight.active) ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topRight.position, Vector3.one * .4f);

                    Gizmos.color = (squareGrid.squares[x, y].bottomRight.active) ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.position, Vector3.one * .4f);

                    Gizmos.color = (squareGrid.squares[x, y].bottomLeft.active) ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.position, Vector3.one * .4f);

                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreTop.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreRight.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreBottom.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreLeft.position, Vector3.one * .15f);

                }
            }
        }
    }*/
    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] _map, float _squareSize)
        {
            int nodeCountX = _map.GetLength(0);
            int nodeCountY = _map.GetLength(1);
            float mapWidth = nodeCountX * _squareSize;
            float mapHeight = nodeCountY * _squareSize;

            ControlNode[,] controlNode = new ControlNode[nodeCountX, nodeCountY];
            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * _squareSize + _squareSize / 2, 0, -mapHeight / 2 + y * _squareSize + _squareSize / 2);
                    controlNode[x, y] = new ControlNode(pos, _map[x, y] == 1, _squareSize);
                }
            }
            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNode[x, y + 1], controlNode[x + 1, y + 1], controlNode[x + 1, y], controlNode[x, y]);
                }
            }
        }
    }

    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int configuration;
        public bool spawnAble;
        public bool isEdge;
        public Vector3 squarePos;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            squarePos = new Vector3(_topLeft.position.x + 0.5f, 0, topLeft.position.z - 0.5f);

            if (!topLeft.active)
            {
                configuration += 8;
            }
            if (!topRight.active)
            {
                configuration += 4;
            }
            if (!bottomRight.active)
            {
                configuration += 2;
            }
            if (!bottomLeft.active)
            {
                configuration += 1;
            }
            if (configuration == 15)
            {
                spawnAble = true;
            }
            else
            {
                spawnAble = false;
            }
        }
    }
    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _position)
        {
            position = _position;
        }
    }
    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _position, bool _active, float _SquareSize) : base(_position)
        {
            active = _active;
            above = new Node(position + Vector3.forward * _SquareSize / 2);
            right = new Node(position + Vector3.right * _SquareSize / 2);
        }
    }
}



