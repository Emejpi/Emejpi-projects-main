using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NNRepresentative : MonoBehaviour {

    NNHandle[] NNhandles;

    public string ID;

    public bool loadOnStart;

    public Transform target;

    private void Start()
    {
        if (loadOnStart && target)
        {
            Inicialize(target);
            Load();
        }

        foreach (NNHandle handle in NNhandles)
        {
            handle.OnStart();
        }
    }

    public void Load()
    {
        Load(ID);
    }

    public void Load(string ID)
    {
        foreach (NNHandle handle in NNhandles)
        {
            handle.net.Load(ID);
        }
    }

    public float GetFitness()
    {
        float fitnessTotal = 0;

        foreach (NNHandle handle in NNhandles)
            fitnessTotal += handle.net.GetFitness();

        return fitnessTotal;
    }

    public void Mutate()
    {
        foreach (NNHandle handle in NNhandles)
            handle.Mutate();
    }

    public NeuralNetwork [] GetNets()
    {
        NeuralNetwork[] nets = new NeuralNetwork[NNhandles.Length];

        for (int i = 0; i < nets.Length; i++)
            nets[i] = NNhandles[i].net;

        return nets;
    }

    public void Inicialize(Transform target)
    {
        NNhandles = GetComponents<NNHandle>();
        foreach (NNHandle NN in NNhandles)
            NN.Inicialize(ID, target);
    }

    public void Inicialize(NeuralNetwork[] nets, Transform target)
    {
        NNhandles = GetComponents<NNHandle>();
        for(int i = 0; i < NNhandles.Length; i++)
            NNhandles[i].Inicialize(nets[i], ID, target);
    }

    public void Finish()
    {
        foreach (NNHandle nn in NNhandles)
            if (!nn.finished)
                nn.SetFitness();
    }

    public void Hide()
    {
        GetComponent<Image>().enabled = false;
        foreach(Image image in GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
    }

}
