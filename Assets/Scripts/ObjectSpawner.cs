using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] float spawnArea_height = 1f;
    [SerializeField] float spawnArea_width = 1f;

    [SerializeField] GameObject[] spawn;
    [SerializeField] float probability = 0.1f;
    [SerializeField] int spawnCount = 1;
    [SerializeField] int objectSpawnLimit = -1;

    [SerializeField] bool oneTime = false;

    List<SpawnedObject> spawnedObjects;
    [SerializeField] JSONStringList targetSaveJSONList;
    [SerializeField] int idInList = -1;

    //REF FOR PLACEABLE OBJECTS
    [SerializeField] PlaceableObjectContainer placeableObjectContainer;

    private int spawnLength;

    private void Start()
    {
        spawnLength = spawn.Length;

        if (!oneTime)
        {
            TimeAgent timeAgent = GetComponent<TimeAgent>();
            timeAgent.onTimeTick += Spawn;
            spawnedObjects = new List<SpawnedObject>();

            LoadData();
        } else
        {
            Spawn();
            //Destroy(gameObject);
        }
    }

    void Spawn()
    {
        if (Random.value > probability) return;
        if (spawnedObjects != null)
        {
            if (objectSpawnLimit <= spawnedObjects.Count && objectSpawnLimit != -1) { return; }
        };

        for (int i = 0; i < spawnCount; i++)
        {
            int id = Random.Range(0, spawnLength);
            GameObject go = Instantiate(spawn[id]);
            Transform t = go.transform;
            t.SetParent(transform);

            //if (!oneTime)
            //{
            //    SpawnedObject spawnedObject = go.AddComponent<SpawnedObject>();
            //    spawnedObjects?.Add(spawnedObject);
            //    spawnedObject.objId = id;
            //}

            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            position.x += UnityEngine.Random.Range((int)-spawnArea_width, (int)spawnArea_width);
            position.y += UnityEngine.Random.Range((int)-spawnArea_height, (int)spawnArea_height);

            if (placeableObjectContainer.Get(position) != null) return;

            if (!oneTime)
            {
                SpawnedObject spawnedObject = go.AddComponent<SpawnedObject>();
                spawnedObjects?.Add(spawnedObject);
                spawnedObject.objId = id;
            }

            //Debug.Log(placeableObjectContainer.Get(position));
            //Vector3 position = transform.position;
            //position.x += UnityEngine.Random.Range(-spawnArea_width, spawnArea_width);
            //position.y += UnityEngine.Random.Range(-spawnArea_height, spawnArea_height);
            
            t.position = position + new Vector3(0.5f, 0.5f, 0f);
        }
    }
    public void SpawnedObjectDestroyed(SpawnedObject spawnedObject)
    {
        spawnedObjects.Remove(spawnedObject);
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
        ToSave toSave = new ToSave();

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i] != null) 
            {
                toSave.spawnedObjectDatas.Add(
                new SpawnedObject.SaveSpawnedObjectData(
                    spawnedObjects[i].objId,
                    spawnedObjects[i].transform.position
                    )
                );
            }
        }

        return JsonUtility.ToJson( toSave );
    }

    public void Load(string json)
    {
        if (json == "" || json == "{}" || json == null) return;

        ToSave toLoad = JsonUtility.FromJson<ToSave>(json);

        for (int i = 0; i < toLoad.spawnedObjectDatas.Count; i++)
        {
            SpawnedObject.SaveSpawnedObjectData data = toLoad.spawnedObjectDatas[i];
            GameObject go = Instantiate(spawn[data.objectId]);
            go.transform.position = data.worldPosition;
            go.transform.SetParent(transform);
            SpawnedObject so = go.AddComponent<SpawnedObject>();
            so.objId = data.objectId;
            spawnedObjects.Add(so);
        }
    }

    private void OnDestroy()
    {
        if (oneTime) return;
        SaveData();
    }

    private bool CheckJSON()
    {
        if (oneTime) return false;

        if (targetSaveJSONList == null)
        {
            Debug.LogError("Target JSON scriptable object to save data on spawnable object is null!");
            return false;
        }

        if (idInList == -1)
        {
            Debug.LogError("Id in list is not assigned, data can't be saved!");
            return false;
        }

        return true;
    }

    private void SaveData()
    {
        if (!CheckJSON()) return;

        string jsonString = Read();
        Debug.Log(idInList);
        targetSaveJSONList.SetString(jsonString, idInList);
    }

    private void LoadData()
    {
        if (!CheckJSON()) return;

        Load(targetSaveJSONList.GetString(idInList));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea_width * 2, spawnArea_height * 2));
    }
}
