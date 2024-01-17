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
        quantity.text = (item.quantity * multiplier).ToString();
        SetEnough();
    }

    public void OnMultiplierError(int multiplier)
    {
        ItemSlot inventorySlot = GameManager.instance.inventoryContainer.GetItemSlot(item.item);
        if (inventorySlot == null || inventorySlot.quantity < item.quantity * (multiplier + 1))
        {
            SetNotEnough();
        }
        else
        {
            SetEnough();
        }
    }

    public void ChangeQuantity(int qty)
    {
        quantity.text = qty.ToString();
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
