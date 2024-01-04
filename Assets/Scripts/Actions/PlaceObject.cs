using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Tool Action/Place Object")]
public class PlaceObject : ToolAction
{
    //public override bool OnApplyToTileMapMultiple(List<Vector3Int> gridPosition, TileMapReadController tileMapReadController, Item item)
    //{
    //    if(tileMapReadController.objectsManager.Check(gridPosition))
    //    {
    //        return false;
    //    }

    //    tileMapReadController.objectsManager.Place(item, gridPosition);
    //    return true;
    //}

    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
        List<Vector3Int> posOnGridList = new List<Vector3Int>();
        if(!item.isLarge)
        {
            posOnGridList.Add(gridPosition);

            if (tileMapReadController.objectsManager.Check(posOnGridList))
            {
                return false;
            }

            tileMapReadController.objectsManager.Place(item, posOnGridList);
            return true;
        }

        if(item.isLarge)
        {
            List<Vector3Int> positions = CreateListOfPositions(gridPosition, item.width, item.height);

            if (tileMapReadController.objectsManager.Check(positions)) { return false; }
            //Debug.Log(tileMapReadController.objectsManager.Check(positions)); OVO JE DOBRO... CHECK RADI
            tileMapReadController.objectsManager.Place(item, positions);
            return true;
        }

        return false;
    }

    private List<Vector3Int> CreateListOfPositions(Vector3Int startPosition, int width, int height)
    {
        List<Vector3Int> list = new List<Vector3Int>();
        //list.Add(startPosition);
        

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
        Debug.Log(startPosition + ", " + list[0] + list[1]);
        return list;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer Inventory)
    {
        Inventory.Remove(usedItem);
    }
}
