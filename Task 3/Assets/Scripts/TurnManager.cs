using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public GameObject level;
    public GameObject gameInfo;
    public GameObject menuPawns;
    public GameObject levelPawns;
    public Sprite[] diceSides;
    public GameObject[] clickablePawns;
    public float diceRollDuration;

    private const string URL = "https://z1.data-qubit.com";

    private bool aiPawn1Finished;
    private bool aiPawn2Finished;
    private bool aiPawn3Finished;
    private bool humanPawn1Finished;
    private bool humanPawn2Finished;
    private bool humanPawn3Finished;
    private int lastDiceRoll;
    public int aiPawn1Progress;
    public int aiPawn2Progress;
    public int aiPawn3Progress;
    public int humanPawn1Progress;
    public int humanPawn2Progress;
    public int humanPawn3Progress;
    private int aiDiceRoll;
    private int aiGameWon;
    private string lastAiMove;
    private Coroutine rollDiceCoroutine;
    private GameObject dice;
    private GameObject diceRollingInfo;
    private GameObject aiMoveInfo;
    private GameObject humanMoveInfo;
    private GameObject aiPawnsInfo;
    private GameObject humanPawnsInfo;
    private GameObject humanPawnsInfoRed;
    private GameObject humanPawnsInfoGreen;
    private GameObject humanPawnsInfoBlue;
    private GameObject aiPawnsInfoRed;
    private GameObject aiPawnsInfoGreen;
    private GameObject aiPawnsInfoBlue;
    private GameObject levelPawnAiRed;
    private GameObject levelPawnAiGreen;
    private GameObject levelPawnAiBlue;
    private GameObject levelPawnHumanRed;
    private GameObject levelPawnHumanGreen;
    private GameObject levelPawnHumanBlue;
    private LevelManager levelManagerScript;

    private void Awake()
    {
        // Game Info Section
        dice = gameInfo.transform.GetChild(0).gameObject;
        diceRollingInfo = gameInfo.transform.GetChild(1).gameObject;
        aiMoveInfo = gameInfo.transform.GetChild(2).gameObject;
        humanMoveInfo = gameInfo.transform.GetChild(3).gameObject;
        // Pawns Info Section
        aiPawnsInfo = menuPawns.transform.GetChild(0).gameObject;
        humanPawnsInfo = menuPawns.transform.GetChild(1).gameObject;
        aiPawnsInfoRed = aiPawnsInfo.transform.GetChild(0).gameObject;
        aiPawnsInfoGreen = aiPawnsInfo.transform.GetChild(1).gameObject;
        aiPawnsInfoBlue = aiPawnsInfo.transform.GetChild(2).gameObject;
        humanPawnsInfoRed = humanPawnsInfo.transform.GetChild(0).gameObject;
        humanPawnsInfoGreen = humanPawnsInfo.transform.GetChild(1).gameObject;
        humanPawnsInfoBlue = humanPawnsInfo.transform.GetChild(2).gameObject;
        // Level Pawns Section
        levelPawnHumanRed = levelPawns.transform.GetChild(0).gameObject;
        levelPawnHumanGreen = levelPawns.transform.GetChild(1).gameObject;
        levelPawnHumanBlue = levelPawns.transform.GetChild(2).gameObject;
        levelPawnAiRed = levelPawns.transform.GetChild(3).gameObject;
        levelPawnAiGreen = levelPawns.transform.GetChild(4).gameObject;
        levelPawnAiBlue = levelPawns.transform.GetChild(5).gameObject;
        // Other Variables
        levelManagerScript = GetComponent<LevelManager>();
    }

    private void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            Invoke("HumanMove", 2f);
        }
        else
        {
            Invoke("AiMove", 2f);
        }
    }

    private int RollDice()
    {
        return Random.Range(1, 7);
    }

    private void HumanMove()
    {
        humanMoveInfo.SetActive(true);
        humanPawnsInfo.SetActive(true);
        aiMoveInfo.SetActive(false);
        aiPawnsInfo.SetActive(false);

        rollDiceCoroutine = StartCoroutine(PlayDiceAnimation());
        StartCoroutine(StopDiceAnimation(false, diceRollDuration));
    }

    private void AiMove()
    {
        aiMoveInfo.SetActive(true);
        aiPawnsInfo.SetActive(true);
        humanMoveInfo.SetActive(false);
        humanPawnsInfo.SetActive(false);

        rollDiceCoroutine = StartCoroutine(PlayDiceAnimation());
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        AiServerMove();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        StartCoroutine(StopDiceAnimation(true, diceRollDuration));
    }

    private IEnumerator PlayDiceAnimation()
    {
        diceRollingInfo.SetActive(true);
        while (true)
        {
            dice.GetComponent<Image>().sprite = diceSides[RollDice() - 1];
            yield return new WaitForSeconds(0.125f);
        }
    }

    private IEnumerator StopDiceAnimation(bool aiTurn, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StopCoroutine(rollDiceCoroutine);
        lastDiceRoll = RollDice();
        if (aiTurn)
        {
            lastDiceRoll = aiDiceRoll;
        }
        else
        {
            EnableUserClicks();
        }
        dice.GetComponent<Image>().sprite = diceSides[lastDiceRoll - 1];
        diceRollingInfo.SetActive(false);
    }

    private void EnableUserClicks()
    {
        foreach (GameObject pawn in clickablePawns)
        {
            if (pawn.GetComponent<PawnScript>() != null)
            {
                pawn.GetComponent<PawnScript>().SetInteractable();
            }
            if (pawn.GetComponent<LevelPawnScript>() != null)
            {
                pawn.GetComponent<LevelPawnScript>().SetInteractable();
            }
        }
    }

    private void DisableUserClicks()
    {
        foreach (GameObject pawn in clickablePawns)
        {
            if (pawn.GetComponent<PawnScript>() != null)
            {
                pawn.GetComponent<PawnScript>().SetNonInteractable();
            }
            if (pawn.GetComponent<LevelPawnScript>() != null)
            {
                pawn.GetComponent<LevelPawnScript>().SetNonInteractable();
            }
        }
    }

    private async System.Threading.Tasks.Task AiServerMove()
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"get_next\"}", Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                lastAiMove = content.Substring(30);
                lastAiMove = lastAiMove.Remove(lastAiMove.Length - 13);
                print("AI Move: " + lastAiMove);

                string[] aiMoves = lastAiMove.Split(',');
                int newAiPawn1Move = int.Parse(aiMoves[0].Trim());
                int newAiPawn2Move = int.Parse(aiMoves[1].Trim());
                int newAiPawn3Move = int.Parse(aiMoves[2].Trim());
                aiDiceRoll = int.Parse(aiMoves[3].Trim());
                aiGameWon = int.Parse(aiMoves[4].Trim());

                StartCoroutine(OperateAiPawns(newAiPawn1Move, newAiPawn2Move, newAiPawn3Move, diceRollDuration));
            }
        }   
    }

    private IEnumerator OperateAiPawns(int newAiPawn1Move, int newAiPawn2Move, int newAiPawn3Move, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (newAiPawn1Move != aiPawn1Progress)
        {
            ClickOnPawn(1, true, newAiPawn1Move - aiPawn1Progress);
            aiPawn1Progress = newAiPawn1Move;
        }
        if (newAiPawn2Move != aiPawn2Progress)
        {
            ClickOnPawn(2, true, newAiPawn2Move - aiPawn2Progress);
            aiPawn2Progress = newAiPawn2Move;
        }
        if (newAiPawn3Move != aiPawn3Progress)
        {
            ClickOnPawn(3, true, newAiPawn3Move - aiPawn3Progress);
            aiPawn3Progress = newAiPawn3Move;
        }
    }

    public void ClickOnPawn(int pawnSelected, bool aiPlayer, int pawnMoveTiles)
    {
        GameObject pawn;
        GameObject menuPawn;
        int pawnProgress = 0;
        if (aiPlayer)
        {
            switch (pawnSelected)
            {
                default:
                case 1:
                    pawn = levelPawnAiRed;
                    menuPawn = aiPawnsInfoRed;
                    pawnProgress = aiPawn1Progress + pawnMoveTiles;
                    break;
                case 2:
                    pawn = levelPawnAiGreen;
                    menuPawn = aiPawnsInfoGreen;
                    pawnProgress = aiPawn2Progress + pawnMoveTiles;
                    break;
                case 3:
                    pawn = levelPawnAiBlue;
                    menuPawn = aiPawnsInfoBlue;
                    pawnProgress = aiPawn3Progress + pawnMoveTiles;
                    break;
            }
            Invoke("HumanMove", 1f);
        }
        else
        {
            pawnMoveTiles = lastDiceRoll;
            DisableUserClicks();
            switch (pawnSelected)
            {
                default:
                case 1:
                    pawn = levelPawnHumanRed;
                    menuPawn = humanPawnsInfoRed;
                    pawnProgress = humanPawn1Progress + pawnMoveTiles;
                    humanPawn1Progress += pawnMoveTiles;
                    //TODO Turning back
                    break;
                case 2:
                    pawn = levelPawnHumanGreen;
                    menuPawn = humanPawnsInfoGreen;
                    pawnProgress = humanPawn2Progress + pawnMoveTiles;
                    humanPawn2Progress += pawnMoveTiles;
                    //TODO Turning back
                    break;
                case 3:
                    pawn = levelPawnHumanBlue;
                    menuPawn = humanPawnsInfoBlue;
                    pawnProgress = humanPawn3Progress + pawnMoveTiles;
                    humanPawn3Progress += pawnMoveTiles;
                    //TODO Turning back
                    break;
            }
            Invoke("AiMove", 1f);
        }

        if (!pawn.activeSelf)
        {
            pawn.SetActive(true);
            menuPawn.SetActive(false);
        }

        // Pawn Movement
        pawn.transform.localPosition = level.transform.GetChild(pawnProgress - 1).localPosition;
        var postEffectTile = GetSpecialTile(pawnProgress);
        if (postEffectTile != 0)
        {
            pawn.transform.localPosition = level.transform.GetChild(postEffectTile - 1).localPosition;
            if (aiPlayer)
            {
                switch (pawnSelected)
                {
                    default:
                    case 1:
                        pawnProgress = postEffectTile;
                        break;
                    case 2:
                        pawnProgress = postEffectTile;
                        break;
                    case 3:
                        pawnProgress = postEffectTile;
                        break;
                }
            }
            else
            {
                switch (pawnSelected)
                {
                    default:
                    case 1:
                        humanPawn1Progress = postEffectTile;
                        break;
                    case 2:
                        humanPawn2Progress = postEffectTile;
                        break;
                    case 3:
                        humanPawn3Progress = postEffectTile;
                        break;
                }
            }

        }
    }

    private int GetSpecialTile(int tile)
    {
        var tunnels = levelManagerScript.GetLevelTunnels();
        var snakes = levelManagerScript.GetLevelSnakes();
        if (tunnels.Contains(tile))
        {
            var tileIndex = Array.IndexOf(tunnels, tile); 
            if (tileIndex % 2 == 0)
            {
                return tunnels[tileIndex + 1];
            }
        }
        if (snakes.Contains(tile))
        {
            var tileIndex = Array.IndexOf(snakes, tile);
            if (tileIndex % 2 == 0)
            {
                return snakes[tileIndex + 1];
            }
        }
        return 0;
    }

    //TODO Winning game state
    private void GameWonByAi()
    {
        if (aiGameWon == 1)
        {
            print("AI Won the Game!!!");
        }
    }
}
