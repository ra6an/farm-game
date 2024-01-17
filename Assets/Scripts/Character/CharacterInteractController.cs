using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController2D characterController;
    Rigidbody2D rgdbd2d;
    private InputManager inputManager;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    Character character;
    ShowPanelsController showPanelsController;
    [SerializeReference] HighlightController highlightController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        rgdbd2d = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        showPanelsController = GetComponent<ShowPanelsController>();
        inputManager = InputManager.instance;
    }

    private void Update()
    {
        Check();

        if (inputManager.GetKeyDown(KeybindingActions.Interact) && !showPanelsController.inventoryOpened)
        {
            Interact();
        }
    }

    private void Check()
    {
        Vector2 position = rgdbd2d.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] collider = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in collider)
        {
            Interactable hit = c.GetComponent<Interactable>();
            if (hit != null)
            {
                highlightController.Highlight(hit.gameObject);
                return;
            }
        }

        highlightController.Hide();
    }

    private void Interact() //DODATI OPCIJU ZA MOUSEOVER PREPOZNAVANJE I INTERACTANJE SA NAJBLIZIM OBJEKTOM U COLLIDERU
    {
        Vector2 position = rgdbd2d.position + characterController.lastMotionVector * offsetDistance;

        Collider2D[] collider = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in collider)
        {
            Interactable hit = c.GetComponent<Interactable>();

            if (hit != null)
            {
                hit.Interact(character);
                break;
            }
        }
    }
}
