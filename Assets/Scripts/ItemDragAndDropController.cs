using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    private InputManager inputManager;
    public ItemSlot itemSlot;
    [SerializeField] GameObject itemIcon;
    RectTransform iconTransform;
    Image itemIconImage;

    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconImage = itemIcon.GetComponent<Image>();
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        if(itemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;

            if (inputManager.GetKeyDown(KeybindingActions.Select) && !EventSystem.current.IsPointerOverGameObject()) // This is Generic MB0 So it doesnt need Keybind Script
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;

                Item item = GameManager.instance.itemsDB.GetItemById(itemSlot.item);
                if (item == null) return;

                ItemSpawnManager.instance.SpawnItem(
                    worldPosition, 
                    item, 
                    itemSlot.quantity
                    );

                itemSlot.Clear();
                itemIcon.SetActive(false);
            }
        }
    }

    public bool Check(Item item, int count = 1)
    {
        if (itemSlot == null) return false;

        int itemId = GameManager.instance.itemsDB.GetItemId(item);

        if (item.stackable)
        {
            return itemSlot.item == itemId && itemSlot.quantity >= count;
        }

        return itemSlot.item == itemId;
    }

    internal void OnClick(ItemSlot itemSlot)
    {
        Debug.Log(this.itemSlot.item);
        //if (this.itemSlot.item == null)
        if(this.itemSlot.item < 0)
        {
            this.itemSlot.Copy(itemSlot);
            itemSlot.Clear();
        }
        else
        {
            if(itemSlot.item == this.itemSlot.item)
            {
                itemSlot.quantity += this.itemSlot.quantity;
                this.itemSlot.Clear();
            }
            else
            {

                //Item item = itemSlot.item;
                int item = itemSlot.item;
                int count = itemSlot.quantity;
                
                itemSlot.Copy(this.itemSlot);
                this.itemSlot.Set(item, count);
                
            }
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if(itemSlot.item < 0)
        {
            itemIcon.SetActive(false);
        }
        else
        {
            itemIcon.SetActive(true);

            Item item = GameManager.instance.itemsDB.GetItemById(itemSlot.item);
            if (item == null) return;

            itemIconImage.sprite = item.icon;
        }
    }
    //
    public void RemoveItem(int count = 1)
    {
        if (itemSlot == null) return;

        Item item = GameManager.instance.itemsDB.GetItemById(itemSlot.item);
        if (item == null) return;

        if (item.stackable)
        {
            itemSlot.quantity -= count;
            if(itemSlot.quantity <= 0)
            {
                itemSlot.Clear();
            }
        } else
        {
            itemSlot.Clear();
        }
        UpdateIcon();
    }
}
