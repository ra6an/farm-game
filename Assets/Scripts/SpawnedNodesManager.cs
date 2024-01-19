using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class SpawnedNodesManager : MonoBehaviour, IDataPersistant
{
    [SerializeField] string sceneName;
    [SerializeField] SpawnedNodeContainer spawnedNodes;
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] List<NodesData> nodesData;

    [SerializeField] public GameObject[] nodes;

    private void Start()
    {
        GameManager.instance.GetComponent<SpawnedNodesReferenceManager>().spawnedNodesManager = this;
        VisualizeMap();

        GameObject go = GameObject.Find("NodeSpawners");

        if(go != null)
        {
            ObjectSpawner[] childs = go.GetComponentsInChildren<ObjectSpawner>();
            
            foreach(ObjectSpawner c in childs) 
            {
                c.canCheck = true;
            }
        }
        
    }

    public void DestroyNode(Vector3Int position)
    {
        List<Vector3Int> listPosition = new List<Vector3Int>();
        listPosition.Add(position);
        SpawnedNode spawnedNode = (SpawnedNode)spawnedNodes.Get(listPosition);

        if (spawnedNode == null) return;

        Destroy(spawnedNode.targetObject.gameObject);
        spawnedNodes.Remove(spawnedNode);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < spawnedNodes.spawnedNodes.Count; i++)
        {
            if (spawnedNodes.spawnedNodes[i].targetObject == null) { continue; }

            IPersistant persistant = spawnedNodes.spawnedNodes[i].targetObject.GetComponent<IPersistant>();
            //Debug.Log(persistant);
            if (persistant != null)
            {
                string jsonString = persistant.SaveData();
                spawnedNodes.spawnedNodes[i].objectState = jsonString;
            }

            spawnedNodes.spawnedNodes[i].targetObject = null;
        }
    }

    public void VisualizeMap()
    {
        foreach(SpawnedNode node in spawnedNodes.spawnedNodes)
        {
            VisualizeNode(node);
        }
    }

    private void VisualizeNode(SpawnedNode node)
    {
        GameObject go = null;
        NodesData nd = ScriptableObject.CreateInstance<NodesData>();

        foreach(GameObject n in nodes)
        {
            if(n.name == node.node.ToString())
            {
                go = Instantiate(n.gameObject);
            }
        }

        foreach(NodesData n in nodesData)
        {
            if(n.nodeName == node.node.ToString())
            {
                nd.nodeName = n.nodeName;
                nd.height = n.height;
                nd.width = n.width;
            }
        }

        if (go == null) return;
        //Debug.Log(node.targetObject.name);

        go.transform.parent = transform;
        //DOVDE UDURE
        int posOnGridX = node.positionOnGrid[0].x;
        int posOnGridY = node.positionOnGrid[0].y;
        int itemWidth = nd.width;
        int itemHeight = nd.height;
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
        else if (itemWidth > 1 && itemWidth % 2 == 0)
        {
            xPosition = (posOnGridX + (float)itemWidth / 2);
            xAddition = 0f;
        }

        if (itemHeight == 1)
        {
            yPosition = posOnGridY;
            yAddition = 0.5f;
        }
        else if (itemHeight > 1 && itemHeight % 2 != 0)
        {
            yPosition = posOnGridY + ((float)itemHeight / 2 + 0.5f);
            yAddition = -0.5f;
        }
        else if (itemHeight > 1 && itemHeight % 2 == 0)
        {
            yPosition = (posOnGridY + (float)itemHeight / 2);
            yAddition = 0f;
        }

        Vector3Int posForWorld = new Vector3Int(
            (int)xPosition, (int)yPosition, node.positionOnGrid[0].z
            );

        Vector3 position = targetTilemap.CellToWorld(posForWorld) + new Vector3(xAddition, yAddition, 0);
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistant persistant = go.GetComponent<IPersistant>();
        
        if (persistant != null)
        {
            persistant.LoadData(node.objectState);
        }

        node.targetObject = go.transform;
    }

    public bool Check(List<Vector3Int> position)
    {
        return spawnedNodes.Get(position) != null;
    }

    public void Spawn(NodeType nodeName, List<Vector3Int> position)
    {
        if (Check(position)) return;

        if (nodes.Length <= 0) return;

        foreach(GameObject n in nodes)
        {
            if (n.name == nodeName.ToString())
            {
                SpawnedNode spawnedNode = new SpawnedNode(nodeName, position);

                VisualizeNode(spawnedNode);
                spawnedNodes.spawnedNodes.Add(spawnedNode);
            }
        }
    }
    //TAKODJE SREDITI KOD ISPOD SA PARAMETROM IZ FUNKCIJE
    public void SaveData(ref GameData data)
    {
        /*
        DataPersistentManager DPManager = DataPersistentManager.instance;

        if (DPManager == null) return;

        SpawnedNodes toSave = new SpawnedNodes();

        toSave.sceneName = sceneName;
        toSave.container = spawnedNodes;

        DPManager.gameData.spawnedNodesContainers.Add(toSave);
        */
    }

    public void LoadData(GameData data)
    {
        /*
        DataPersistentManager DPManager = DataPersistentManager.instance;

        if (DPManager == null) return;

        foreach(SpawnedNodes sn in DPManager.gameData.spawnedNodesContainers)
        {
            if (sn.sceneName == sceneName)
            {
                spawnedNodes = sn.container;
            }
        }
        */
    }
}