using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Progress;
//
public class StorageContainerInteract : Interactable, IPersistant
{
    [SerializeField] bool opened;
    [SerializeField] AudioClip onOpenChest;
    [SerializeField] AudioClip onCloseChest;
    Animator animator;
    [SerializeField] ItemContainer itemContainer;
    private Character character;

    private void Update()
    {
        if(opened && Input.GetKeyUp(KeyCode.Tab))
        {
            Close(character);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (itemContainer == null)
        {
            Init();
        }
    }

    private void Init()
    {
            itemContainer = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
            itemContainer.Init();
    }

    public override void Interact(Character c)
    {
        character = c;
        if (!opened)
        {
            Open(c);
        }
        else
        {
            Close(c);
        }
    }
    //
    public void Open(Character character)
    {
        opened = true;
        animator.SetTrigger("OpenChest");

        if (!onOpenChest) return;
        AudioManager.instance.Play(onOpenChest);

        character.GetComponent<ItemContainerInteractController>().Open(itemContainer, transform);
    }

    public void Close(Character character)
    {
        opened = false;
        animator.SetTrigger("CloseChest");

        if (!onCloseChest) return;
        AudioManager.instance.Play(onCloseChest);

        character.GetComponent<ItemContainerInteractController>().Close();
    }

    [Serializable]
    public class SaveLootItemData
    {
        public int itemId;
        public int quantity;

        public SaveLootItemData(int id, int q)
        {
            itemId = id;
            quantity = q;
        }
    }

    [Serializable]
    public class ToSave
    {
        public List<SaveLootItemData> itemDatas;

        public ToSave()
        {
            itemDatas = new List<SaveLootItemData>();
        }
    }

    public string Read()
    {
        ToSave toSave = new ToSave();

        for(int i = 0; i < itemContainer.slots.Count; i++)
        {
            if (itemContainer.slots[i].item == null)
            {
                toSave.itemDatas.Add(new SaveLootItemData(-1, 0));
            } else
            {
                toSave.itemDatas.Add(new SaveLootItemData(
                    itemContainer.slots[i].item.id, 
                    itemContainer.slots[i].quantity
                    ));
            }
        }

        return JsonUtility.ToJson(toSave);
    }

    public void Load(string jsonString)
    {
        if (jsonString == "" || jsonString == "{}" || jsonString == null) return;
        if (itemContainer == null)
        {
            Init();
        }

        ToSave toLoad = JsonUtility.FromJson<ToSave>(jsonString);
        for(int i = 0; i < toLoad.itemDatas.Count; i++)
        {
            if (toLoad.itemDatas[i].itemId == -1)
            {
                itemContainer.slots[i].Clear();
            } else
            {
                itemContainer.slots[i].item = GameManager.instance.itemsDB.items[toLoad.itemDatas[i].itemId];
                itemContainer.slots[i].quantity = toLoad.itemDatas[i].quantity;
            }
        }
    }
}
