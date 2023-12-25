using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementDetails : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text elementName;
    [SerializeField] Text quantity;

    public void SetDetails(ItemSlot itemSlot)
    {
        icon.sprite = itemSlot.item.icon;
        elementName.text = itemSlot.item.name;
        quantity.text = itemSlot.quantity.ToString();
    }
}
