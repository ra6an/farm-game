using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeType
{
    Apple,
    Peach,
    Orange,
    Pear
}

[Serializable]
public class TreeControllerData
{
    public bool hasFullyGrown;
    public int currGrowthStage = 0;
    public int ticksToNextStageLeft = 5;

    public bool hasFruit = false;
    public int ticksToNextFruit = 5;

    public TreeControllerData()
    {
        
    }
}

[RequireComponent(typeof(TimeAgent))]
public class TreeController : MonoBehaviour, IPersistant
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Animator fruitsAnimator;
    private GameObject fruitsGO;

    [SerializeField] Sprite[] sprites;
    public Item itemFruit;
    public int count;

    //[SerializeField] float spread = 0.7f;
    //[SerializeField] float probability = 0.5f;

    [SerializeField] int growthStages = 3;
    [SerializeField] int ticksToGrow = 5;

    //FRUIT
    [SerializeField] public bool isFruitTree;
    [SerializeField] GameObject fruits;
    [SerializeField] public TreeType fruit;
    [SerializeField] int ticksToGrowFruit = 5;
    public bool isHarvested = false;

    public TreeControllerData data;

    private bool isInit = false;
    private bool updateState = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        fruitsAnimator = this.transform.GetChild(0).GetComponent<Animator>();
        fruitsGO = this.transform.GetChild(0).gameObject;

        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += UpdateTreeData;

        if(data == null) data = new TreeControllerData();

        gameObject.GetComponent<ResourceNode>().ChangeHealth((data.currGrowthStage + 1) * 4);

        updateState = true;
        
        isInit = true;
    }

    private void Update()
    {
        if(isFruitTree && data.hasFruit && fruitsAnimator != null)
        {
            fruitsAnimator.SetBool(fruit.ToString(), true);
            this.transform.GetComponent<TreeInteract>().hasFruits = true;
            fruitsGO.SetActive(true);
        }

        if (updateState && isInit)
        {
            UpdateState();
        }

        if(isHarvested)
        {
            ResetFruitsData();
        }
    }

    public IEnumerator HarvestTreeAnimation()
    {
        fruitsAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.26f);
        fruitsGO.SetActive(false);
    }

    private void ResetFruitsData()
    {
        data.ticksToNextFruit = ticksToGrowFruit;
        data.hasFruit = false;
        StartCoroutine(HarvestTreeAnimation());

        isHarvested = false;
    }

    public void UpdateTreeData()
    {
        if (data == null) return;
        if (!data.hasFullyGrown)
        {
            ChangeGrowthStage();
        }
        
        if(data.hasFullyGrown && isFruitTree)
        {
            UpdateFruitGrowthStage();
        }
    }

    private void UpdateFruitGrowthStage()
    {
        if (data.ticksToNextFruit > 0)
        {
            data.ticksToNextFruit--;
        }

        if(data.ticksToNextFruit == 0 && !data.hasFruit)
        {
            data.hasFruit = true;
            fruitsAnimator.SetBool(fruit.ToString(), true);
            fruitsGO.SetActive(true);
            this.transform.GetComponent<TreeInteract>().hasFruits = true;
        }
    }

    public void UpdateState()
    {
        
        if (fruitsAnimator != null)
        {
            fruitsGO.SetActive(data.hasFruit);
            this.transform.GetComponent<TreeInteract>().hasFruits = data.hasFruit;
        }

        if(spriteRenderer != null)
        {
            ChangeSprite();
        }

        updateState = false;
    }

    public void ChangeSprite()
    {
        spriteRenderer.sprite = sprites[data.currGrowthStage];
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        animator.SetInteger("GrowthStage", data.currGrowthStage);
    }

    public void ChangeGrowthStage()
    {
        if (!CheckIfCanGrow() && data.currGrowthStage == 1) return;

        if (data.ticksToNextStageLeft > 0)
        {
            data.ticksToNextStageLeft--;
        }

        if(data.ticksToNextStageLeft == 0 && data.currGrowthStage < growthStages)
        {
            data.currGrowthStage++;
            data.ticksToNextStageLeft = ticksToGrow;
            gameObject.GetComponent<ResourceNode>().ChangeHealth((data.currGrowthStage + 1) * 4);
            ChangeSprite();
        }

        if(data.currGrowthStage == growthStages)
        {
            data.hasFullyGrown = true;
        }
    }

    public bool CheckIfCanGrow()
    {
        Vector2 sizeOfGrowth = new Vector2(2f, 2f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.transform.position, sizeOfGrowth, 0f);

        bool interupted = false;
        foreach (Collider2D col in colliders)
        {
            if(System.Enum.IsDefined(typeof(NodeType), col.tag) && col.transform.position != transform.position)
            {
                interupted = true;
            }
        }

        return !interupted;
    }

    public void LoadData(string jsonString)
    {
        data = JsonUtility.FromJson<TreeControllerData>(jsonString);
        updateState = true;
    }

    public string SaveData()
    {
        return JsonUtility.ToJson(data);
    }
}
