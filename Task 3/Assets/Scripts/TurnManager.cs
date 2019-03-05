using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject humanPlayer;
    public GameObject aiPlayer;

    public GameObject dice;

    private PawnScript aiPawn1;
    private PawnScript aiPawn2;
    private PawnScript aiPawn3;

    public int aiPawn1Position = 0;
    public int aiPawn2Position = 0;
    public int aiPawn3Position = 0;
    public int aiGameWon = 0;

    public int humanDiceRoll = 0;
    public int aiDiceRoll = 0;

    private readonly string url = "https://z1.data-qubit.com";

    private void Awake()
    {
        aiPawn1 = aiPlayer.transform.GetChild(0).GetComponent<PawnScript>();
        aiPawn2 = aiPlayer.transform.GetChild(1).GetComponent<PawnScript>();
        aiPawn3 = aiPlayer.transform.GetChild(2).GetComponent<PawnScript>();
    }

    private void Start()
    {
        if (Random.Range(0, 2) == 0)
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

    private int RollDice()
    {
        return Random.Range(1, 7);
    }

    private void HumanMove()
    {
        humanDiceRoll = RollDice();
        print("Dice Roll: " + humanDiceRoll);
    }

    public void PawnClicked(GameObject pawn)
    {
        PawnScript pawnScript = pawn.GetComponent<PawnScript>();
        int newPawnPosition = pawnScript.currentPosition + humanDiceRoll;
        pawnScript.GoToPosition(newPawnPosition);
        Invoke("AiMove", 1.5f);
    }

    private void AiMove()
    {
        Invoke("PostAiMove", 0.5f);
        Invoke("HumanMove", 1.5f);
    }

    private async System.Threading.Tasks.Task PostAiMove()
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"get_next\"}", Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();

                string aiMove = content.Substring(30);
                aiMove = aiMove.Remove(aiMove.Length - 13);
                print("aiMove: " + aiMove);
                string[] aiMovesString = aiMove.Split(',');
                int[] aiMoves = new int[aiMovesString.Length];
                for (int i = 0; i < aiMoves.Length; i++)
                {
                    aiMoves[i] = int.Parse(aiMovesString[i].Trim());
                }
                aiDiceRoll = aiMoves[3];
                aiGameWon = aiMoves[4];

                if (aiPawn1Position != aiMoves[0])
                {
                    aiPawn1Position = aiMoves[0];
                    aiPawn1.GoToPosition(aiPawn1Position);
                }
                if (aiPawn2Position != aiMoves[1])
                {
                    aiPawn2Position = aiMoves[1];
                    aiPawn2.GoToPosition(aiPawn2Position);
                }
                if (aiPawn3Position != aiMoves[2])
                {
                    aiPawn3Position = aiMoves[2];
                    aiPawn3.GoToPosition(aiPawn3Position);
                }
            }
        }
    }

    private void GameWon()
    {
        print("Game Won!!!");
    }
}
