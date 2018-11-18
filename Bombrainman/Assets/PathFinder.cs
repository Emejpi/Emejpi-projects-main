using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFinder : MonoBehaviour {

    public delegate bool LookAroundFunc(SquareGraphElem square);
    public delegate SquareGraphElem LookAroundFuncReturn(SquareGraphElem square);

    List<SquareGraphElem> currentStep;

    [HideInInspector]
    public Square squareGoal;

    protected float value;

    public class SquareGraphElem
    {
        public Square mySquare;
        public SquareGraphElem stepBack;
        public List<SquareGraphElem> stepForward;

        public SquareGraphElem(Square square)
        {
            mySquare = square;
            stepForward = new List<SquareGraphElem>();
        }
    }

    public void ClearBoard()
    {
        foreach(Transform child in Board.main.transform)
        {
            ExtraSquareInfo squareInfo = child.GetComponent<ExtraSquareInfo>();
            if (!squareInfo)
                return;

            squareInfo.visited = false;
            squareInfo.danger = false;
        }
    }

    public void MarkDanger()
    {
        foreach(Bomb bomb in GetComponentsInChildren<Bomb>())
        {
            Board.main.ShadowExplode(bomb.currentSquare, bomb.range);
        }
    }

    public SquareGraphElem LookAround(Square square, LookAroundFunc BreakPoint, LookAroundFuncReturn Return, bool ignorePlayers = false, bool dangerIsNotWalkable = false)
    {
        ExtraSquareInfo squareInfo = square.GetComponent<ExtraSquareInfo>();

        currentStep = new List<SquareGraphElem>();
        currentStep.Add(new SquareGraphElem(square));

        MarkDanger();

        for (int x = 0; x < 10; x++)
        {
            int loopCount = currentStep.Count;
            for (int i = loopCount - 1; i >=0; i--)
            {
                SquareGraphElem currentSquare = currentStep[i];
                squareInfo = currentSquare.mySquare.GetComponent<ExtraSquareInfo>();
                if (squareInfo.visited)
                {
                    currentStep.RemoveAt(i);
                    continue;
                }

                squareInfo.visited = true;

                if (!BreakPoint(currentSquare))
                {
                    //print("break");
                    ClearBoard();
                    return Return(currentSquare);
                }
            }

            loopCount = currentStep.Count;
            for(int i = 0; i < loopCount; i++)
            {
                SquareGraphElem currentSquare = currentStep[i];

                foreach (Square neighbour in currentSquare.mySquare.Neighbours)
                {
                    if (!neighbour.Walkable(ignorePlayers, !dangerIsNotWalkable) || neighbour.info.visited)
                        continue;
                    SquareGraphElem newSquareGraphElem = new SquareGraphElem(neighbour);
                    currentStep.Add(newSquareGraphElem);
                    Connect(newSquareGraphElem, currentSquare);
                }
            }
        }

        ClearBoard();

        //print("no break");
        return new SquareGraphElem(square);
    }

    public SquareGraphElem Connect(SquareGraphElem current, SquareGraphElem origin)
    {
        current.stepBack = origin;
        origin.stepForward.Add(current);

        return current;
    }

    public SquareGraphElem FindFirstSave(Square square)
    {
        SquareGraphElem save = LookAround(square, IsThisSquareNotSave, ReturnSquare);
        //save.mySquare.GetComponent<Image>().color = (Color.blue + Color.red) / 2; //przenosc target niech pokazuje
        return save;
    }

    public Square FindNextStep(Square from, Square to)
    {

        squareGoal = to;
        return (LookAround(from, IsThisSquareGoal, ReturnNextStepSquare)).mySquare;
    }

    public bool IsThisSquareNotSave(SquareGraphElem square)
    {
        return square.mySquare.info.danger;
    }

    public bool IsThisSquareGoal(SquareGraphElem square)
    {
        return square.mySquare != squareGoal;
    }

    public bool AddPointsToValue(SquareGraphElem square)
    {
        if (square.mySquare.info.danger)
            return true;

        value += 1;
        foreach(Square neighbour in square.mySquare.Neighbours)
        {
            if (neighbour.Walkable(false, true) && (square.stepBack == null || !Board.main.InLine(square.stepBack.mySquare, square.mySquare, neighbour))) //to chyba nei dziala
                value += 1;
        }
        return true;
    }

    public SquareGraphElem ReturnSquare(SquareGraphElem graphElem)
    {
        return graphElem;
    }

    public SquareGraphElem ReturnNextStepSquare(SquareGraphElem graphElem)
    {
        SquareGraphElem last = graphElem;
        SquareGraphElem current = graphElem;
        while (current.stepBack != null)
        {
            last = current;
            current = current.stepBack;
        }

        return last;
    }

    public Square NextStepToSaveSquare(Square square)
    {
        return ReturnNextStepSquare(FindFirstSave(square)).mySquare;
    }

    public Square NextStepToSaveSquareWithMoreOptions(Square square, int numOfSquaresToChoseFrom)
    {
        List<Square> squaresToChoseFrom = new List<Square>();

        for(int i = 0; i < numOfSquaresToChoseFrom; i++)
        {
            foreach(Square elem in squaresToChoseFrom)
            {
                elem.info.visited = true;
            }

            squaresToChoseFrom.Add(FindFirstSave(square).mySquare);
        }

        return FindNextStep(square, GetBestSquareOf(squaresToChoseFrom));
    }

    public Square NextStepWithMoreOptions(Square square)
    {
        Square bestStep = square;
        float bestValue = 0;

        foreach(Square neighbour in square.Neighbours)
        {
            square.info.visited = true;
            value = 0;
            if (!neighbour.Walkable(true) || neighbour.info.danger)
                continue;

            LookAround(neighbour, AddPointsToValue, ReturnSquare, true);
            if (value > bestValue)
            {
                bestValue = value;
                bestStep = neighbour;
            }
        }

        return bestStep;
    }

    public float ValueateSquare(Square square)
    {
        value = 0;
        LookAround(square, AddPointsToValue, ReturnSquare, true);
        return value;
    }

    public Square GetBestSquareOf(List<Square> squares)
    {
        Square bestStep = squares[0];
        float bestValue = 0;

        foreach (Square square in squares)
        {
            //square.GetComponent<Image>().color = Color.blue;
            //square.info.visited = true;
            value = 0;
            if (!square.Walkable(true) || square.info.danger)
                continue;

            LookAround(square, AddPointsToValue, ReturnSquare, true, true);
            if (value > bestValue)
            {
                bestValue = value;
                bestStep = square;
            }
        }

        return bestStep;
    }
}
