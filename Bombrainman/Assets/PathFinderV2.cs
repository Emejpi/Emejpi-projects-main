using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderV2 : PathFinder {

    public static PathFinderV2 main;

    public delegate void DoingSomethingWithSquare(Square square);

    private void Awake()
    {
        main = this;
    }

    ///

    List<SquareGraphElem> currentStep;

    void StartLookAround(Square square)
    {
        ClearBoard();
        currentStep = new List<SquareGraphElem>();
        currentStep.Add(new SquareGraphElem(square));
        MarkDanger();
    }

    void LookAroundStep(bool ignorePlayers = false, bool ignoreDanger = true, bool ignoreDestructable = false, bool ignoreBombs = false)
    {
        int loopCount = currentStep.Count;
        for (int i = loopCount - 1; i >= 0; i--)
        {
            SquareGraphElem currentSquare = currentStep[i];
            ExtraSquareInfo squareInfo = currentSquare.mySquare.info;
            if (squareInfo.visited)
            {
                currentStep.RemoveAt(i);
                continue;
            }

            squareInfo.visited = true;
        }

        loopCount = currentStep.Count;
        for (int i = 0; i < loopCount; i++)
        {
            SquareGraphElem currentSquare = currentStep[i];

            foreach (Square neighbour in currentSquare.mySquare.Neighbours)
            {
                if (!neighbour.Walkable(ignorePlayers, ignoreDanger, ignoreDestructable, ignoreBombs) || neighbour.info.visited)
                    continue;
                SquareGraphElem newSquareGraphElem = new SquareGraphElem(neighbour);
                currentStep.Add(newSquareGraphElem);
                Connect(newSquareGraphElem, currentSquare);
            }
        }
    }

    void EndLookAround()
    {
        ClearBoard();
    }

    public Square LookAroundBaseForCopy(Square square, int loopCount)
    {
        StartLookAround(square);
        Square squareOut = square;

        for(int i = 0; i < loopCount; i++)
        {
            foreach(SquareGraphElem elem in currentStep)
            {
                //Do stuff
            }

            LookAroundStep();
        }

        EndLookAround();

        return squareOut;
    }
    ///

    struct ValuatedSquare
    {
        public Square square;
        public float value;
    }

    public List<Square> BestSquareToBomb(Square square, int loopCount, int reach, int outBombs) //Valueting is wrong, should be done by NN
    {
        StartLookAround(square);
        List<Square> squaresOut = new List<Square>();
        squaresOut.Add(square);

        float baseValue = 10000;
        float bestValue = baseValue;

        LookAroundStep();

        if (currentStep.Count == 0)
            return squaresOut;

        currentStep.RemoveAt(0);
        List<ValuatedSquare> valuatedSquares = new List<ValuatedSquare>();

        for (int i = 0; i < loopCount; i++)
        {
            foreach (SquareGraphElem elem in currentStep)
            {
                Board.main.ShadowExplode(elem.mySquare, reach, false);
                elem.mySquare.bomb = new GameObject();
                ValueateSquare(square);  //MIAU MIAU MIAU
                elem.mySquare.bomb = null;

                valuatedSquares.Add(new ValuatedSquare { value = value, square = elem.mySquare });

                //if (value < bestValue)
                //{
                //    bestValue = value;
                //    squareOut = elem.mySquare;
                //}
            }

            LookAroundStep();
        }

        Square bestSquere = square;

        if (valuatedSquares.Count == 0)
            return squaresOut;

        squaresOut = new List<Square>();

        for (int i = 0; i < outBombs; i++)
        {
            bestValue = 1000;
            ValuatedSquare bestValuetedSquare = valuatedSquares[0];
            foreach (ValuatedSquare valuetedSquare in valuatedSquares)
            {
                if (valuetedSquare.value < bestValue && !squaresOut.Contains(valuetedSquare.square))
                {
                    bestValuetedSquare = valuetedSquare;
                    bestValue = value;
                    bestSquere = valuetedSquare.square;
                }
            }
            valuatedSquares.Remove(bestValuetedSquare);
            squaresOut.Add(bestSquere);
        }

        EndLookAround();

        return squaresOut;
    }

    public List<Square> SquaresToTrap(Square square, int bombRange)
    {
        List<Square> squaresToTrap = new List<Square>();

        int directionIter = 0;
        for(int i = 0; i < square.Neighbours.Count; i++)
        {
            Square neighbour = square.Neighbours[i];

            if(neighbour.Walkable())
            {
                squaresToTrap.Add(FarthestBlocking(neighbour, i, bombRange));
            }
        }

        return squaresToTrap;
    }

    public Square FarthestBlocking(Square square, int direction, int maxDistance)
    {
        Square currentSquare = square;

        int counter = 0;

        while (currentSquare.Neighbours[direction].Walkable(true) && SidesBlocked(currentSquare, direction) && counter <= maxDistance)
        {
            counter++;
            currentSquare = currentSquare.Neighbours[direction];
        } 

        return currentSquare;
    }

    public bool SidesBlocked(Square square, int direction)
    {
        for(int i = 0; i < square.Neighbours.Count; i++)
        {
            if(i != direction && square.Neighbours[i] != square.GetOpposite(square.Neighbours[direction]))
            {
                if (square.Neighbours[i].Walkable(true))
                    return false;
            }
        }

        return true;
    }

    public Square BestSquareToBombInReach(Square target, Square mySquare, int loopCount, int outBombs)
    {
        List<Square> bombsOut = SquaresToTrap(target, 3);

        StartLookAround(mySquare);
        Square squareOut = mySquare;

        for (int i = 0; i < loopCount; i++)
        {
            foreach (SquareGraphElem elem in currentStep)
            {
                if (bombsOut.Contains(elem.mySquare))
                {
                    EndLookAround();
                    return elem.mySquare;
                }
                //Do stuff
            }

            LookAroundStep();
        }

        EndLookAround();

        return squareOut;
    }

    public Square NextSquareTo(Square mySquare, Square targetSquare, int loopCount)
    {
        StartLookAround(mySquare);
        Square squareOut = mySquare;

        for (int i = 0; i < loopCount; i++)
        {
            LookAroundStep(true, true, true, true);

            foreach (SquareGraphElem elem in currentStep)
            {
                if(elem.mySquare == targetSquare)
                {
                    squareOut = ReturnNextStepSquare(elem).mySquare;
                    i = loopCount;
                    break;
                }
            }
        }

        EndLookAround();

        return squareOut;
    }
}
