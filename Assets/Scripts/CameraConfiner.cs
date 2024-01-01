using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] CinemachineConfiner confiner;

    void Start()
    {
        string currScene = GameManager.instance.GetComponent<GameSceneManager>().currentScene;
        Debug.Log(currScene);
        if (currScene == null) return;
        UpdateBounds(currScene);
        //UpdateBounds(confiner);
    }

    public void UpdateBounds(string str)
    {
        GameObject go = GameObject.Find("CameraConfiner (" + str + ")");

        if(go == null)
        {
            confiner.m_BoundingShape2D = null;
            return;
        }

        Collider2D bounds = go.GetComponent<Collider2D>();
        confiner.m_BoundingShape2D = bounds;
    }

    internal void UpdateBounds(Collider2D confiner)
    {
        this.confiner.m_BoundingShape2D = confiner;
    }
}
