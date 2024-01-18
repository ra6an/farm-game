using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Convertable Item")]
public class ConvertableItem : ScriptableObject
{
    public ItemSlot input;
    public int timeToConvert;
    public ItemSlot output;
}
