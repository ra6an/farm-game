using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReadController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap cropsTilemap;
    public CropsManager cropsManager;
    public PlaceableObjectsReferenceManager objectsManager;

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        if(tilemap == null)
        {
            tilemap = GameObject.Find("BaseTilemap")?.GetComponent<Tilemap>();
        }

        if (tilemap == null) return Vector3Int.zero;

        Vector3 worldPosition;

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);

        return gridPosition;
    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        if (tilemap == null)
        {
            tilemap = GameObject.Find("BaseTilemap")?.GetComponent<Tilemap>();
        }

        if (tilemap == null) return null;

        TileBase tile = tilemap.GetTile(gridPosition);

        return tile;
    }

    public TileBase GetCropTileBase(Vector3Int gridPosition)
    {
        if (cropsTilemap == null)
        {
            cropsTilemap = GameObject.Find("CropsTilemap")?.GetComponent<Tilemap>();
        }

        if(cropsTilemap == null) return null;

        TileBase tile = cropsTilemap.GetTile(gridPosition);

        return tile;
    }
}
