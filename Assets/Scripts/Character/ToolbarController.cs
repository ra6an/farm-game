using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 10;
    int selectedTool;

    public Action<int> onChange;
    [SerializeField] IconHighlight iconHighlight;

    private InputManager inputManager;

    public ItemSlot GetItemSlot
    {
        get
        {
            return GameManager.instance.inventoryContainer.slots[selectedTool];
        }
    }

    public Item GetItem
    {
        get
        {
            if (GameManager.instance.inventoryContainer.slots.Count == 0) return null;
            int itemId = GameManager.instance.inventoryContainer.slots[selectedTool].item;

            return GameManager.instance.itemsDB.GetItemById(itemId);
        }
    }

    private void Start()
    {
        inputManager = InputManager.instance;

        onChange += UpdateHighlightIcon;
        UpdateHighlightIcon(selectedTool);

        Item i = GetItem;
        if(i != null && i.isWeapon)
        {
            i.Equip(this.GetComponent<Character>());
            this.GetComponent<EquipItemController>().Refresh();
        }
    }

    private void Update()
    {
        if (!this.GetComponent<ShowPanelsController>().AllPanelsAreClosed()) return;

        //SCROLL 
        float delta = Input.mouseScrollDelta.y;
        if(delta != 0)
        {
            if(delta < 0 )
            {
                int index = selectedTool + 1 > toolbarSize - 1 ? 0 : selectedTool + 1;
                UpdateToolbarSlot(index);
            } else
            {
                int index = selectedTool - 1 < 0 ? toolbarSize - 1 : selectedTool - 1;
                UpdateToolbarSlot(index);
            }
        }

        //Buttons
        if (inputManager.GetKeyDown(KeybindingActions.Slot_1))
        {
            UpdateToolbarSlot(0);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_2))
        {
            UpdateToolbarSlot(1);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_3))
        {
            UpdateToolbarSlot(2);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_4))
        {
            UpdateToolbarSlot(3);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_5))
        {
            UpdateToolbarSlot(4);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_6))
        {
            UpdateToolbarSlot(5);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_7))
        {
            UpdateToolbarSlot(6);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_8))
        {
            UpdateToolbarSlot(7);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_9))
        {
            UpdateToolbarSlot(8);
        }
        if (inputManager.GetKeyDown(KeybindingActions.Slot_10))
        {
            UpdateToolbarSlot(9);
        }
    }

    private void UpdateToolbarSlot(int i)
    {
        EquipUnequipHandler(i);
        Set(i);
        onChange?.Invoke(selectedTool);
    }

    private void EquipUnequipHandler(int i)
    {
        bool needRefresh = false;
        Item item = GetItem;
        int newItemId = GameManager.instance.inventoryContainer.slots[i].item;
        Item newItem = GameManager.instance.itemsDB.GetItemById(newItemId);

        if (item == null && newItem == null) return;

        if(item != null && item.isWeapon)
        {
            item.Unequip(this.GetComponent<Character>());
            needRefresh = true;
        }
        if(newItem != null && newItem.isWeapon)
        {
            newItem.Equip(this.GetComponent<Character>());
            needRefresh = true;
        }

        if(needRefresh) this.GetComponent<EquipItemController>().Refresh();
    }

    internal void Set(int id)
    {
        selectedTool = id;
    }

    public void UpdateHighlightIcon(int id = 0)
    {
        Item item = GetItem;

        //if(item == null)
        if (item == null)
        {
            iconHighlight.Show = false;
            iconHighlight.itemWidth = 1;
            iconHighlight.itemHeight = 1;
            return;
        }

        iconHighlight.Show = item.iconHighlight;
        if (item.iconHighlight)
        {
            iconHighlight.Set(item.icon);
            iconHighlight.itemWidth = item.width;
            iconHighlight.itemHeight = item.height;
        }
    }
}
