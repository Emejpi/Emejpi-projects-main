using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllGensManager : MonoBehaviour {

    public static AllGensManager main;

    List<GenManager> genManagers;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        genManagers = new List<GenManager>();
        genManagers.AddRange(FindObjectsOfType<GenManager>());
    }

    public List<GameObject> GetNewCreaturePrefabs()
    {
        List<GameObject> newCreatures = new List<GameObject>();

        foreach (GenManager genManager in genManagers)
        {
            genManager.gen = new List<Creature>();
        }

        foreach (Creature creature in FindObjectsOfType<Creature>())
        {
            if (genManagers.Count <= creature.familyID)
                genManagers.Add(Instantiate(genManagers[0].gameObject, transform).GetComponent<GenManager>());
            genManagers[creature.familyID].gen.Add(creature);
        }

        foreach (GenManager genManager in genManagers)
        {
            newCreatures.AddRange(genManager.NextGen());
        }

        return newCreatures;
    }
}
