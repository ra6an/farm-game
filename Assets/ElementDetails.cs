using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementDetails : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text elementName;
    [SerializeField] Text quantity;
    [SerializeField] Text inventoryQuantity;

    public void SetDetails(ItemSlot itemSlot)
    {
        icon.sprite = itemSlot.item.icon;
        elementName.text = itemSlot.item.name;
        quantity.text = itemSlot.quantity.ToString();
    }

    public void ChangeQuantity(int qty)
    {
        quantity.text = qty.ToString();
    }

    public void ChangeInventoryQuantity(int inventoryQty)
    {
        inventoryQuantity.text = inventoryQty.ToString();
    }

    public void SetNotEnough()
    {
        quantity.color = Color.red;
    }

    public void SetEnough()
    {
        quantity.color = Color.black;
    }
 }
