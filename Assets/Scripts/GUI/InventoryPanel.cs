using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : ItemPanel
{
    public override void OnClick(int id)
    {
        GameManager.instance.dragAndDropController.OnClick(inventory.slots[id]);
        Show();
    }
}
