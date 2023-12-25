using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    public ItemSlot itemSlot;
    [SerializeField] GameObject itemIcon;
    RectTransform iconTransform;
    Image itemIconImage;

    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconImage = itemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        if(itemIcon.activeInHierarchy)
        {
            iconTransform.position = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0;

                    ItemSpawnManager.instance.SpawnItem(
                        worldPosition, 
                        itemSlot.item, 
                        itemSlot.quantity
                        );

                    itemSlot.Clear();
                    itemIcon.SetActive(false);
                }
            }
        }
    }

    public bool Check(Item item, int count = 1)
    {
        if (itemSlot == null) return false;

        if (item.stackable)
        {
            return itemSlot.item == item && itemSlot.quantity >= count;
        }

        return itemSlot.item == item;
    }

    internal void OnClick(ItemSlot itemSlot)
    {
        if (this.itemSlot.item == null)
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
                {
                    Item item = itemSlot.item;
                    int count = itemSlot.quantity;

                    itemSlot.Copy(this.itemSlot);
                    this.itemSlot.Set(item, count);
                }
            }
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if(itemSlot.item == null)
        {
            itemIcon.SetActive(false);
        }
        else
        {
            itemIcon.SetActive(true);
            itemIconImage.sprite = itemSlot.item.icon;
        }
    }

    internal void RemoveItem(int count = 1)
    {
        if (itemSlot == null) return;

        if(itemSlot.item.stackable)
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
