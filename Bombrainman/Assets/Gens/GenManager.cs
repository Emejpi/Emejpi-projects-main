using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenManager : MonoBehaviour {

    public List<Creature> gen;

    public int generation = 0;
    [Range(0,1)]
    public float survivePerc = 0.1f;

    public List<GameObject> NextGen()
    {
        return NextGen(gen);
    }

    public List<GameObject> NextGen(List<Creature> lastGen)
    {
        int surviveNumber = (int)(lastGen.Count * survivePerc);
        if (surviveNumber == 0)
            surviveNumber = 1;
        
        List<GameObject> newGen = new List<GameObject>();

        List<Creature> lastGenSorted = Sort(lastGen);

        for(int i = 0; i < surviveNumber; i++)
        {
            newGen.Add(Copy(lastGenSorted[0]));
            lastGenSorted.RemoveAt(0);
        }

        int survivorsIterator = 0;
        while(newGen.Count < lastGen.Count)
        {
            GameObject newCreature = Copy(lastGenSorted[survivorsIterator]);
            newCreature.GetComponent<Creature>().Mutate(lastGenSorted[survivorsIterator]);
            newGen.Add(newCreature);

            survivorsIterator++;
            if (survivorsIterator >= surviveNumber)
                survivorsIterator = 0;
        }

        newGen[0].GetComponent<Creature>().BestOne();

        generation++;

        DestroyGen(lastGen);

        return newGen;
    }

    void DestroyGen(List<Creature> gen)
    {
        foreach(Creature creature in gen)
        {
            Destroy(creature.gameObject);
        }
    }

    public GameObject Copy(Creature creature)
    {
        GameObject newCreature = Instantiate(creature.gameObject, transform);
        newCreature.GetComponent<Creature>().Inicialize(creature);
        //newCreature.GetComponent<Creature>().Mutate(creature);
        return newCreature;
    }

    List<Creature> Sort(List<Creature> creatures)
    {
        List<Creature> sorted = new List<Creature>();

        foreach(Creature creature in creatures)
        {
            InsertSort(creature, sorted);
        }

        return sorted;
    }

    void InsertSort(Creature creature, List<Creature> creatures)
    {
        for(int i = 0; i < creatures.Count; i++)
        {
            if (creature.fitness >= creatures[i].fitness)
            {
                creatures.Insert(i, creature);
                return;
            }
        }

        creatures.Add(creature);
    }

}
