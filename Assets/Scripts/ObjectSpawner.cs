using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public enum NodeType
{
    Tree,
    AppleTree,
    PeachTree,
    PearTree,
    OrangeTree,
    Rock,
    MediumRock,
    SmallRock,
    BronzeOre,
    IronOre,
    GoldOre
}

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] float spawnArea_height = 1;
    [SerializeField] float spawnArea_width = 1;

    [SerializeField] NodeType[] spawn;
    [SerializeField] float probability = 0.1f;
    [SerializeField] int spawnCount = 0;
    [SerializeField] int objectSpawnLimit = -1;

    [SerializeField] bool oneTime = false;

    //List<SpawnedObject> spawnedObjects;
    int spawnedNodes;
    //[SerializeField] int idInList = -1;

    //REF FOR PLACEABLE OBJECTS
    [SerializeField] PlaceableObjectContainer placeableObjectContainer;
    [SerializeField] SpawnedNodeContainer spawnedNodeContainer;

    private Vector2 areaSize;
    private int spawnLength;
    public bool canCheck = false;
    public bool canSpawn = false;

    //TODO: 
    //NASTIMATI KAD BUDE NOVI DAN DA SE SVIM SPAWNERIMA RESETUJE CAN SPAWN KAKO BI O5 MOGLI SPAWNATI NODEOVE!!!

    private void Start()
    {
        areaSize = new Vector2(spawnArea_width * 2, spawnArea_height * 2);

        //CheckNumberOfSpawnItems();

        spawnLength = spawn.Length;

        if (!oneTime)
        {
            TimeAgent timeAgent = GetComponent<TimeAgent>();
            timeAgent.onTimeTick += Spawn;
        } else
        {
            Spawn();
        }
    }

    private void Update()
    {
        if (canCheck)
        {
            CheckNumberOfSpawnItems();
            canSpawn = true;
            canCheck = false;
        }
    }

    public void CheckNumberOfSpawnItems()
    {
        int numOfTrees = 0;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, areaSize, 0f);

        foreach (Collider2D collider in colliders)
        {
            if(System.Enum.IsDefined(typeof(NodeType), collider.tag)) numOfTrees++;
        }

        spawnedNodes = numOfTrees;
    }



    void Spawn()
    {
        if (!canSpawn) return;
        
        if (objectSpawnLimit <= spawnedNodes && objectSpawnLimit != -1) return;

        if (Random.value > probability) return;

        for (int i = 0; i < spawnCount; i++)
        {
            if (objectSpawnLimit == spawnedNodes) return;

            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            position.x += UnityEngine.Random.Range((int)-spawnArea_width, (int)spawnArea_width);
            position.y += UnityEngine.Random.Range((int)-spawnArea_height, (int)spawnArea_height);

            if (placeableObjectContainer.Get(position) != null) return;
            if (spawnedNodeContainer.Get(position) != null) return;

            int id = Random.Range(0, spawnLength);
            GameManager.instance.GetComponent<SpawnedNodesReferenceManager>().spawnedNodesManager.Spawn(spawn[id], position);

            spawnedNodes++;
        }

        if(oneTime) canSpawn = false;
    }
    public void SpawnedObjectDestroyed(SpawnedObject spawnedObject)
    {
        //spawnedObjects.Remove(spawnedObject);
    }

    //PERSISTING DATA
    public class ToSave
    {
        public List<SpawnedObject.SaveSpawnedObjectData> spawnedObjectDatas;

        public ToSave()
        {
            spawnedObjectDatas = new List<SpawnedObject.SaveSpawnedObjectData>();
        }
    }

    string Read()
    {
        //ToSave toSave = new ToSave();

        //for (int i = 0; i < spawnedObjects.Count; i++)
        //{
        //    if (spawnedObjects[i] != null) 
        //    {
        //        toSave.spawnedObjectDatas.Add(
        //        new SpawnedObject.SaveSpawnedObjectData(
        //            spawnedObjects[i].objId,
        //            spawnedObjects[i].transform.position
        //            )
        //        );
        //    }
        //}

        //return JsonUtility.ToJson( toSave );
        return "";
    }

    public void Load(string json)
    {
        //if (json == "" || json == "{}" || json == null) return;

        //ToSave toLoad = JsonUtility.FromJson<ToSave>(json);

        //for (int i = 0; i < toLoad.spawnedObjectDatas.Count; i++)
        //{
        //    SpawnedObject.SaveSpawnedObjectData data = toLoad.spawnedObjectDatas[i];
        //    GameObject go = Instantiate(spawn[data.objectId]);
        //    go.transform.position = data.worldPosition;
        //    go.transform.SetParent(transform);
        //    SpawnedObject so = go.AddComponent<SpawnedObject>();
        //    so.objId = data.objectId;
        //    spawnedObjects.Add(so);
        //}
    }

    private void OnDestroy()
    {
        if (oneTime) return;
        //SaveData();
    }

    //private bool CheckJSON()
    //{
    //    if (oneTime) return false;

    //    if (targetSaveJSONList == null)
    //    {
    //        Debug.LogError("Target JSON scriptable object to save data on spawnable object is null!");
    //        return false;
    //    }

    //    if (idInList == -1)
    //    {
    //        Debug.LogError("Id in list is not assigned, data can't be saved!");
    //        return false;
    //    }

    //    return true;
    //}

    //private void SaveData()
    //{
    //    if (!CheckJSON()) return;

    //    string jsonString = Read();
    //    Debug.Log(idInList);
    //    targetSaveJSONList.SetString(jsonString, idInList);
    //}

    //private void LoadData()
    //{
    //    if (!CheckJSON()) return;

    //    Load(targetSaveJSONList.GetString(idInList));
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea_width * 2, spawnArea_height * 2));
    }
}
