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

    public Item GetItem
    {
        get
        {
            return GameManager.instance.inventoryContainer.slots[selectedTool].item;
        }
    }

    private void Start()
    {
        onChange += UpdateHighlightIcon;
        UpdateHighlightIcon(selectedTool);
    }

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if(delta != 0)
        {
            if(delta < 0 )
            {
                selectedTool = selectedTool + 1 > toolbarSize - 1 ? 0 : selectedTool + 1;
            } else
            {
                selectedTool = selectedTool - 1 < 0 ? toolbarSize - 1 : selectedTool - 1;
            }
            onChange?.Invoke(selectedTool);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTool = 0;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTool = 1;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTool = 2;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedTool = 3;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedTool = 4;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedTool = 5;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedTool = 6;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedTool = 7;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectedTool = 8;
            onChange?.Invoke(selectedTool);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedTool = 9;
            onChange?.Invoke(selectedTool);
        }
    }

    internal void Set(int id)
    {
        selectedTool = id;
    }

    public void UpdateHighlightIcon(int id = 0)
    {
        Item item = GetItem;

        if(item == null)
        {
            iconHighlight.Show = false;
            return;
        }

        iconHighlight.Show = item.iconHighlight;
        if (item.iconHighlight)
        {
            iconHighlight.Set(item.icon);
        }
    }
}
