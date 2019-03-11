using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPawnScript : MonoBehaviour
{
    public GameObject gameManager;
    public bool isInteractable;
    private TurnManager turnManager;

    private void Awake()
    {
        turnManager = gameManager.GetComponent<TurnManager>();
    }

    private void OnMouseDown()
    {
        if (isInteractable)
        {
            switch (gameObject.name)
            {
                case "Hero Pawn 1":
                    turnManager.ClickOnPawn(1, false, 1);
                    break;
                case "Hero Pawn 2":
                    turnManager.ClickOnPawn(2, false, 1);
                    break;
                case "Hero Pawn 3":
                    turnManager.ClickOnPawn(3, false, 1);
                    break;
            }
        }
    }

    public void SetInteractable() 
    {
        isInteractable = true;
    }

    public void SetNonInteractable()
    {
        isInteractable = false;
    }
}
