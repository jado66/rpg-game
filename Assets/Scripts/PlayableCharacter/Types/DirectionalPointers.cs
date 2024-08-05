using UnityEngine;

public static class DirectionalPointers
{
    public static readonly Vector3Int[] Pointers = new Vector3Int[]
    {
        new Vector3Int(0, 1, 0),  // North
        new Vector3Int(1, 0, 0),  // East
        new Vector3Int(0, -1, 0), // South
        new Vector3Int(-1, 0, 0)  // West
    };
}