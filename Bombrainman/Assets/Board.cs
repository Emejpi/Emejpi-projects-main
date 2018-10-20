using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public GameObject squarePref;

    public int rows;
    public Color wallColor;
    public Color destructableColor;
    public Color powerUpColor;
    public Color baseColor;
    public Color explosionColor;

    public static Board main;

    public Player[] playersPrefs;

    [System.Serializable]
    public struct Player
    {
        public GameObject pref;
        public Color color;
    }

    private void Awake()
    {
        main = this;
    }

    public bool InLine(Square s1, Square s2, Square s3)
    {
        if (Mathf.Abs(s1.transform.GetSiblingIndex() - s2.transform.GetSiblingIndex()) == 1
            && Mathf.Abs(s2.transform.GetSiblingIndex() - s3.transform.GetSiblingIndex()) == 1)
            return true;

        if (Mathf.Abs(s1.transform.GetSiblingIndex() - s2.transform.GetSiblingIndex()) == rows
            && Mathf.Abs(s2.transform.GetSiblingIndex() - s3.transform.GetSiblingIndex()) == rows)
            return true;

        return true;
    }

    // Use this for initialization
    void Start()
    {
        GetComponent<GridLayoutGroup>().constraintCount = rows;

        int playerIndexer = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                Image square = Instantiate(squarePref, transform).GetComponent<Image>();

                if (x == 0
                    || y == 0
                    || x == rows - 1
                    || y == rows - 1
                    || (x % 2 == 0 && y % 2 == 0))
                {
                    square.color = wallColor;
                    square.GetComponent<Square>().ChangeType (Square.Type.wall);
                }
                else
                {
                    square.color = baseColor;
                    square.GetComponent<Square>().ChangeType(Square.Type.basic);
                }

                if ((x == 1 && y == 1)
                    || (x == 1 && y == rows - 2)
                    || (x == rows - 2 && y == 1)
                    || (x == rows - 2 && y == rows - 2))
                {
                    Player playerPref = playersPrefs[playerIndexer++];
                    Movement player = Instantiate(playerPref.pref, square.transform).GetComponent<Movement>();
                    player.GetComponent<Image>().color = playerPref.color;
                    square.GetComponent<Square>().Place(player);
                    //player.transform.SetParent(transform.parent, false);
                }
                else if(square.GetComponent<Square>().type != Square.Type.wall
                    &&((x > 2 && x < rows - 3 )|| (y > 2 && y < rows - 3)))
                {
                    if(x>2 && x < rows-3 && y > 2 && y < rows - 3 && Random.Range(0, 100) < 20)
                    {
                        square.color = wallColor;
                        square.GetComponent<Square>().ChangeType(Square.Type.wall);
                    }
                    else if(Random.Range(0,100) < 80)
                        square.GetComponent<Square>().ChangeType(Square.Type.destructable);
                }
            }
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                int index = x + y * rows;

                Square square = transform.GetChild(index).GetComponent<Square>();
                if(x > 0)
                {
                    square.Left = transform.GetChild(index - 1).GetComponent<Square>();
                }
                if(x < rows - 1)
                {
                    square.Right = transform.GetChild(index + 1).GetComponent<Square>();
                }
                if(y > 0)
                {
                    square.Up = transform.GetChild(index - rows).GetComponent<Square>();
                }
                if (y < rows - 1)
                {
                    square.Down = transform.GetChild(index + rows).GetComponent<Square>();
                }
                square.SetUp();
            }
        }
    }

    public void DestroyPlayer(GameObject player)
    {
        Destroy(player);
    }

    public void ShadowExplode(Square square, int reach, bool visualise = true)
    {
        ShadowExplodeSquare(square, visualise);

        Square currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Left;
            if (!ShadowExplodeSquare(currentSquare, visualise))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Right;
            if (!ShadowExplodeSquare(currentSquare, visualise))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Up;
            if (!ShadowExplodeSquare(currentSquare, visualise))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Down;
            if (!ShadowExplodeSquare(currentSquare, visualise))
                break;
        }
    }

    bool ShadowExplodeSquare(Square square, bool visualise = true)
    {
        if (square.type == Square.Type.wall)
            return false;

        square.info.SetDanger(true, visualise);

        if (square.type == Square.Type.destructable)
            return false;

        return true;
    }

    public void Explode(Square square, int reach)
    {
        if (square.bomb)
        {
            Destroy(square.bomb);
            square.bomb = null;
        }

        ExplodeSquare(square);

        Square currentSquare = square;
        for(int i = 0; i < reach; i ++)
        {
            currentSquare = currentSquare.Left;
            if (!ExplodeSquare(currentSquare))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Right;
            if (!ExplodeSquare(currentSquare))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Up;
            if (!ExplodeSquare(currentSquare))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Down;
            if (!ExplodeSquare(currentSquare))
                break;
        }
    }

    bool ExplodeSquare(Square square)
    {
        if (square.type == Square.Type.wall)
            return false;

        Square.Type beforeType = square.type;

        square.ChangeType(Square.Type.explosion);
        square.Invoke("AfterBomb", 0.5f);

        if (beforeType == Square.Type.destructable)
            return false;

        return true;
    }


}
