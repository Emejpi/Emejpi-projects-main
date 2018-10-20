using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Tag
{

}

public struct TagWithValue
{
    public Tag tag;
    public bool value;
}

public class ExtraSquareInfo : MonoBehaviour {

    public bool visited;
    public bool danger;

    public void SetDanger(bool danger, bool visualise = true)
    {
        if (this.danger == danger)
            return; 

        this.danger = danger;
        //if(danger && visualise)
        //    GetComponent<Image>().color = (Color.red + GetComponent<Image>().color) / 2;
    }
}
