using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNHandle : MonoBehaviour {

    [HideInInspector]
    public NeuralNetwork net;
    public int[] layers;
    protected float startTime;

    [HideInInspector]
    public bool finished;

    public Transform target;

    public virtual void OnStart()
    {

    }

    public void Inicialize(NeuralNetwork net, string index, Transform target)
    {
        this.net = new NeuralNetwork(net);
        this.net.SetFitness(0);
        this.net.ID = index;
        this.target = target;
        startTime = Time.time;
    }

    public void Inicialize(string index, Transform target)
    {
        net = new NeuralNetwork(layers);
        net.SetFitness(0);
        net.ID = index;
        this.target = target;
        startTime = Time.time;
    }

    protected float GetTime()
    {
        return Time.time - startTime;
    }
	
    public virtual void SetFitness()
    {
        finished = true;
    }

    public void Mutate()
    {
        net.Mutate(true, 0.5f);
    }
}
