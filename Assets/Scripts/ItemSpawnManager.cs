using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    //NOVI KOD
    /// /////////////////////////////////////////////////

    public static ItemSpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject pickUpItemPrefab;

    public void SpawnItem(Vector3 position, Item item, int count) {
        GameObject o = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        o.GetComponent<SpawnItem>().Set(item, count);
    }
}
