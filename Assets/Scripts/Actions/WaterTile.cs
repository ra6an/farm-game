using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tool Action/Water")]
public class WaterTile : ToolAction
{
    [SerializeField] List<TileBase> canWater;
    [SerializeField] AudioClip onWaterTile;

    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        TileBase tileToWater = tileMapReadController.GetCropTileBase(gridPosition);
        Debug.Log(tileToWater);

        if (!canWater.Contains(tileToWater))
        {
            Debug.Log(canWater.Contains(tileToWater));
            return false;
        }

        tileMapReadController.cropsManager.Water(gridPosition);

        if(onWaterTile != null)
        {
            AudioManager.instance.Play(onWaterTile);
        }

        return true;
    }
}
