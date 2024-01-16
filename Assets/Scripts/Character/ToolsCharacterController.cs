using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolsCharacterController : MonoBehaviour
{
    CharacterController2D character;
    Rigidbody2D rgdbd2d;
    ToolbarController toolbarController;
    Animator animator;
    AttackController attackController;
    [SerializeField] float offsetDistance = 1f;
    //[SerializeField] float sizeOfInteractableArea = 1.2f;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] float maxDistance = 1.5f;
    [SerializeField] ToolAction onTilePickUp;
    [SerializeField] IconHighlight iconHighlight;

    ShowPanelsController showPanelsController;

    //COOLDOWN
    private float cooldownTime = 0.6f;
    private float nextOnAction = 0;

    Vector3Int selectedTilePosition;
    bool selectable;

    private void Awake()
    {
        character = GetComponent<CharacterController2D>();
        rgdbd2d = GetComponent<Rigidbody2D>();
        toolbarController = GetComponent<ToolbarController>();
        animator = GetComponent<Animator>();
        showPanelsController = GetComponent<ShowPanelsController>();
        attackController = GetComponent<AttackController>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            WeaponAction();
        }

        SelectTile();
        CanSelectCheck();
        Marker();

        if(Input.GetMouseButtonDown(0) && Time.time > nextOnAction && showPanelsController.AllPanelsAreClosed())
        {
            nextOnAction = Time.time + cooldownTime;

            if(UseToolWorld())
            {
                return;
            }
            UseToolGrid();
        }
    }

    private void WeaponAction()
    {
        Item item = toolbarController.GetItem;
        if(item == null) { return; }

        if(!item.isWeapon) { return; }

        attackController.Attack(item, character.lastMotionVector);
    }

    private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
    }

    void CanSelectCheck()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        iconHighlight.CanSelect = selectable;

        //TEST MOJ KOD
        if(selectable)
        {
            List<Vector3Int> listPositions = CreateListOfPositions(selectedTilePosition, iconHighlight.itemWidth, iconHighlight.itemHeight);

            bool isOcupied = GameManager.instance.GetComponent<PlaceableObjectsReferenceManager>().Check(listPositions);
            iconHighlight.SetColor(!isOcupied);
        }
    }

    private List<Vector3Int> CreateListOfPositions(Vector3Int startPosition, int width, int height)
    {
        List<Vector3Int> list = new List<Vector3Int>();


        int heightHelper = 0;

        while (heightHelper < height)
        {
            for (int i = 0; i < width; i++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + i, startPosition.y + heightHelper, startPosition.z);
                list.Add(position);
            }

            heightHelper++;
        }
        return list;
    }

    private void Marker()
    {
        markerManager.markedCellPosition = selectedTilePosition;
        iconHighlight.cellPosition = selectedTilePosition;
    }

    private bool UseToolWorld()
    {
        Vector2 position = rgdbd2d.position + character.lastMotionVector * offsetDistance;

        Item item = toolbarController.GetItem;
        if(item == null) return false;
        if (item.onAction == null) return false;

        animator.SetTrigger("act");
        bool complete = item.onAction.OnApply(position, item.ToolDmg);

        if (complete)
        {
            if (item.onItemUsed != null)
            {
                item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
            }
        }

        return complete;
    }

    private void UseToolGrid()
    {
        if (selectable) 
        {
            Item item = toolbarController.GetItem;

            if (item == null) 
            {
                PickUpTile();
                return;
            }

            if (item.onTileMapAction == null) return;

            animator.SetTrigger("act");
            bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, tileMapReadController, item);

            if(complete)
            {
                if(item.onItemUsed != null)
                {
                    item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
                }
            }
        }
    }

    private void PickUpTile()
    {
        if (onTilePickUp == null) return;

        onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
    }
}
