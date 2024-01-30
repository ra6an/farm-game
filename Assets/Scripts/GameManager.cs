using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        
        activeSceneName = SceneManager.GetActiveScene().name;
    }

    public string activeSceneName;
    public GameObject canvas;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController timeController;
    public DialogueSystem dialogueSystem;
    public ItemList itemsDB;
    public RecipeList recipesDB;
    public CropsList cropsDB;
    public OnScreenMessageSystem messageSystem;
    public ScreenTint screenTint;
    public PlaceableObjectContainer placeableObjects;

    public bool oneTimeLoader = true;

    public void SaveData(GameData data)
    {
        data.inventory = SerializeInventory();
        data.activeSceneName = GameSceneManager.instance.currentScene;
    }

    public void LoadData(GameData data)
    {
        player.SetActive(true);
        canvas.SetActive(true);

        //Debug.Log("tu smo");
        
        DeserializeInventory(data.inventory);
        activeSceneName = data.activeSceneName;
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
        if(inventoryContainer == null)
        {
            inventoryContainer = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
            inventoryContainer.Init();
        }

        if (data == "" || data == "{}" || data == null)
        {
            foreach(ItemSlot slot in inventoryContainer.slots)
            {
                slot.Clear();
            }
        } else
        {
            SlotsToSave deserializedInventory = JsonUtility.FromJson<SlotsToSave>(data);

            for (int i = 0; i < deserializedInventory.slots.Count; i++)
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

            Debug.Log(inventoryContainer.slots[0].item);
        }
    }

    public bool isOneTimeLoader()
    {
        return oneTimeLoader;
    }
}
