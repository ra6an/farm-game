using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemController : MonoBehaviour
{
    [SerializeField] EquipedItemsData equipedItemsData;
    [SerializeField] GameObject playerStatsPanel;
    private ItemContainer inventory;
    private bool needRefresh = false;

    private void Start()
    {
        inventory = GameManager.instance.inventoryContainer;
    }

    private void Update()
    {
        if (needRefresh)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        RefreshGUI();
        needRefresh = false;
    }

    private void RefreshGUI()
    {
        if (playerStatsPanel == null) return;

        playerStatsPanel.GetComponent<PlayerStatsPanel>().ShowStats();
        transform.GetComponent<Character>().UpdateStatusBars();
    }

    public void EquipItem(Item item, int myIndex)
    {
        //If Item is already equiped, first unequip it
        if(!playerStatsPanel.GetComponent<PlayerStatsPanel>().SlotIsEmpty(item.equipableType))
        {
            playerStatsPanel.GetComponent<PlayerStatsPanel>().UnequipSlot(item.equipableType);
        }

        //Equiping item on panel
        playerStatsPanel.GetComponent<PlayerStatsPanel>().EquipItem(item);

        //Adding moddifiers to stats
        item.Equip(GameManager.instance.player.GetComponent<Character>());

        //Removing item from inventory
        inventory.RemoveAt(myIndex);

        //Refreshing all the GUI
        RefreshGUI();
    }

    public void UnequipItem(Item item, TypeEquipable type)
    {
        if (item == null) return;

        if (inventory == null) return;

        bool hasFreeSpace = inventory.CheckFreeSpace();
        if (hasFreeSpace)
        {
            int itemId = GameManager.instance.itemsDB.GetItemId(item);
            inventory.Add(itemId);
            item.Unequip(GameManager.instance.player.GetComponent<Character>());

            for (int i = 0; i < equipedItemsData.equipedItems.Count; i++)
            {
                if (equipedItemsData.equipedItems[i].type == type)
                {
                    equipedItemsData.equipedItems[i].item = null;
                }
            }
        }

        RefreshGUI();
    }
}
