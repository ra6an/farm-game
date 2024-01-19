using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemInventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] Sprite activeButton;
    [SerializeField] Sprite inactiveButton;

    CraftingRecipe recipe;
    int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void Set(CraftingRecipe rec)
    {
        if (recipe != null) { return; }
        recipe = rec;

        Item item = GameManager.instance.itemsDB.GetItemById(rec.output.item);

        if (item == null) return;

        icon.sprite = item.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("CraftingPanel").GetComponent<RecipePanel>().SetActiveRecipe(myIndex);
    }

    public void ActiveButton()
    {
        transform.GetComponent<Image>().sprite = inactiveButton;
    }

    public void InactiveButton()
    {
        transform.GetComponent<Image>().sprite = activeButton;
    }
}
