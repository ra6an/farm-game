using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerStatsPanel : MonoBehaviour
{
    [SerializeField] GameObject statsContainer;
    [SerializeField] GameObject singleStatPrefab;
    [SerializeField] GameObject playerExperience;
    [SerializeField] Text playerLevel;
    [SerializeField] List<GameObject> equipmentSlots;
    [SerializeField] EquipedItemsData equippedItems;

    [HeaderAttribute("Prefab Stats")]
    [SerializeField] GameObject health;
    [SerializeField] GameObject healthRegen;
    [SerializeField] GameObject mana;
    [SerializeField] GameObject manaRegen;
    [SerializeField] GameObject speed;
    [SerializeField] GameObject physicalAttack;
    [SerializeField] GameObject magicAttack;
    [SerializeField] GameObject physicalPenetration;
    [SerializeField] GameObject magicPenetration;
    [SerializeField] GameObject criticalChance;
    [SerializeField] GameObject criticalDamage;
    [SerializeField] GameObject physicalDefense;
    [SerializeField] GameObject magicDefense;
    [SerializeField] GameObject physicalResistance;
    [SerializeField] GameObject magicResistance;
    [SerializeField] GameObject criticalResistance;

    private void Start()
    {
        ShowStats();
        ShowEquippedItems();
    }

    public void ShowStats()
    {
        Character character = GameManager.instance.player.GetComponent<Character>();

        if (character == null) return;

        health.GetComponent<SingleStat>().Set(character.health.maxVal.Value);
        healthRegen.GetComponent<SingleStat>().Set(character.healthRegen.Value, StatOutputType.Decimal);
        mana.GetComponent<SingleStat>().Set(character.mana.maxVal.Value);
        manaRegen.GetComponent<SingleStat>().Set(character.manaRegen.Value, StatOutputType.Decimal);
        speed.GetComponent<SingleStat>().Set(character.speed.Value, StatOutputType.Decimal);

        //Attack
        physicalAttack.GetComponent<SingleStat>().Set(character.physicalDamage.Value);
        magicAttack.GetComponent<SingleStat>().Set(character.magicDamage.Value);
        physicalPenetration.GetComponent<SingleStat>().Set(character.physicalPenetration.Value);
        magicPenetration.GetComponent<SingleStat>().Set(character.magicPenetration.Value);
        criticalChance.GetComponent<SingleStat>().Set(character.criticalChance.Value, StatOutputType.Percent);
        criticalDamage.GetComponent<SingleStat>().Set(character.criticalDamage.Value);

        //Defense
        physicalDefense.GetComponent<SingleStat>().Set(character.physicalDefense.Value);
        magicDefense.GetComponent<SingleStat>().Set(character.magicDefense.Value);
        physicalResistance.GetComponent<SingleStat>().Set(character.physicalResistance.Value);
        magicResistance.GetComponent<SingleStat>().Set(character.magicResistance.Value);
        criticalResistance.GetComponent<SingleStat>().Set(character.criticalResistance.Value);

        playerLevel.text = character.level.ToString();
        playerExperience.GetComponent<ExperiencePanel>().Set(character.experience.currVal, character.experience.maxVal.BaseValue);
    }

    public void ShowEquippedItems()
    {
        for (int i = 0; i < equippedItems.equipedItems.Count; i++)
        {
            Item currItem = equippedItems.equipedItems[i].item;
            if (equippedItems.equipedItems[i].item == null) continue;

            for (int x = 0; x < equipmentSlots.Count; x++)
            {
                EquipmentSlot es = equipmentSlots[x].gameObject.GetComponent<EquipmentSlot>();

                if (currItem.equipableType == es.typeEquipable)
                {
                    es.EquipItem(currItem);
                }
            }
        }
    }

    public void EquipItem(Item item)
    {
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            EquipmentSlot es = equipmentSlots[i].gameObject.GetComponent<EquipmentSlot>();

            if(item.equipableType == es.typeEquipable)
            {
                es.EquipItem(item);
            }
        }

        for (int i = 0; i < equippedItems.equipedItems.Count; i++)
        {
            if (equippedItems.equipedItems[i].type == item.equipableType)
            {
                equippedItems.equipedItems[i].item = item;
            }
        }

        ShowEquippedItems();
    }

    public bool SlotIsEmpty(TypeEquipable type)
    {
        bool isEmpty = true;

        foreach (GameObject go in equipmentSlots)
        {
            EquipmentSlot es = go.GetComponent<EquipmentSlot>();
            if (es.typeEquipable == type && es.equipedItem != null) 
            {
                isEmpty = false;
            }
        }

        return isEmpty;
    }

    public void UnequipSlot(TypeEquipable type)
    {
        foreach (GameObject go in equipmentSlots)
        {
            EquipmentSlot es = go.GetComponent<EquipmentSlot>();
            if (es.typeEquipable == type)
            {
                es.UnequipItem();
            }
        }
    }
}
