using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class NNPopulationControl : MonoBehaviour {

    public GameObject netPrefab;
    public GameObject targetPrefab;

    public string targetsID;

    NNRepresentative[] nets;
    GameObject[] targets;

    public int populationSize;

    [Range(0,1)]
    public float pupulationKeepPrec;

    public float generationTime;

    public struct Gens
    {
        public NeuralNetwork[] nets; 
    }

    List<Gens> gens;

    int generation;

    public Transform target;

    public Transform targetsHolder;

    public Text text;

    void CreatePopulation()
    {
        generation = 1;

        nets = new NNRepresentative[populationSize];
        targets = new GameObject[populationSize];

        for(int i = 0; i < populationSize; i++)
        {
            nets[i] = Instantiate(netPrefab, transform).GetComponent<NNRepresentative>();
            if(!targetPrefab)
                nets[i].Inicialize(target);
            else
            {
                targets[i] = Instantiate(targetPrefab, targetsHolder);
                nets[i].Inicialize(targets[i].transform);
                ConnectTarget(nets[i], targets[i], i);
            }

            Load(i);

            if (i > 0)
                nets[i].Mutate();
        }

    }

    public virtual void ConnectTarget(NNRepresentative NN, GameObject target, int index)
    {
        NN.gameObject.name = index + "";
        target.name = index + "";
        target.GetComponent<NNRepresentative>().Inicialize(NN.transform);

        target.GetComponent<NNRepresentative>().Load(targetsID);

        if (index > 0)
            target.GetComponent<NNRepresentative>().Hide();

    }

    void CreatePrefab()
    {
        PrefabUtility.CreatePrefab("Assets/" + gameObject.name + ".prefab", nets[0].gameObject);
    }

    void Save()
    {
        nets[0].GetNets()[0].Save();
    }

    void Load(int index)
    {
        nets[index].GetNets()[0].Load(targetsID);
    }

    void NextPopulation()
    {

        foreach(NNRepresentative nn in nets)
        {
            nn.Finish();
        }

        //
        generation++;
        text.text = "gen " + generation;

        GetGens();

        foreach (NNRepresentative nn in nets)
        {
            Destroy(nn.gameObject);
        }

        if (targetPrefab)
            foreach (GameObject target in targets)
            {
                Destroy(target);
            }

        OnGenStart();

        nets = new NNRepresentative[populationSize];
        targets = new GameObject[populationSize];
        int gensIterator = 0;

        for (int i = 0; i < populationSize; i++)
        {
            nets[i] = Instantiate(netPrefab, transform).GetComponent<NNRepresentative>();
            if(!targetPrefab)
                nets[i].Inicialize(gens[gensIterator].nets, target);
            else
            {
                targets[i] = Instantiate(targetPrefab, targetsHolder);
                nets[i].Inicialize(targets[i].transform);
                ConnectTarget(nets[i], targets[i], i);
            }


            if(i > 0)
                nets[i].Hide();

            if (i >= gens.Count)
                nets[i].Mutate();


            gensIterator++;
            if (gensIterator >= gens.Count)
                gensIterator = 0;
        }

        Invoke("NextPopulation", generationTime);
    }

    protected virtual void OnGenStart()
    {
        if (!target)
            return;

        if (generation % 2 == 0)
            target.position = new Vector2(transform.position.x + 500 * (generation % 4 == 0 ? 1 : -1), Random.Range(0, Screen.height));

        target.GetComponent<Image>().color = Color.white;
    }

    void GetGens()
    {
        BubbleSortNets();

        gens = new List<Gens>();

        int populationKeepCount = (int)(populationSize * pupulationKeepPrec);

        for(int i = populationSize - 1; i > populationSize - populationKeepCount - 1; i--)
        {
            gens.Add(new Gens { nets = nets[i].GetNets() });
        }
    }

    public void BubbleSortNets()
    {
        int i, j;
        int N = nets.Length;

        for (j = N - 1; j > 0; j--)
        {
            for (i = 0; i < j; i++)
            {
                if (nets[i].GetFitness() > nets[i + 1].GetFitness())
                    Exchange(nets, i, i + 1);
            }
        }
    }

    void Exchange(NNRepresentative[] nets, int index1, int index2)
    {
        NNRepresentative temp = nets[index1];
        nets[index1] = nets[index2];
        nets[index2] = temp;
    }

    // Use this for initialization
    void Start () {
        Application.runInBackground = true;

        CreatePopulation();
        Invoke("NextPopulation", generationTime);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Save();
    }
}
