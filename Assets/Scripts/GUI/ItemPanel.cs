using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    public ItemContainer inventory;
    public List<InventoryButton> buttons;

    //private void Update()
    //{
    //    if(inventory == null)
    //    {
    //        if (GameManager.instance.inventoryContainer == null) return;
    //        inventory = GameManager.instance.inventoryContainer;
    //        Init();
    //    }    
    //}

    private void Awake()
    {
        
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetIndex();
        Show();
    }

    private void OnEnable()
    {
        Show();
    }

    private void LateUpdate()
    {
        if(inventory.isDirty)
        {
            Show();
            inventory.isDirty = false;
        }
    }

    private void SetIndex()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetIndex(i);
        }
    }

    public virtual void Show()
    {
        for (int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            if (inventory.slots[i].item < 0)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(inventory.slots[i]);
            }
        }

        //if(inventory.slotsLocked > 0)
        //{
        //    for (int i = inventory.slots.Count; i < inventory.slots.Count + inventory.slotsLocked; i++)
        //    {
        //        buttons[i].Locked(item);
        //    }
        //}
    }

    public virtual void OnClick(int id)
    {

    }
}
