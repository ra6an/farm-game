using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int maxVal;
    public int currVal;

    public Stat(int curr, int max)
    {
        maxVal = max;
        currVal = curr;
    }

    internal void Subtract(int amount)
    {
        currVal = currVal - amount <= 0 ? 0 : currVal - amount;
    }

    internal void Add(int amount)
    {
        currVal = currVal + amount > maxVal ? maxVal : currVal + amount;
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }
}

public class Character : MonoBehaviour, IDamageable
{
    public Stat hp;
    public Stat mana;

    [SerializeField] StatusBar hpBar;
    [SerializeField] StatusBar manaBar;

    public bool isDead;
    public bool noMana;

    DisableControls disableControls;
    PlayerRespawn playerRespawn;

    private void Awake()
    {
        disableControls = GetComponent<DisableControls>();
        playerRespawn = GetComponent<PlayerRespawn>();
    }

    private void Start()
    {
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);
    }

    // Dmg take
    public void TakeDamage(int amount)
    {
        if(isDead) return;

        hp.Subtract(amount);
        if(hp.currVal <= 0)
        {
            Dead();
        }

        UpdateHpBar();
    }

    private void Dead()
    {
        isDead = true;
        disableControls.DisableControl();
        playerRespawn.StartRespawn();
    }

    // Heal 
    public void Heal(int amount)
    {
        hp.Add(amount);

        UpdateHpBar();
    }

    public void FullHeal()
    {
        hp.SetToMax();

        UpdateHpBar();
    }

    // Mana
    public void UseMana(int amount)
    {
        mana.Subtract(amount);

        if(mana.currVal <= 0)
        {
            noMana = true;
        }
    }

    public void RestoreMana(int amount)
    {
        mana.Add(amount);
    }

    public void RestoreManaToMax()
    {
        mana.SetToMax();
    }

    public void FullRest(int amount)
    {
        hp.SetToMax();
        UpdateHpBar();
        //mana.SetToMax();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            TakeDamage(15);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Heal(5);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FullHeal();
        }
    }

    public void CalculateDamage(ref int damage)
    {
        
    }

    public void ApplyDamage(int damage)
    {
        TakeDamage((int)damage);
    }

    public void CheckState()
    {
        
    }
}
