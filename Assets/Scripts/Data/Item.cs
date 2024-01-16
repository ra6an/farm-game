using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEquipable
{
    Undefined,
    Helmet,
    Chest,
    Pants,
    Shoes,
    Ring,
    Necklace,
    Rune
}

public enum WeaponType
{
    Undefined,
    Melee,
    Range,
    Mage
}

[CreateAssetMenu(menuName ="Data/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string name;
    public bool stackable;
    public Sprite icon;

    [Header("Actions")]
    public ToolAction onAction;
    public ToolAction onTileMapAction;
    public ToolAction onItemUsed;
    //public ToolAction onItemEquip;
    public bool iconHighlight;
    public GameObject itemPrefab;

    [Header("Equipable Item")]
    public bool equipable = false;
    public TypeEquipable equipableType = TypeEquipable.Undefined;
    [Space]
    //Attack FLAT
    [Header("Attack Flat")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float physicalPenetration = 0;
    public float magicPenetration = 0;
    [Range(0, 100)]
    public float criticalChance = 0;
    [Range(0, 100)]
    public float criticalDamage = 0;
    //[Space]
    //Attack PERCENT
    [Header("Attack Percent")]
    public float physicalDamagePercent = 0;
    public float magicDamagePercent = 0;
    public float physicalPenetrationPercent = 0;
    public float magicPenetrationPercent = 0;
    [Range(0, 1)]
    public float criticalChancePercent = 0;
    [Range(0, 1)]
    public float criticalDamagePercent = 0;
    //[Space]
    //Defense FLAT
    [Header("Defense Flat")]
    public float physicalDefense = 0;
    public float magicDefense = 0;
    public float physicalResistance = 0;
    public float magicResistance = 0;
    public float criticalResistance = 0;
    //[Space]
    //Other FLAT
    [Header("Other Flat")]
    public float speed = 0;
    public float health = 0;
    public float healthRegen = 0;
    public float mana = 0;
    public float manaRegen = 0;
    //[Space]
    //Other PERCENT
    [Header("Other Percent")]
    public float speedPercent = 0;
    public float healthPercent = 0;
    public float manaPercent = 0;

    public void Equip(Character character)
    {
        //Attack
        if (physicalDamage != 0) character.physicalDamage.AddModifier(new StatModifier(physicalDamage, StatModType.Flat, this));
        if (magicDamage != 0) character.magicDamage.AddModifier(new StatModifier(magicDamage, StatModType.Flat, this));
        if (physicalPenetration != 0) character.physicalPenetration.AddModifier(new StatModifier(physicalPenetration, StatModType.Flat, this));
        if (magicPenetration != 0) character.magicPenetration.AddModifier(new StatModifier(magicPenetration, StatModType.Flat, this));
        if (criticalChance != 0) character.criticalChance.AddModifier(new StatModifier(criticalChance, StatModType.Flat, this));
        if (criticalDamage != 0) character.criticalDamage.AddModifier(new StatModifier(criticalDamage, StatModType.Flat, this));

        //Attack in Percent
        if (physicalDamagePercent != 0) character.physicalDamage.AddModifier(new StatModifier(physicalDamagePercent, StatModType.PercentAdd, this));
        if (magicDamagePercent != 0) character.magicDamage.AddModifier(new StatModifier(magicDamagePercent, StatModType.PercentAdd, this));
        if (physicalPenetrationPercent != 0) character.physicalPenetration.AddModifier(new StatModifier(physicalPenetrationPercent, StatModType.PercentAdd, this));
        if (magicPenetrationPercent != 0) character.magicPenetration.AddModifier(new StatModifier(magicPenetrationPercent, StatModType.PercentAdd, this));
        if (criticalChancePercent != 0) character.criticalChance.AddModifier(new StatModifier(criticalChancePercent, StatModType.PercentAdd, this));
        if (criticalDamagePercent != 0) character.criticalDamage.AddModifier(new StatModifier(criticalDamagePercent, StatModType.PercentAdd, this));

        //Defense
        if (physicalDefense != 0) character.physicalDefense.AddModifier(new StatModifier(physicalDefense, StatModType.Flat, this));
        if (magicDefense != 0) character.magicDefense.AddModifier(new StatModifier(magicDefense, StatModType.Flat, this));
        if (physicalResistance != 0) character.physicalResistance.AddModifier(new StatModifier(physicalResistance, StatModType.Flat, this));
        if (magicResistance != 0) character.magicResistance.AddModifier(new StatModifier(magicResistance, StatModType.Flat, this));
        if (criticalResistance != 0) character.criticalResistance.AddModifier(new StatModifier(criticalResistance, StatModType.Flat, this));

        //Other
        if (speed != 0) character.speed.AddModifier(new StatModifier(speed, StatModType.Flat, this));
        if (health != 0) character.health.maxVal.AddModifier(new StatModifier(health, StatModType.Flat, this));
        if (healthRegen != 0) character.healthRegen.AddModifier(new StatModifier(healthRegen, StatModType.Flat, this));
        if (mana != 0) character.mana.maxVal.AddModifier(new StatModifier(mana, StatModType.Flat, this));
        if (manaRegen != 0) character.manaRegen.AddModifier(new StatModifier(manaRegen, StatModType.Flat, this));

        //Other in Percent
        if (speedPercent != 0) character.speed.AddModifier(new StatModifier(speedPercent, StatModType.PercentAdd, this));
        if (healthPercent != 0) character.health.maxVal.AddModifier(new StatModifier(healthPercent, StatModType.PercentAdd, this));
        if (manaPercent != 0) character.mana.maxVal.AddModifier(new StatModifier(manaPercent, StatModType.PercentAdd, this));
    }

    public void Unequip(Character character)
    {
        //Main
        if (health != 0 || healthPercent != 0) character.health.maxVal.RemoveAllModifiersFromSource(this);
        if (healthRegen != 0) character.healthRegen.RemoveAllModifiersFromSource(this);
        if (mana != 0 || manaPercent != 0) character.mana.maxVal.RemoveAllModifiersFromSource(this);
        if (manaRegen != 0) character.manaRegen.RemoveAllModifiersFromSource(this);
        if (speed != 0 || speedPercent != 0) character.speed.RemoveAllModifiersFromSource(this);

        //Attack
        if (physicalDamage != 0 || physicalDamagePercent != 0) character.physicalDamage.RemoveAllModifiersFromSource(this);
        if (magicDamage != 0 || magicDamagePercent != 0) character.magicDamage.RemoveAllModifiersFromSource(this);
        if (physicalPenetration != 0 || physicalPenetrationPercent != 0) character.physicalPenetration.RemoveAllModifiersFromSource(this);
        if (magicPenetration != 0 || magicPenetrationPercent != 0) character.magicPenetration.RemoveAllModifiersFromSource(this);
        if (criticalChance != 0 || criticalChancePercent != 0) character.criticalChance.RemoveAllModifiersFromSource(this);
        if (criticalDamage != 0 || criticalDamagePercent != 0) character.criticalDamage.RemoveAllModifiersFromSource(this);

        //Defense
        if (physicalDefense != 0) character.physicalDefense.RemoveAllModifiersFromSource(this);
        if (magicDefense != 0) character.magicDefense.RemoveAllModifiersFromSource(this);
        if (physicalResistance != 0) character.physicalResistance.RemoveAllModifiersFromSource(this);
        if (magicResistance != 0) character.magicResistance.RemoveAllModifiersFromSource(this);
        if (criticalResistance != 0) character.criticalResistance.RemoveAllModifiersFromSource(this);
    }
    [Space]
    [Header("Tool Stats")]
    public int ToolDmg;
    public bool fillable = false;
    [Range(0, 100)]
    public int filled = 0;
    [Space]
    [Header("Weapon Stats")]
    public bool isWeapon;
    public WeaponType weaponType = WeaponType.Undefined;
    public float damage;
    public float penetration;
    public float critChance;
    public float critDamage;
    [Space]
    [Header("Crops Options")]
    public Crop crop;
    [Space]
    [Header("Size of Placeable Object")]
    public bool isLarge = false;
    public int width = 1;
    public int height = 1;

    //BOUNCING EFF DATA
    //[Tooltip("XReducer will slow down horizontal axis ( left right top bottom movement )")]
    //[Range(1f, 2.5f)]
    //public float YReducer = 1.5f;

    //[Tooltip("YReducer will slow down vertical axis ( height of the bounce )")]
    //[Range(1f, 2.5f)]
    //public float XReducer = 1.5f;

    //public int numberOfBounces = 3;

    //[Tooltip("Amount of vertical force")]
    //public float velocity = 10;

    //[Tooltip("Amount of horizontal force")]
    //public float horizontalForce = 2;

    //public float gravity = -30;

    //[Tooltip("Tag of entity who can collect this item")]
    //public string collectorTag = "Player";

    //[Tooltip("When can Player pick up item")]
    //public PickUpType pickUpType = PickUpType.AFTER;

    //[Tooltip("It will create small shadow after last bounce")]
    //public bool shadow = true;

    //public float destroyTime = 0f;

    //[Tooltip("When Item hits the wall with that tag")]
    //public string[] colliderTags;
}
