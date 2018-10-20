using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SimpleAIMovement : Movement {

    public Movement player;

    public GameObject targetPrefabl;
    public List<GameObject> targets;

    private void Update()
    {
        if (targets.Count == 0)
        {
            targets = new List<GameObject>();
            for (int i = 0; i < 1; i++)
                targets.Add(Instantiate(targetPrefabl, transform));

            Movement[] players = GameObject.FindObjectsOfType<Movement>();

            do
            {
                player = players[Random.Range(0, players.Length)];
            } while (player == this);

            targets[0].SetActive(false);
        }

        Square targetSquare;
        if (currentSquare.info.danger)
            targetSquare = PathFinderV2.main.NextStepToSaveSquareWithMoreOptions(currentSquare, 1);
        else
        {
            targetSquare = PathFinderV2.main.NextStepWithMoreOptions(currentSquare);
            //targetSquare = PathFinderV2.main.NextSquareTo(currentSquare, player.currentSquare, 25);

            //if (!targetSquare.Walkable(false, false) && currentSquare.bomb == null)
            //{
            //    GameObject.FindObjectOfType<Booming>().Bomb(currentSquare);
            //    transform.SetAsLastSibling();
            //    return;
            //}
        }
        //targetSquare.GetComponent<Image>().color = Color.blue;
        Move(targetSquare);

        //transform.SetAsFirstSibling();

        //List<Square> bestBombSquares = PathFinderV2.main.BestSquareToBomb(currentSquare, 5, 3, 3); //ZROB HUNTER IA

        if (!player)
            return;

        Square squareToBomb = PathFinderV2.main.BestSquareToBombInReach(currentSquare, player.currentSquare, 5, 3);

        //targets[0].transform.parent = squareToBomb.transform;
        //targets[0].transform.localPosition = Vector3.zero;

        //for (int i = 0; i < bestBombSquares.Count; i++)
        //{
        //    targets[i].transform.parent = bestBombSquares[i].transform;
        //    targets[i].transform.localPosition = Vector3.zero;
        //}
    }

}
