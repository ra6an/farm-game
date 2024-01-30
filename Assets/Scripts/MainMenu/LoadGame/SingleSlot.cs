using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] Color activeSlot;
    [SerializeField] Color inactiveSlot;
    [SerializeField] GameObject slotTitle;
    [SerializeField] GameObject saveDate;
    [SerializeField] GameObject playerLevel;
    [SerializeField] GameObject empty;
    [SerializeField] bool isEmpty = true;

    private Button singleSlotButton;

    private void Awake()
    {
        singleSlotButton = GetComponent<Button>();
    }

    public void Set(GameData data)
    {
        if (data == null)
        {
            empty.SetActive(true);
            saveDate.SetActive(false);
            slotTitle.SetActive(false);
            playerLevel.SetActive(false);

            //gameObject.GetComponent<Image>().color = inactiveSlot;
        } else
        {
            empty.SetActive(false);
            saveDate.SetActive(true);
            slotTitle.SetActive(true);
            playerLevel.SetActive(true);

            saveDate.GetComponent<Text>().text = $"{data.createdAt}";
            playerLevel.GetComponent<Text>().text = $"Player LVL: {data.playerLevel}";

            //gameObject.GetComponent<Image>().color = activeSlot;
        }
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        singleSlotButton.interactable = interactable;
    }
}
