using UnityEngine;
using System.Collections;

public class Tile  {

    int x;
    int y;
    public enum TileType { Grass, Water };
    TileType tileType;
    Vector3 worldPosition;

    public Tile(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
