using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNMovement : Movement {
    //system ktory szuka bezpiecznych pol
    //system ktory szuka odpowiednich pol do polozenia bomby
    //system wyliczajacy czy na obecnym polu oplaca sie postawic bombe

    //basic AI: idzie do najblizszego bezpiecznego pola i stawia bombe, gdy postawienie jej jest bezpieczne
	
	// Update is called once per frame
	void Update () {
        Square nextStep = new Square();

        Move(nextStep);
	}

    void CalculateFitness()
    {

    }
}
