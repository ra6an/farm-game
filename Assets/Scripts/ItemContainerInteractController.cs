using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    InventoryController inventoryController;
    [SerializeField] GameObject storagePanel;
    [SerializeField] GameObject storageColorImage;
    [SerializeField] GameObject storageSlider;
    [SerializeField] ItemContainerPanel itemContainerPanel;
    Transform openedChest;
    [SerializeField] float maxDistance = 0.8f;
    ShowPanelsController showPanelsController;

    private bool changeColor = false;
    
    private void Awake()
    {
        inventoryController = GetComponent<InventoryController>();
        showPanelsController = GetComponent<ShowPanelsController>();
    }

    private void Update()
    {
        if(changeColor)
        {
            SetSliderColor();
        }

        if(openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            if(distance > maxDistance)
            {
                openedChest.GetComponent<StorageContainerInteract>().Close(GetComponent<Character>());
                showPanelsController.CloseStorage();
            }
        }
    }

    public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        itemContainerPanel.inventory = targetItemContainer;

        changeColor = true;

        showPanelsController.OpenStorage();
        openedChest = _openedChest;
    }

    public void Close()
    {
        showPanelsController.CloseStorage();
        openedChest = null;
    }

    public void SetChestColor(Color color)
    {
        if (openedChest == null) return;
        openedChest.GetComponent<StorageContainerInteract>().SetColor(color);
    }

    public void SetSliderColor()
    {
        if (openedChest == null) return;
        //Change slider value and image color
        float hueValue, satValue, valValue;
        Color.RGBToHSV(openedChest.GetComponent<SpriteRenderer>().color, out hueValue, out satValue, out valValue);
        storageSlider.GetComponent<RGBSlider>().SetHueValue(hueValue);
        storageColorImage.GetComponent<Image>().color = openedChest.GetComponent<SpriteRenderer>().color;

        changeColor = false;
    }
}
