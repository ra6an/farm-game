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
    public bool iconHighlight;
    public GameObject itemPrefab;

    [Header("Equipable Item")]
    public bool equipable = false;
    public TypeEquipable equipableType = TypeEquipable.Undefined;
    public int physicalDefense;
    public int magicDefense;
    public int physicalResistance;
    public int magicResistance;
    public int criticalResistance;
    public int speed;
    public int health;
    public int mana;

    [Header("Tool Stats")]
    public int ToolDmg;
    public bool fillable = false;
    [Range(0, 100)]
    public int filled = 0;

    [Header("Weapon Stats")]
    public bool isWeapon;
    public WeaponType weaponType = WeaponType.Undefined;
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int physicalPenetration = 0;
    public int magicPenetration = 0;
    [Range(0, 100)]
    public int criticalChance = 0;
    [Range(0, 100)]
    public int criticalDamage = 0;

    [Header("Crops Options")]
    public Crop crop;

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

    //public string type; //Napraviti da ima default vrijednosti, ja msm da se moze sa enum-om to uraditi
    //TOOLS
    //public string tool;
    //public int dmg;
    //WEAPONS
    //public int atk;
    //public int magAtk;
    //public int def;
    //public int magDef;
    //public int crit;
    //public int critDef;
    //POTIONS
    //public int heal;
    //public int mana;
    //BUFFS
    //public int manaRegen;
    //public int speedBoost;
    //public int atkBoost;
    //public int magAtkBoost;
    //public int defBoost;
    //public int critBoost;
    //public int critDefBoost;
}
