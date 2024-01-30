using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EquipItemController : MonoBehaviour, IDataPersistant
{
    [SerializeField] public EquipedItemsData equipedItemsData;
    [SerializeField] GameObject playerStatsPanel;
    private ItemContainer inventory;
    private bool needRefresh = false;
    public bool oneTimeLoader = true;

    private void Start()
    {
        //if (equipedItemsData == null)
        //{
        //    equipedItemsData = (EquipedItemsData)ScriptableObject.CreateInstance(typeof(EquipedItemsData));
        //    equipedItemsData.Init();
        //}
        if(GameManager.instance.inventoryContainer != null)
        {
            inventory = GameManager.instance.inventoryContainer;
        }
    }

    private void Update()
    {
        if (needRefresh)
        {
            if (inventory == null) inventory = GameManager.instance.inventoryContainer;
            if (inventory == null) return;
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
                    equipedItemsData.equipedItems[i].item = -1;
                }
            }
        }

        RefreshGUI();
    }

    //Persisting Equiped Items
    [Serializable]
    public class CopyOfEquipedItems
    {
        public List<EquipedItem> equipedItems;

        public void Init()
        {
            equipedItems = new();
        }
    }

    public void SaveData(GameData data)
    {
        string equipedItemsJson = "";

        if (equipedItemsData == null) return;
        Debug.Log("EQUIPED ITEMS SAVE");

        CopyOfEquipedItems copyOfEquipedItems = new();
        copyOfEquipedItems.Init();

        foreach (EquipedItem ei in equipedItemsData.equipedItems)
        {
            copyOfEquipedItems.equipedItems.Add(ei);
        }

        equipedItemsJson = JsonUtility.ToJson(copyOfEquipedItems);

        data.equipedItems = equipedItemsJson;
    }

    public void LoadData(GameData data)
    {
        if(equipedItemsData == null)
        {
            equipedItemsData = (EquipedItemsData)ScriptableObject.CreateInstance(typeof(EquipedItemsData));
            equipedItemsData.Init();
        }

        Debug.Log(equipedItemsData.equipedItems.Count);

        if (data.equipedItems == "" || data.equipedItems == "{}" || data.equipedItems == null) return;

        CopyOfEquipedItems copyOfEquipedItems = new();
        copyOfEquipedItems.Init();

        CopyOfEquipedItems deserializedList = JsonUtility.FromJson<CopyOfEquipedItems>(data.equipedItems);

        foreach (EquipedItem ei in deserializedList.equipedItems)
        {
            if(ei.item != -1)
            {
                Item itemToEquip = GameManager.instance.itemsDB.GetItemById(ei.item);

                if (itemToEquip == null || !itemToEquip.equipable) continue;

                equipedItemsData.SetEquipSlot(ei.item, ei.itemState, ei.type);
                itemToEquip.Equip(this.GetComponent<Character>());
            }
        }
    }

    public bool isOneTimeLoader()
    {
        return oneTimeLoader;
    }
}
