using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    public Item equipedItem;
    [SerializeField] Image itemImage;
    [SerializeField] Image placeholderImage;
    public TypeEquipable typeEquipable;

    public void EquipItem(Item item)
    {
        if (!item.equipable) return;
        if (item.equipableType != typeEquipable) return;

        SetEquipmentSlot(item);
    }

    public void UnequipItem()
    {
        GameManager.instance.player.GetComponent<EquipItemController>().UnequipItem(equipedItem, typeEquipable);

        CleanEquipmentSlot();
    }

    public void SetEquipmentSlot(Item i)
    {
        equipedItem = i;
        itemImage.sprite = i.icon;
        itemImage.gameObject.SetActive(true);
        placeholderImage.gameObject.SetActive(false);
    }

    public void CleanEquipmentSlot()
    {
        equipedItem = null;
        itemImage.gameObject.SetActive(false);
        placeholderImage.gameObject.SetActive(true);
        itemImage.sprite = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && equipedItem != null)
        {
            UnequipItem();
        }
    }
}
