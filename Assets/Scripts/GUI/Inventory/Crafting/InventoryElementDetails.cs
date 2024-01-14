using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElementDetails : MonoBehaviour
{
    private ItemSlot item;
    [SerializeField] Image icon;
    [SerializeField] Text elementName;
    [SerializeField] Text quantity;
    [SerializeField] Text inventoryQuantity;

    private void Awake()
    {
        item = new ItemSlot();
    }

    public void SetDetails(ItemSlot itemSlot, int multiplyer)
    {
        //Debug.Log(itemSlot.item + ", " + itemSlot.quantity + ", " + item);
        item.Copy(itemSlot);
        icon.sprite = itemSlot.item.icon;
        elementName.text = itemSlot.item.name;
        quantity.text = itemSlot.quantity.ToString();

        CheckInventoryQuantity(itemSlot, multiplyer);
    }

    public void CheckInventoryQuantity(ItemSlot itemSlot, int multiplier)
    {
        ItemSlot inventorySlot = GameManager.instance.inventoryContainer.GetItemSlot(itemSlot.item);
        if (inventorySlot == null || inventorySlot.quantity < itemSlot.quantity * multiplier)
        {
            inventoryQuantity.text = inventorySlot == null ? "0" : inventorySlot.quantity.ToString();
            SetNotEnough();
        } else
        {
            inventoryQuantity.text = inventorySlot.quantity.ToString();
            SetEnough();
        }
    }

    public void OnMultiplierChange(int multiplier)
    {
        //Debug.Log("change-tu smo: " + transform.name);
        //Debug.Log(item.item.name + ", " + item.quantity + ", " + multiplier);
        quantity.text = (item.quantity * multiplier).ToString();
        SetEnough();
    }

    public void OnMultiplierError(int multiplier)
    {
        //Debug.Log("error-tu smo: " + transform.name);
        //Debug.Log(item.item.name + ", " + item.quantity + ", " + multiplier);
        ItemSlot inventorySlot = GameManager.instance.inventoryContainer.GetItemSlot(item.item);
        if (inventorySlot == null || inventorySlot.quantity < item.quantity * (multiplier + 1))
        {
            //inventoryQuantity.text = inventorySlot == null ? "0" : inventorySlot.quantity.ToString();
            //quantity.text = multiplier.ToString();
            SetNotEnough();
        }
        else
        {
            //quantity.text = multiplier.ToString();
            SetEnough();
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
