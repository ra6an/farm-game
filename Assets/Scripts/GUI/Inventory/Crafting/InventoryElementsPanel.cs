using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryElementsPanel : MonoBehaviour
{
    [SerializeField] GameObject prefabElementPanel;

    public void SetElementsDetails(List<ItemSlot> elements, int multiplyer)
    {
        if (elements.Count == 0) return;

        foreach (ItemSlot element in elements)
        {
            GameObject go = Instantiate(prefabElementPanel, transform);
            go.GetComponent<InventoryElementDetails>().SetDetails(element, multiplyer);
        }
    }

    public void OnMultiplierChange(int multiplier, bool canChange = true)
    {
        if(canChange)
        {
            foreach(Transform t in transform)
            {
                t.GetComponent<InventoryElementDetails>().OnMultiplierChange(multiplier);
            }
        } else
        {
            foreach (Transform t in transform)
            {
                t.GetComponent<InventoryElementDetails>().OnMultiplierError(multiplier);
            }
        }
    }

    public void ClearPanel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
