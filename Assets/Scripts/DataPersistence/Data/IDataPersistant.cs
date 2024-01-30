using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public interface IDataPersistant
{
    public bool isOneTimeLoader();

    public void SaveData(GameData data);

    public void LoadData(GameData data);
}
