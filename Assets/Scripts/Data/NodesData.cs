using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Node Data")]
public class NodesData : ScriptableObject
{
    public string nodeName;
    public int width = 1;
    public int height = 1;

    public NodesData() { }
}
