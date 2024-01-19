using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//
public class ElementDetails : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text elementName;
    [SerializeField] Text quantity;
    [SerializeField] Text inventoryQuantity;

    public void SetDetails(ItemSlot itemSlot)
    {
        Item item = GameManager.instance.itemsDB.GetItemById(itemSlot.item);

        if (item == null) return;

        icon.sprite = item.icon;
        elementName.text = item.name;
        quantity.text = itemSlot.quantity.ToString();

        //TEST MOJ KOD
        ItemSlot inventorySlot = GameManager.instance.inventoryContainer.GetItemSlot(itemSlot.item);
        if(inventorySlot != null )
        {
            inventoryQuantity.text = inventorySlot.quantity.ToString();
        }
        else
        {
            inventoryQuantity.text = "0";
        }
    }

    public void ChangeQuantity(int qty)
    {
        quantity.text = qty.ToString();
    }

    //public void ChangeInventoryQuantity(int inventoryQty)
    //{
    //    inventoryQuantity.text = inventoryQty.ToString();
    //}

    public void SetNotEnough()
    {
        quantity.color = Color.red;
    }

    public void SetEnough()
    {
        quantity.color = Color.black;
    }
 }
