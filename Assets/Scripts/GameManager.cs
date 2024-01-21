using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class GameManager : MonoBehaviour, IDataPersistant
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < itemsDB.items.Count; i++)
        {
            itemsDB.items[i].id = i;
        }
    }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController timeController;
    public DialogueSystem dialogueSystem;
    public ItemList itemsDB;
    public RecipeList recipesDB;
    public OnScreenMessageSystem messageSystem;
    public ScreenTint screenTint;
    public PlaceableObjectContainer placeableObjects;

    public void SaveData(ref GameData data)
    {
        data.inventory = SerializeInventory();
    }

    public void LoadData(GameData data)
    {
        DeserializeInventory(data.inventory);
    }

    //INVENTORY SAVE/LOAD
    public class SlotsToSave
    {
        public List<ItemSlot> slots;

        public void Init(List<ItemSlot> listOfSlots)
        {
            slots = new List<ItemSlot>();
            foreach(ItemSlot slot in listOfSlots)
            {
                ItemSlot newSlot = new()
                {
                    item = slot.item,
                    quantity = slot.quantity
                };

                slots.Add(newSlot);
            }
        }
    }

    private string SerializeInventory()
    {
        string inv = "";

        SlotsToSave slots = new SlotsToSave();
        slots.Init(inventoryContainer.slots);

        inv = JsonUtility.ToJson(slots);

        return inv;
    }

    private void DeserializeInventory(string data)
    {
        if (data == "" || data == "{}") return;

        SlotsToSave deserializedInventory = JsonUtility.FromJson<SlotsToSave>(data);
        
        for(int i = 0; i < deserializedInventory.slots.Count; i++)
        {
            if (deserializedInventory.slots[i].item == -1)
            {
                inventoryContainer.slots[i].Clear();
            }
            else
            {
                inventoryContainer.slots[i].item = deserializedInventory.slots[i].item;
                inventoryContainer.slots[i].quantity = deserializedInventory.slots[i].quantity;
            }
        }
    }
}
