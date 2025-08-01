using UnityEngine;

public static class Extensions
{
    //turns a Vector2 into a Vector2Int
    public static Vector2Int ToInt(this Vector2 v2)
        => new Vector2Int((int)v2.x, (int)v2.y);

    //sets the Z value of a Vector3 to zero
    public static Vector3 XY(this Vector3 vector)
        => new Vector3(vector.x, vector.y);

    //sets the Z value of a Vector3 to zero
    public static Vector3 ToVector3(this Vector2Int vector)
        => new Vector3(vector.x, vector.y);

    //Vector3Int to Vector2Int
    public static Vector2Int ToVector2Int(this Vector3Int vector)
        => new Vector2Int(vector.x, vector.y);

    //Vector3Int to Vector2Int
    public static Vector3Int ToVector3Int(this Vector2Int vector)
        => new Vector3Int(vector.x, vector.y);

    public static readonly Vector2Int[] NeighbourPositions = new Vector2Int[6]
    {
        new Vector2Int(-1,  0),
        new Vector2Int(-1,  1),
        new Vector2Int( 0, -1),
        new Vector2Int( 0,  1),
        new Vector2Int( 1, -1),
        new Vector2Int( 1,  0),
    };
}