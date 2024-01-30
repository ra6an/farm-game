using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsPanel : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadGameSlots;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SingleSlot[] singleSlots;

    private bool isLoadingGame = false;
    private bool dataIsLoaded = false;

    private void Awake()
    {
        singleSlots = this.GetComponentsInChildren<SingleSlot>();
    }

    private void Update()
    {
        if (dataIsLoaded)
        {
            GameData gd = DataPersistentManager.instance.gameData;
            GameSceneManager.instance.OnGameStartTransition(gd.activeSceneName);
            DataPersistentManager.instance.LoadGame();
            dataIsLoaded = false;
        }
    }

    public void OnSingleSlotClicked(SingleSlot slot)
    {
        DisableMenuButtons();

        DataPersistentManager.instance.ChangeSelectedProfileId(slot.GetProfileId());

        if(!isLoadingGame)
        {
            DataPersistentManager.instance.NewGame();

            GameSceneManager.instance.OnGameStartTransition("MainScene");

            DataPersistentManager.instance.LoadGame();
        } 
        else
        {
            DataPersistentManager.instance.GetLoadData();

            while (DataPersistentManager.instance.gameData == null)
            {
                continue;
            }

            dataIsLoaded = true;
        }

    }

    private void DisableMenuButtons()
    {
        foreach(SingleSlot slot in singleSlots)
        {
            slot.SetInteractable(false);
        }
        backButton.interactable = false;
    }

    public void OnBackClicked()
    {
        loadGameSlots.SetActive(false);
    }

    public void ActivateMenu(bool _isLoadingGame)
    {
        loadGameSlots.SetActive(true);
        isLoadingGame = _isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistentManager.instance.GetAllProfilesGameData();

        GameObject firstSelected = backButton.gameObject;
        foreach(SingleSlot slot in singleSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(slot.GetProfileId(), out profileData);
            slot.Set(profileData);
            if(profileData == null && isLoadingGame)
            {
                slot.SetInteractable(false);
            } else
            {
                slot.SetInteractable(true);
                if(firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = slot.gameObject;
                }
            }
        }

        this.gameObject.SetActive(true);

        StartCoroutine(this.SetFirstSelected(firstSelected));
    }
}
