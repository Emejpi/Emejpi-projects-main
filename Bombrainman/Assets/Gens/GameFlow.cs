using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    public static GameFlow main;

    public int gen;

    public Text text;

    private void Awake()
    {
        main = this;

        if(PlayerPrefs.HasKey("gen"))
            gen = PlayerPrefs.GetInt("gen");

        text.text = "gen " + gen;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            DuelComplited();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            Application.LoadLevel(0);
        }
    }

    public void DuelComplited() //zamiast robienia kopii, zrob nowe prefaby i zainicjiuj je uzywajac poprzednich creatur
    {
        gen++;
        PlayerPrefs.SetInt("gen", gen);
        text.text = "gen " + gen;
        //Board.main.DestroyAllPlayers();

        List<GameObject> newCreatures = AllGensManager.main.GetNewCreaturePrefabs();

        //Board.main.Populate(newCreatures);

        Application.LoadLevel(0);
    }

}
