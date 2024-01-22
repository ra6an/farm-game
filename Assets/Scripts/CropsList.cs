using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Crops List")]
public class CropsList : ScriptableObject
{
    public List<Crop> crops;

    public Crop GetCropById(int id)
    {
        return crops[id];
    }

    public int GetCropId(Crop crop)
    {
        for (int i = 0; i < crops.Count; i++)
        {
            if(crop == crops[i]) return i;
        }

        return -1;
    }
}
