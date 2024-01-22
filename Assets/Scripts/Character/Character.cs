using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class Character : MonoBehaviour, IDamageable, IDataPersistant
{
    [Header("Main Stats")]
    public int level;
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
    //[SerializeField] EquipedItemsData equipedItems;
    [Space]
    public bool isDead = false;
    public bool noMana = false;
    public float regenCooldown = 0;

    //Recipe Lists
    //private RecipeList characterRecipeList;
    //private RecipeList workingBenchRecipeList;
    public RecipeList characterRecipeList;
    public RecipeList workingBenchRecipeList;

    DisableControls disableControls;
    PlayerRespawn playerRespawn;

    private TimeAgent timeAgent;

    private void Awake()
    {
        timeAgent = GetComponent<TimeAgent>();
        disableControls = GetComponent<DisableControls>();
        playerRespawn = GetComponent<PlayerRespawn>();
        
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    private void Start()
    {
        UpdateHpBar();
        UpdateManaBar();

        experience.maxVal.BaseValue = levelStatsData.levels[level - 1].experience;

        SetStatsBasedOnLevel();
        transform.GetComponent<EquipItemController>().Refresh();

        timeAgent.onTimeTick += RegenHealth;
        timeAgent.onTimeTick += RegenMana;
    }

    public void EquipItemsOnStart()
    {
        EquipedItemsData equipedItems = transform.GetComponent<EquipItemController>().equipedItemsData;

        if (equipedItems == null ||equipedItems.equipedItems.Count == 0) return;

        for (int i = 0; i < equipedItems.equipedItems.Count; i++)
        {
            if (equipedItems.equipedItems[i].item < 0) return;

            Item item = GameManager.instance.itemsDB.GetItemById(equipedItems.equipedItems[i].item);
            if (item != null)
            {
                item.Equip(transform.GetComponent<Character>());
            }
        }
    }

    private void SetStatsBasedOnLevel()
    {
        int lvl = level - 1;
        health.maxVal.BaseValue = levelStatsData.levels[lvl].health;
        healthRegen.BaseValue = levelStatsData.levels[lvl].healthRegen;
        mana.maxVal.BaseValue = levelStatsData.levels[lvl].mana;
        manaRegen.BaseValue = levelStatsData.levels[lvl].manaRegen;
        experience.maxVal.BaseValue = levelStatsData.levels[lvl].experience;
        speed.BaseValue = levelStatsData.levels[lvl].speed;

        //Attack
        physicalDamage.BaseValue = levelStatsData.levels[lvl].physicalAttack;
        magicDamage.BaseValue = levelStatsData.levels[lvl].magicAttack;
        physicalPenetration.BaseValue = levelStatsData.levels[lvl].physicalPenetration;
        magicPenetration.BaseValue = levelStatsData.levels[lvl].magicPenetration;
        criticalChance.BaseValue = levelStatsData.levels[lvl].criticalChance;
        criticalDamage.BaseValue = levelStatsData.levels[lvl].criticalDamage;

        //Defense
        physicalDefense.BaseValue = levelStatsData.levels[lvl].physicalDefense;
        magicDefense.BaseValue = levelStatsData.levels[lvl].magicDefense;
        physicalResistance.BaseValue = levelStatsData.levels[lvl].physicalResistance;
        magicResistance.BaseValue = levelStatsData.levels[lvl].magicResistance;
        criticalResistance.BaseValue = levelStatsData.levels[lvl].critialResistance;
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

    internal void UpdateStatusBars()
    {
        UpdateHpBar();
        UpdateManaBar();
    }

    // Persisting Data
    [Serializable]
    public class RecipeListIDs
    {
        public List<int> recipes;

        public void Init()
        {
            recipes = new List<int>();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
        data.playerLevel = level;
        data.playerExperience = experience.currVal;
        data.playerHealth = health.currVal;
        data.playerMana = mana.currVal;

        // Create empty list of recipe IDs
        RecipeListIDs characterRecipeIDs = new();
        characterRecipeIDs.Init();
        RecipeListIDs workingBenchRecipeIDs = new();
        workingBenchRecipeIDs.Init();

        // Add recipe IDs to lists
        foreach (CraftingRecipe recipe in characterRecipeList.recipes)
        {
            characterRecipeIDs.recipes.Add(GameManager.instance.recipesDB.GetRecipeId(recipe));
        }

        foreach (CraftingRecipe recipe in workingBenchRecipeList.recipes)
        {
            workingBenchRecipeIDs.recipes.Add(GameManager.instance.recipesDB.GetRecipeId(recipe));
        }

        // Serialize recipe ID lists
        string serializedCharacterRecipeIDs = JsonUtility.ToJson(characterRecipeIDs);
        string serializedWorkingBenchRecipeIDs = JsonUtility.ToJson(workingBenchRecipeIDs);

        // Save them to GameData
        data.characterRecipeList = serializedCharacterRecipeIDs;
        data.workingBenchRecipeList = serializedWorkingBenchRecipeIDs;
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
        level = data.playerLevel;
        experience.currVal = data.playerExperience;
        health.currVal = data.playerHealth;
        mana.currVal = data.playerMana;

        // Initialize scriptable object
        characterRecipeList = (RecipeList)ScriptableObject.CreateInstance(typeof(RecipeList));
        characterRecipeList.Init();
        workingBenchRecipeList = (RecipeList)ScriptableObject.CreateInstance(typeof(RecipeList));
        workingBenchRecipeList.Init();

        // Deserialize Json data
        RecipeListIDs deserializesCharacterRecipeIDs = JsonUtility.FromJson<RecipeListIDs>(data.characterRecipeList);
        RecipeListIDs deserializesWorkingBenchRecipeIDs = JsonUtility.FromJson<RecipeListIDs>(data.workingBenchRecipeList);

        // Add crafting recipes to variables
        foreach(int i in deserializesCharacterRecipeIDs.recipes)
        {
            characterRecipeList.recipes.Add(GameManager.instance.recipesDB.GetRecipeById(i));
        }

        foreach(int i in deserializesWorkingBenchRecipeIDs.recipes)
        {
            workingBenchRecipeList.recipes.Add(GameManager.instance.recipesDB.GetRecipeById(i));
        }

        // Set reference of crafting station known recipe list
        CraftingStationContainerInteractController csic = this.GetComponent<CraftingStationContainerInteractController>();
        csic.recipeList = (RecipeList)ScriptableObject.CreateInstance(typeof(RecipeList));
        csic.recipeList.Init();
        csic.recipeList.recipes = workingBenchRecipeList.recipes;

        Crafting crafting = this.GetComponent<Crafting>();
        crafting.recipeList = (RecipeList)ScriptableObject.CreateInstance(typeof (RecipeList));
        crafting.recipeList.Init();
        crafting.recipeList.recipes = characterRecipeList.recipes;

        UpdateStatusBars();
    }
}
