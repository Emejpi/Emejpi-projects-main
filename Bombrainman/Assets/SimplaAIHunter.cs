using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplaAIHunter : Movement { //BEZ ROZUMIENIA CZASU TO NIE POCIAGNIE DALEJ< LEPIEJ POMYSL JAK ZROBIC O ZEBY SI MOGLO STEROWAC DECYZJAMI OPDNOSNIE CZASU, ALBO TO OBLICZ, PO PROSTU FUNKCJA CAN I REACH IT IN TIME? ;)
    //TO POWINNO BYC PROSTE, PIERW ZNAJDUJE SQUERSY TO BOMB, PATRZY PO NICH CZY JEST W STANIE DOJSC W TYM CZASIE, NASTEPNIE JE SHADOW BOMBUJE I SPRAWDZA CZY PO DOJSCIU DO NICH BEDZIE W STANIE UCIEC, JESLI TAK, LECI, TO JEST SOLUTION

    public Movement player;

    public GameObject targetPrefabl;
    public List<GameObject> targets;

    Booming bombing;

    GameObject currentBomb;
    //Pomysl o zrobieniu funkcji ktora zwraca miejsca na bomby potrzebne do stapowania gracza
    private void Update() //Zrob zeby jak raz podejmie decyzje gdzie isc to nie sprawdfzal ponownie drogi, jedynie czy jestepny krok jest walkable, k? Moze z bombowanie tz tak, zeby mimo wszystko stawial
    {//znajdowanie sciezki przez destructable, ogarnij to lepiej, zeby na drodze bylo jak najmniej destructable
        if (targets.Count == 0)
        {
            targets = new List<GameObject>();
            for (int i = 0; i < 5; i++)
                targets.Add(Instantiate(targetPrefabl, transform));

            bombing = GetComponent<Booming>();
        }

        if(!player || !player.enabled)
        {
            Movement[] players = GameObject.FindObjectsOfType<Movement>();

            if (players.Length == 1)
            {
                enabled = false;
                return;
            }

            do
            {
                player = players[Random.Range(0, players.Length)];
            } while (player == this);
        }

        //
        List<Square> squaresTOBomb = PathFinderV2.main.SquaresToTrap(player.currentSquare, 3);

        for(int i = 0; i < targets.Count; i++)
        {
            targets[i].SetActive(false);
        }

        for(int i = 0; i < squaresTOBomb.Count; i++)
        {
            //targets[i].SetActive(true);
            targets[i].transform.parent = squaresTOBomb[i].transform;
            targets[i].transform.localPosition = Vector3.zero;
            targets[i].transform.SetAsLastSibling();
        }
        //

        Square targetSquare = currentSquare;

        PathFinderV2.main.MarkDanger();

        if (currentSquare.bomb == null)
            foreach (Square neighbour in currentSquare.Neighbours)
            {
                if (neighbour.player)
                {
                    currentBomb = bombing.Bomb(currentSquare);
                    transform.SetAsLastSibling();
                }
            }

        if (currentSquare.info.danger || currentSquare.bomb != null)
        {
            List<Square> squaresToBomb = PathFinderV2.main.SquaresToTrap(player.currentSquare, 3);

            if(squaresToBomb.Contains(currentSquare) && (currentSquare.bomb == null))
            {
                currentBomb = bombing.Bomb(currentSquare);
                transform.SetAsLastSibling();
            }

            bool squareToBombInReach = false;

            PathFinderV2.main.MarkDanger();

            foreach (Square square in squaresToBomb)
            {
                targetSquare = PathFinderV2.main.FindNextStep(currentSquare, square);
                if (targetSquare != currentSquare && !targetSquare.info.danger)
                {
                    squareToBombInReach = false;
                    break;
                }
            }
            if(!squareToBombInReach)
                targetSquare = PathFinderV2.main.NextStepToSaveSquareWithMoreOptions(currentSquare, 1);
            //targets[0].transform.parent = targetSquare.transform;
            //targets[0].transform.localPosition = Vector3.zero;
        }
        else
        {
            //if(Random.Range(0,1000) == 1)
            //{
            //    currentBomb = bombing.Bomb(currentSquare);
            //    transform.SetAsLastSibling();
            //    return;
            //}

            if (!player)
                return;

            Square squareToBomb = PathFinderV2.main.BestSquareToBombInReach(currentSquare, player.currentSquare, 5, 3);
            targetSquare = PathFinderV2.main.FindNextStep(currentSquare, squareToBomb);
            
            if (targetSquare == currentSquare)
            {
                targetSquare = PathFinderV2.main.NextSquareTo(currentSquare, player.currentSquare, 25);
            }
                PathFinderV2.main.MarkDanger();

                if (targetSquare.info.danger /*|| (currentBomb != null)*/)
                    return;

                if ((!targetSquare.Walkable(false, false) && (targetSquare.type == Square.Type.destructable || targetSquare.player)) && (currentSquare.bomb == null))
                {
                    currentBomb = bombing.Bomb(currentSquare);
                    transform.SetAsLastSibling();
                    return;
                }
            
            //targetSquare = PathFinderV2.main.NextStepWithMoreOptions(currentSquare);


        }
        //targetSquare.GetComponent<Image>().color = Color.blue;
        Move(targetSquare);

        //PathFinderV2.main.ClearBoard();

        //List<Square> bestBombSquares = PathFinderV2.main.BestSquareToBomb(currentSquare, 5, 3, 3); //ZROB HUNTER IA



        //Square squareToBomb = PathFinderV2.main.BestSquareToBombInReach(currentSquare, player.currentSquare, 5, 3);

        //targets[0].transform.parent = squareToBomb.transform;
        //targets[0].transform.localPosition = Vector3.zero;

        //for (int i = 0; i < bestBombSquares.Count; i++)
        //{
        //    targets[i].transform.parent = bestBombSquares[i].transform;
        //    targets[i].transform.localPosition = Vector3.zero;
        //}
    }
}
