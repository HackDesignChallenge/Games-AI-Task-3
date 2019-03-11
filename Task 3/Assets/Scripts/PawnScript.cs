using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnScript : MonoBehaviour
{
    public GameObject gameManager;
    public bool isInteractable;
    private TurnManager turnManager;

    private void Awake()
    {
        turnManager = gameManager.GetComponent<TurnManager>();
    }

    public void ClickRedHumanPawn()
    {
        turnManager.ClickOnPawn(1, false, 1);
    }

    public void ClickGreenHumanPawn()
    {
        turnManager.ClickOnPawn(2, false, 1);
    }

    public void ClickBlueHumanPawn()
    {
        turnManager.ClickOnPawn(3, false, 1);
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
