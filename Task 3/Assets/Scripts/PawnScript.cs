using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnScript : MonoBehaviour
{
    public bool isReturning = false;
    public int currentPosition;

    private static readonly int MAX_FIELDS = 100;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GoToPosition(int diceRoll)
    {
        if (isReturning) { 
            if (currentPosition + diceRoll > MAX_FIELDS)
            {
                isReturning = true;
                currentPosition = 2 * MAX_FIELDS - currentPosition - diceRoll;
            }
            else
            {
                currentPosition += diceRoll;
            }
        }
        else
        {
            currentPosition -= diceRoll;
            if (currentPosition <= 0)
            {
                GameWon();
            }
        }
        //TUNNEL / SNAKE effect
        AnimateToPosition(currentPosition);
    }

    private void AnimateToPosition(int position)
    {

    }

    private void GameWon()
    {
        print("Game Won!!!");
    }
}
