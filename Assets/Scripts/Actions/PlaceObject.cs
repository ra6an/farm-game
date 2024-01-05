using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Tool Action/Place Object")]
public class PlaceObject : ToolAction
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
            List<Vector3Int> positions = CreateListOfPositions(gridPosition, item.width, item.height);

            if (tileMapReadController.objectsManager.Check(positions)) { return false; }
            //Debug.Log(tileMapReadController.objectsManager.Check(positions)); OVO JE DOBRO... CHECK RADI
            tileMapReadController.objectsManager.Place(item, positions);
            return true;
    }

    private List<Vector3Int> CreateListOfPositions(Vector3Int startPosition, int width, int height)
    {
        List<Vector3Int> list = new List<Vector3Int>();
        

        int heightHelper = 0;

        while(heightHelper < height)
        {
            for (int i = 0; i < width; i++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + i, startPosition.y + heightHelper, startPosition.z);
                list.Add(position);
            }

            heightHelper++;
        }
        return list;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer Inventory)
    {
        Inventory.Remove(usedItem);
    }
}
