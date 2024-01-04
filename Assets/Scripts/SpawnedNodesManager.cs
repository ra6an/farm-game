using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnedNodesManager : MonoBehaviour
{
    [SerializeField] SpawnedNodeContainer spawnedNodes;
    [SerializeField] Tilemap targetTilemap;

    [SerializeField] GameObject[] nodes;

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
        SpawnedNode spawnedNode = (SpawnedNode)spawnedNodes.Get(position);

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
                string jsonString = persistant.Read();
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

        foreach(GameObject n in nodes)
        {
            if(n.name == node.node.ToString())
            {
                go = Instantiate(n.gameObject);
            }
        }

        if (go == null) return;

        go.transform.parent = transform;

        Vector3 position = targetTilemap.CellToWorld(node.positionOnGrid) + targetTilemap.cellSize / 2;
        position -= Vector3.forward * 0.1f;
        go.transform.position = position;

        IPersistant persistant = go.GetComponent<IPersistant>();
        //Debug.Log(persistant);
        if (persistant != null)
        {
            persistant.Load(node.objectState);
        }

        node.targetObject = go.transform;
    }

    public bool Check(Vector3Int position)
    {
        return spawnedNodes.Get(position) != null;
    }

    public void Spawn(NodeType nodeName, Vector3Int position)
    {
        if (Check(position)) return;

        //GameObject node;
        if (nodes.Length <= 0) return;

        foreach(GameObject n in nodes)
        {
            if (n.name == nodeName.ToString())
            {
                SpawnedNode spawnedNode = new SpawnedNode(nodeName, position);

                VisualizeNode(spawnedNode);
                spawnedNodes.spawnedNodes.Add(spawnedNode);
                //node = Instantiate(n.gameObject);
            }
        }

        //SpawnedNode spawnedNode = new SpawnedNode(nodeName, position);

        //VisualizeNode(spawnedNode);
        //spawnedNodes.spawnedNodes.Add(spawnedNode);
    }
}