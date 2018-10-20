using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    public Square Up;
    public Square Down;
    public Square Right;
    public Square Left;

    public List<Square> Neighbours;

    public Movement player;
    public GameObject bomb;

    public ExtraSquareInfo info;

    public Square GetOpposite(Square square)
    {
        if (square == Up)
            return Down;
        if (square == Down)
            return Up;
        if (square == Right)
            return Left;
        if (square == Left)
            return Right;

        return this;
    }

    public bool Walkable(bool igonrePlayers = false, bool ignoreDanger = true, bool ignoreDestructable = false, bool ignoreBombs = false)
    {
        return type != Type.explosion 
            && type != Type.wall 
            && (type != Type.destructable || ignoreDestructable)
            && (bomb == null || ignoreBombs) 
            && (player == null || igonrePlayers)
            && (ignoreDanger || !info.danger);  
    }

    public void SetUp()
    {
        Neighbours = new List<Square>();
        if (Up)
            Neighbours.Add(Up);
        if (Down)
            Neighbours.Add(Down);
        if (Right)
            Neighbours.Add(Right);
        if (Left)
            Neighbours.Add(Left);

        info = GetComponent<ExtraSquareInfo>();
    }

    public void AfterBomb()
    {
        info.SetDanger(false);
        ChangeType(Square.Type.basic);
    }

    public void Place(Movement player)
    {
        this.player = player;
        if(player.currentSquare)
            player.currentSquare.player = null;
        player.currentSquare = this;
    }

    public enum Type
    {
        basic,
        wall,
        destructable,
        explosion
    }

    public Type type;

    public void ChangeType(Type type)
    {
        this.type = type;

        switch(type)
        {
            case Type.basic:
                GetComponent<Image>().color = Board.main.baseColor;
                break;

            case Type.destructable:
                GetComponent<Image>().color = Board.main.destructableColor;
                break;

            case Type.wall:
                GetComponent<Image>().color = Board.main.wallColor;

                if (player != null)
                    Board.main.DestroyPlayer(player.gameObject);
                break;

            case Type.explosion:
                GetComponent<Image>().color = Board.main.explosionColor;

                if (player != null)
                    Board.main.DestroyPlayer(player.gameObject);

                if(bomb)
                {
                    Board.main.Explode(this, bomb.GetComponent<Bomb>().range);
                }
                break;
        }
    }
}
