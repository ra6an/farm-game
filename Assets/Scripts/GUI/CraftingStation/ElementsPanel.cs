using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElementsPanel : MonoBehaviour
{
    [SerializeField] GameObject prefabElementPanel;

    public void SetElementsDetails(List<ItemSlot> elements)
    {
        if (elements.Count == 0) return;
        
        foreach (ItemSlot element in elements)
        {
            GameObject go = Instantiate(prefabElementPanel, transform);
            go.GetComponent<ElementDetails>().SetDetails(element);
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
