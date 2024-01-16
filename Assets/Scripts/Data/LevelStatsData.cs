using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelStats
{
    [Header("Basic")]
    public float health;
    public float healthRegen;
    public float mana;
    public float manaRegen;
    public float speed;
    public int experience;
    [Space]
    [Header("Attack")]
    public float physicalAttack;
    public float magicAttack;
    public float physicalPenetration;
    public float magicPenetration;
    public float criticalChance;
    public float criticalDamage;
    [Space]
    [Header("Defense")]
    public float physicalDefense;
    public float magicDefense;
    public float physicalResistance;
    public float magicResistance;
    public float critialResistance;
}

[CreateAssetMenu(menuName = "Data/Level Stats Data")]
public class LevelStatsData : ScriptableObject
{
    public List<LevelStats> levels;
}
