using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistant
{
    public string SaveData();

    public void LoadData(string jsonString);
}
