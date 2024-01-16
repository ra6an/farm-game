using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] EquipedItemsData equipedItemsData;
    public Item equipedItem;
    [SerializeField] Image itemImage;
    [SerializeField] Image placeholderImage;
    public TypeEquipable typeEquipable;

    private void Start()
    {
        //equipedItem = new Item();
    }

    public void EquipItem(Item item)
    {
        if (!item.equipable) return;
        if (item.equipableType != typeEquipable) return;

        equipedItem = item;
        itemImage.sprite = item.icon;
        itemImage.gameObject.SetActive(true);
        placeholderImage.gameObject.SetActive(false);
    }

    public void UnequipItem()
    {
        if (equipedItem == null) return;

        ItemContainer inventory = GameManager.instance.inventoryContainer;
        if (inventory == null) return;

        bool hasFreeSpace = inventory.CheckFreeSpace();
        if(hasFreeSpace)
        {
            inventory.Add(equipedItem);
            equipedItem.Unequip(GameManager.instance.player.GetComponent<Character>());
            equipedItem = null;
            itemImage.gameObject.SetActive(false);
            placeholderImage.gameObject.SetActive(true);
            itemImage.sprite = null;

            for (int i = 0; i < equipedItemsData.equipedItems.Count; i++)
            {
                if (equipedItemsData.equipedItems[i].type == typeEquipable)
                {
                    equipedItemsData.equipedItems[i].item = null;
                }
            }
        }

        GameObject.Find("PlayerStatsPanel").GetComponent<PlayerStatsPanel>().ShowStats();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && equipedItem != null)
        {
            UnequipItem();
        }
    }
}
