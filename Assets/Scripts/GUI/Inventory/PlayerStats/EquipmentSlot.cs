using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
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
}
