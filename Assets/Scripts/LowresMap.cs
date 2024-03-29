using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LowresMap : MonoBehaviour
{
    // Lowres textures
    public static LowresMap active;
    public static Tilemap lowresMap;
    public static TileBase lowresTile;
    public Tilemap _lowresMap;
    public TileBase _lowresTile;
    public static bool useLowresMap;

    // Get active instance
    public void Awake() { active = this; }

    // Start is called before the first frame update
    public void Setup()
    {
        // Check if lowres being used
        useLowresMap = _lowresMap != null && _lowresTile != null;

        // Set lowres
        lowresMap = _lowresMap;
        lowresTile = _lowresTile;
    }

    // Adds a lowres tile to the map
    public static void AddLowres(Vector2Int coords, Color color)
    {
        if (!useLowresMap) return;

        Vector3Int adjustedCoords = new Vector3Int(coords.x / 5, coords.y / 5, 0);
        lowresMap.SetTile(adjustedCoords, lowresTile);
        lowresMap.SetTileFlags(adjustedCoords, TileFlags.None);
        lowresMap.SetColor(adjustedCoords, color);
    }

    // Removes a lowres tile from the map
    public static void RemoveLowres(Vector2Int coords)
    {
        if (!useLowresMap) return;

        Vector3Int adjustedCoords = new Vector3Int(coords.x / 5, coords.y / 5, 0);
        lowresMap.SetTile(adjustedCoords, null);
    }
}
