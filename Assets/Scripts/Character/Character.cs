using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Stat
{
    public CharacterStat maxVal;
    //public int maxVal;
    public float currVal;

    public Stat(float curr, float max)
    {
        //maxVal = max;
        maxVal.BaseValue = max;
        currVal = curr;
    }

    internal void Subtract(float amount)
    {
        currVal = currVal - amount <= 0 ? 0 : currVal - amount;
    }

    internal void Add(float amount)
    {
        currVal = currVal + amount > maxVal.Value ? maxVal.Value : currVal + amount;
    }

    internal void SetToMax()
    {
        currVal = maxVal.Value;
    }
}

public class Character : MonoBehaviour, IDamageable
{
    [Header("Main Stats")]
    public Stat level;
    public Stat experience;
    public Stat health;
    public Stat mana;
    public CharacterStat speed;
    public CharacterStat healthRegen;
    public CharacterStat manaRegen;

    //STATS
    [Header("Attack Stats")]
    public CharacterStat physicalDamage;
    public CharacterStat magicDamage;
    public CharacterStat physicalPenetration;
    public CharacterStat magicPenetration;
    public CharacterStat criticalChance;
    public CharacterStat criticalDamage;
    [Header("Defense Stats")]
    public CharacterStat physicalDefense;
    public CharacterStat magicDefense;
    public CharacterStat physicalResistance;
    public CharacterStat magicResistance;
    public CharacterStat criticalResistance;
    [Space]
    [Header("Status Bars and Level Stats Data")]
    [SerializeField] StatusBar hpBar;
    [SerializeField] StatusBar manaBar;
    [SerializeField] LevelStatsData levelStatsData;
    [SerializeField] EquipedItemsData equipedItems;
    [Space]
    public bool isDead;
    public bool noMana;
    public float regenCooldown = 0;

    DisableControls disableControls;
    PlayerRespawn playerRespawn;

    private TimeAgent timeAgent;

    private void Awake()
    {
        timeAgent = GetComponent<TimeAgent>();
        disableControls = GetComponent<DisableControls>();
        playerRespawn = GetComponent<PlayerRespawn>();
        
    }

    private void Start()
    {
        UpdateHpBar();
        UpdateManaBar();
        EquipItemsOnStart();
        level.maxVal.BaseValue = levelStatsData.levels.Count;
        //level.currVal = 1;

        SetStatsBasedOnLevel();

        timeAgent.onTimeTick += RegenHealth;
        timeAgent.onTimeTick += RegenMana;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            TakeDamage(15);
        }
    }

    private void EquipItemsOnStart()
    {
        for (int i = 0; i < equipedItems.equipedItems.Count; i++)
        {
            if (equipedItems.equipedItems[i].item != null)
            {
                equipedItems.equipedItems[i].item.Equip(transform.GetComponent<Character>());
            }
        }
    }

    private void SetStatsBasedOnLevel()
    {
        int lvl = (int)level.currVal;
        health.currVal = 30;
        health.maxVal.BaseValue = levelStatsData.levels[lvl - 1].health;
        healthRegen.BaseValue = levelStatsData.levels[lvl - 1].healthRegen;
        mana.maxVal.BaseValue = levelStatsData.levels[lvl - 1].mana;
        manaRegen.BaseValue = levelStatsData.levels[lvl - 1].manaRegen;
        mana.currVal = 10;
        experience.maxVal.BaseValue = levelStatsData.levels[lvl - 1].experience;
        speed.BaseValue = levelStatsData.levels[lvl - 1].speed;

        //Attack
        physicalDamage.BaseValue = levelStatsData.levels[lvl - 1].physicalAttack;
        magicDamage.BaseValue = levelStatsData.levels[lvl - 1].magicAttack;
        physicalPenetration.BaseValue = levelStatsData.levels[lvl - 1].physicalPenetration;
        magicPenetration.BaseValue = levelStatsData.levels[lvl - 1].magicPenetration;
        criticalChance.BaseValue = levelStatsData.levels[lvl - 1].criticalChance;
        criticalDamage.BaseValue = levelStatsData.levels[lvl - 1].criticalDamage;

        //Defense
        physicalDefense.BaseValue = levelStatsData.levels[lvl - 1].physicalDefense;
        magicDefense.BaseValue = levelStatsData.levels[lvl - 1].magicDefense;
        physicalResistance.BaseValue = levelStatsData.levels[lvl - 1].physicalResistance;
        magicResistance.BaseValue = levelStatsData.levels[lvl - 1].magicResistance;
        criticalResistance.BaseValue = levelStatsData.levels[lvl - 1].critialResistance;
    }

    private void UpdateHpBar()
    {
        hpBar.Set(health.currVal, health.maxVal.Value);
    }

    private void UpdateManaBar()
    {
        manaBar.Set(mana.currVal, mana.maxVal.Value);
    }

    private void RegenHealth()
    {
        if (regenCooldown > 0) 
        {
            regenCooldown -= 1;
            return;
        }

        if (health.currVal < health.maxVal.Value)
        {
            Heal(healthRegen.Value);
        }
    }

    private void RegenMana()
    {
        if (mana.currVal < mana.maxVal.Value)
        {
            RestoreMana(manaRegen.Value);
        }
    }

    // Dmg take
    public void TakeDamage(float amount)
    {
        if(isDead) return;

        health.Subtract(amount);
        if(health.currVal <= 0)
        {
            Dead();
        }

        UpdateHpBar();
        regenCooldown = 2;
    }

    private void Dead()
    {
        isDead = true;
        disableControls.DisableControl();
        playerRespawn.StartRespawn();
    }

    // Heal 
    public void Heal(float amount)
    {
        health.Add(amount);

        UpdateHpBar();
    }

    public void FullHeal()
    {
        health.SetToMax();

        UpdateHpBar();
    }

    // Mana
    public void UseMana(float amount)
    {
        mana.Subtract(amount);

        if(mana.currVal <= 0)
        {
            noMana = true;
        }

        UpdateManaBar();
    }

    public void RestoreMana(float amount)
    {
        mana.Add(amount);

        UpdateManaBar();
    }

    public void RestoreManaToMax()
    {
        mana.SetToMax();

        UpdateManaBar();
    }

    public void FullRest(float amount)
    {
        health.SetToMax();
        UpdateHpBar();
        mana.SetToMax();
        UpdateManaBar();
    }

    public void CalculateDamage(ref float damage)
    {
        
    }

    public void ApplyDamage(float damage)
    {
        TakeDamage(damage);
    }

    public void CheckState()
    {
        
    }
}
