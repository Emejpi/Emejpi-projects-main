using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour //CLEAN IT BOY, it will be benefitial
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

    List<Square> populable;

    [System.Serializable]
    public struct Player
    {
        public GameObject pref;
        public int quantity;
    }

    private void Awake()
    {
        main = this;
    }

    public void Populate(List<GameObject> creatures)
    {
        List<Square> currentPopulable = new List<Square>();
        currentPopulable.AddRange(populable);

        foreach(GameObject creature in creatures)
        {
            int sID = Random.Range(0, currentPopulable.Count);

            //print("creature");

            currentPopulable[sID].ChangeType(Square.Type.basic);
            currentPopulable[sID].Place(creature.GetComponent<Movement>());
            creature.transform.parent = currentPopulable[sID].transform;
            creature.transform.localPosition = Vector3.zero;
            creature.GetComponent<Creature>().Inicialize();
            foreach(Square neighbour in creature.GetComponent<Movement>().currentSquare.Neighbours)
            {
                if (neighbour.type == Square.Type.destructable)
                    neighbour.ChangeType(Square.Type.basic);
            }

            currentPopulable.RemoveAt(sID);

            if (currentPopulable.Count == 0)
                return;
        }
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

        populable = new List<Square>();

        int playerIndexer = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                Image square = Instantiate(squarePref, transform).GetComponent<Image>();

                if (x != 0
                    && y != 0
                    && x != rows - 1
                    && y != rows - 1
                    && (x % 2 == 1 && y % 2 == 1))
                {
                    populable.Add(square.GetComponent<Square>());
                    //print("pop");
                }

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

                //if ((x == 1 && y == 1)
                //    || (x == 1 && y == rows - 2)
                //    || (x == rows - 2 && y == 1)
                //    || (x == rows - 2 && y == rows - 2))
                //{
                //    Player playerPref = playersPrefs[playerIndexer++];
                //    Movement player = Instantiate(playerPref.pref, square.transform).GetComponent<Movement>();
                //    player.GetComponent<Image>().color = playerPref.color;
                //    square.GetComponent<Square>().Place(player);
                //    //player.transform.SetParent(transform.parent, false);
                //}
                if(square.GetComponent<Square>().type != Square.Type.wall)
                {
                    if(x>2 && x < rows-3 && y > 2 && y < rows - 3 && Random.Range(0, 100) < 0)
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

        List<GameObject> creatures = new List<GameObject>();
        foreach(Player player in playersPrefs)
        {
            for(int i = 0; i < player.quantity; i++)
                creatures.Add(Instantiate(player.pref, transform));
        }
        Populate(creatures);
    }

    public void DestroyAllPlayers()
    {
        foreach (Creature player in FindObjectsOfType<Creature>())
            if(player.GetComponent<Movement>().enabled)
            DestroyPlayer(player.gameObject, true);
    }

    public void DestroyPlayer(GameObject player, bool gloryDeath = false)
    {
        //Destroy(player);
        if(!gloryDeath)
        {
            player.GetComponent<Creature>().FeedPoints(-1);
        }
        Movement pMovement = player.GetComponent<Movement>();
        pMovement.currentSquare.player = null;
        pMovement.currentSquare = null;
        pMovement.enabled = false;
        player.gameObject.SetActive(false);
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

    //public void Explode(Square square, int reach)
    //{
    //    int killCount;
    //    Explode(square, reach, out killCount);
    //}

    public void Explode(Square square, int reach, out int killCount)
    {
        killCount = 0;

        //print("explode");

        square.bomb = null;

        ExplodeSquare(square, ref killCount);

        Square currentSquare = square;
        for(int i = 0; i < reach; i ++)
        {
            currentSquare = currentSquare.Left;
            if (!ExplodeSquare(currentSquare, ref killCount))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Right;
            if (!ExplodeSquare(currentSquare, ref killCount))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Up;
            if (!ExplodeSquare(currentSquare, ref killCount))
                break;
        }
        currentSquare = square;
        for (int i = 0; i < reach; i++)
        {
            currentSquare = currentSquare.Down;
            if (!ExplodeSquare(currentSquare, ref killCount))
                break;
        }
    }

    bool ExplodeSquare(Square square)
    {
        int killCount = 0;
        return ExplodeSquare(square, ref killCount);
    }

    bool ExplodeSquare(Square square, ref int killCount)
    {
        if (square.type == Square.Type.wall)
            return false;

        Square.Type beforeType = square.type;

        bool kill;
        square.ChangeType(Square.Type.explosion, out kill);
        if (kill)
            killCount++;

        //print(killCount);

        square.Invoke("AfterBomb", 0.5f);

        if (beforeType == Square.Type.destructable)
            return false;

        return true;
    }

    void ClearBoard()
    {
        foreach(Square square in GetComponentsInChildren<Square>())
        {
            foreach (Transform child in square.transform)
                Destroy(child.gameObject);

            if (square.type == Square.Type.explosion)
                square.ChangeType(Square.Type.basic);
        }
    }


}
