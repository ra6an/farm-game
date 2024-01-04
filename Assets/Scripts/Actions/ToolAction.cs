using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAction : ScriptableObject
{
    public virtual bool OnApply(Vector2 worldPoint, int dmg)
    {
        Debug.LogWarning("OnApply implemented");
        return true;
    }

    public virtual bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        Debug.LogWarning("OnApplyToTileMap is not implemented!");
        return true;
    }

    public virtual bool OnApplyToTileMapMultiple(List<Vector3Int> gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        Debug.LogWarning("OnApplyToTileMap is not implemented!");
        return true;
    }

    public virtual void OnItemUsed(Item usedItem, ItemContainer Inventory)
    {
        
    }
}
