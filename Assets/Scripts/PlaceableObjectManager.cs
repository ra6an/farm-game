using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class PlaceableObjectManager : MonoBehaviour, IDataPersistant
{
    [SerializeField] string sceneName;
    [SerializeField] PlaceableObjectContainer placeableObjects;
    [SerializeField] SpawnedNodeContainer spawnedNodes;
    [SerializeField] Tilemap targetTilemap;

    public bool oneTimeLoader = false;
    private bool isLoaded = true;

    private void Start()
    {
        if(placeableObjects == null)
        {
            placeableObjects = (PlaceableObjectContainer)ScriptableObject.CreateInstance(typeof(PlaceableObjectContainer));
            placeableObjects.Init();
        }
        GameManager.instance.GetComponent<PlaceableObjectsReferenceManager>().placeableObjectManager = this;
        VisualizeMap();
    }

    private void Update()
    {
        if (isLoaded) LoadData(DataPersistentManager.instance.gameData);
        isLoaded = false;
    }

    private void OnDestroy()
    {
        for(int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            if (placeableObjects.placeableObjects[i].targetObject == null) { continue; }

            IPersistant persistant = placeableObjects.placeableObjects[i].targetObject.GetComponent<IPersistant>();

            if(persistant != null)
            {
                string jsonString = persistant.SaveData();
                placeableObjects.placeableObjects[i].objectState = jsonString;
            }

            placeableObjects.placeableObjects[i].targetObject = null;
        }
    }

    private void VisualizeMap()
    {
        for(int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            VisualizeItem(placeableObjects.placeableObjects[i]);
        }
    }

    internal void PickUp(Vector3Int gridPosition)
    {
        List<Vector3Int> gridPosList = new List<Vector3Int>();
        gridPosList.Add(gridPosition);
        PlaceableObject placedObject = (PlaceableObject)placeableObjects.Get(gridPosList);

        if(placedObject == null)
        {
            return;
        }

        Item item = GameManager.instance.itemsDB.GetItemById(placedObject.placedItem);
        if (item == null) return;
        
        ItemSpawnManager.instance.SpawnItem
        (
        targetTilemap.CellToWorld(gridPosition) + targetTilemap.cellSize / 2,
        item, 1
        );

        Destroy(placedObject.targetObject.gameObject);

        placeableObjects.Remove(placedObject);
    }
    
    private void VisualizeItem(PlaceableObject placeableObject)
    {
        Item item = GameManager.instance.itemsDB.GetItemById(placeableObject.placedItem);
        if (item == null) return;

        GameObject go = Instantiate(item.itemPrefab);
        go.transform.parent = transform;

        int posOnGridX = placeableObject.positionOnGrid[0].x;
        int posOnGridY = placeableObject.positionOnGrid[0].y;
        int itemWidth = item.width;
        int itemHeight = item.height;
        float xPosition = 0f;
        float yPosition = 0f;
        float xAddition = 0f;
        float yAddition = 0f;

        if (itemWidth == 1) 
        {
            xPosition = posOnGridX;
            xAddition = 0.5f;
        } 
        else if (itemWidth > 1 && itemWidth % 2 != 0)
        {
            xPosition = posOnGridX + ((float)itemWidth / 2 + 0.5f);
            xAddition = -0.5f;
        } 
        else if(itemWidth > 1 && itemWidth % 2 == 0)
        {
            xPosition = (posOnGridX + (float)itemWidth / 2);
            xAddition = 0f;
        }

        if(itemHeight == 1)
        {
            yPosition = posOnGridY;
            yAddition = 0.5f;
        }
        else if(itemHeight > 1 && itemHeight % 2 != 0)
        {
            yPosition = posOnGridY + ((float)itemHeight / 2 + 0.5f);
            yAddition = -0.5f;
        }
        else if (itemHeight > 1 && itemHeight % 2 == 0)
        {
            Debug.Log("Height: 2");
            yPosition = (posOnGridY + (float)itemHeight / 2);
            yAddition = 0f;
        }

        Vector3Int posForWorld = new Vector3Int(
            (int)xPosition, (int)yPosition, placeableObject.positionOnGrid[0].z
            );

        Vector3 position = targetTilemap.CellToWorld(posForWorld) + new Vector3(xAddition, yAddition, 0);
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistant persistant = go.GetComponent<IPersistant>();
        if (persistant != null)
        {
            persistant.LoadData(placeableObject.objectState);
        }

        placeableObject.targetObject = go.transform;
    }

    public bool Check(List<Vector3Int> positions)
    {
        return placeableObjects.Get(positions) != null || spawnedNodes.Get(positions) != null;
    }

    public void Place(Item item, List<Vector3Int> positionOnGrid)
    {
        if (Check(positionOnGrid)) return;

        //Item item = GameManager.instance.itemsDB.GetItemById(placedObject.placedItem);
        //if (item == null) return;
        int itemId = GameManager.instance.itemsDB.GetItemId(item);
        if (itemId == -1) return;

        PlaceableObject placeableObject = new(itemId, positionOnGrid);
        VisualizeItem(placeableObject);
        placeableObjects.placeableObjects.Add(placeableObject);
        
    }
    // SREDITI MALO KOD SA PARAMETRIMA IZ FUNKCIJE

    [Serializable]
    public class PlacedObjectToSave
    {
        public List<PlaceableObject> placedObjects;

        public void Init()
        {
            placedObjects = new List<PlaceableObject>();
        }
    }

    public void SaveData(GameData data)
    {

        Debug.Log("Savea placeable objects");
        PlaceableObjects po = new();
        po.sceneName = sceneName;

        PlacedObjectToSave copyOfPlacedObjects = new();
        copyOfPlacedObjects.Init();

        foreach (PlaceableObject copyOfObject in placeableObjects.placeableObjects)
        {
            string objectStateJson = "";
            IPersistant persistent = copyOfObject.targetObject.GetComponent<IPersistant>();
            if (persistent != null) objectStateJson = persistent.SaveData();

            PlaceableObject placedObject = new(copyOfObject.placedItem, copyOfObject.positionOnGrid);
            placedObject.objectState = objectStateJson;
            copyOfPlacedObjects.placedObjects.Add(placedObject);
        }

        string serializedContainer = JsonUtility.ToJson(copyOfPlacedObjects);

        po.container = serializedContainer;

        bool doesExists = false;

        foreach (PlaceableObjects gameDataPO in data.placeableObjectsContainers)
        {
            if(gameDataPO.sceneName == sceneName)
            {
                doesExists = true;
                gameDataPO.container = serializedContainer;
                break;
            }
        }

        if(!doesExists)
        {
            data.placeableObjectsContainers.Add(po);
        }
    }

    public void LoadData(GameData data)
    {
        Debug.Log("Loada placeable objects");
        placeableObjects = (PlaceableObjectContainer)ScriptableObject.CreateInstance(typeof(PlaceableObjectContainer));
        placeableObjects.Init();
        string jsonPlacedObjects = "";

        foreach(PlaceableObjects po in data.placeableObjectsContainers)
        {
            if(po.sceneName == sceneName) jsonPlacedObjects = po.container;
        }

        if (jsonPlacedObjects == "" || jsonPlacedObjects == "{}" || jsonPlacedObjects == null) return;

        PlacedObjectToSave deserializedPlacedObjects = JsonUtility.FromJson<PlacedObjectToSave>(jsonPlacedObjects);

        foreach(PlaceableObject po in deserializedPlacedObjects.placedObjects)
        {
            PlaceableObject objectToLoad = new(po.placedItem, po.positionOnGrid);
            objectToLoad.objectState = po.objectState;
            placeableObjects.placeableObjects.Add(objectToLoad);
        }

        VisualizeMap();
    }

    public bool isOneTimeLoader()
    {
        return oneTimeLoader;
    }
}
