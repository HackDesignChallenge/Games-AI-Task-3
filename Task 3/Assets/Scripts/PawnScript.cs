using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnScript : MonoBehaviour
{
    public int currentPosition;
    public GameObject level;

    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject.Find("GameManager").GetComponent<TurnManager>().PawnClicked(hit.transform.gameObject);
        }
    }

    public void GoToPosition(int position)
    {
        currentPosition = position;
        Vector3 pawnPosition = level.transform.GetChild(position).position;
        pawnPosition.x = pawnPosition.x + 3;
        pawnPosition.y = pawnPosition.y + 1;
        pawnPosition.z = pawnPosition.z - 1;
        transform.position = pawnPosition;
    }
}
