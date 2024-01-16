using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ItemContainer inventory;
    [SerializeField] Image icon;
    [SerializeField] Text quantity;
    [SerializeField] Image highlight;
    [SerializeField] Sprite activeButton;
    [SerializeField] Sprite inactiveButton;

    public ItemSlot itemSlot;

    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void Set(ItemSlot slot)
    {
        itemSlot.Copy(slot);
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;

        if(slot.item.stackable)
        {
            quantity.gameObject.SetActive(true);
            quantity.text = slot.quantity.ToString();
        } else
        {
            quantity.gameObject.SetActive(false);
        }
    }

    public void Clean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        itemSlot.Clear();

        quantity.gameObject.SetActive(false);
    }

    public void Locked(Item locked)
    {
        quantity.text = null;
        quantity.gameObject.SetActive(false);

        icon.gameObject.SetActive(true);
        icon.sprite = locked.icon;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right && itemSlot.item != null)
        {
            if(itemSlot.item.equipable)
            {
                GameManager.instance.player.GetComponent<EquipItemController>().EquipItem(itemSlot.item, myIndex);
                return;
            }
        }
        
        ItemPanel itemPanel = transform.parent.GetComponent<ItemPanel>();
        itemPanel.OnClick(myIndex);
    }

    public void Highlight(bool active)
    {
        this.GetComponent<Image>().sprite = active ? activeButton : inactiveButton;

        icon.transform.position = active ? 
            new Vector3(icon.transform.position.x, icon.transform.position.y + 1f, icon.transform.position.z) : 
            new Vector3(icon.transform.position.x, icon.transform.position.y - 1f, icon.transform.position.z);
    }
}
