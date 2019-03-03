using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject humanPlayer;
    public GameObject aiPlayer;

    public GameObject dice;

    void Start()
    {
        int playerStarting = Random.Range(0, 2);
        print(playerStarting);
        if (playerStarting == 0)
        {
            HumanMove();
        }
        else
        {
            AiMove();
        }
    }

    void Update()
    {

    }

    private void HumanMove()
    {
        humanPlayer.SetActive(true);
        aiPlayer.SetActive(false);
    }

    private void AiMove()
    {
        aiPlayer.SetActive(true);
        humanPlayer.SetActive(false);
    }
}
