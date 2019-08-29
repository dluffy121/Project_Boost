using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilatingObstacle : MonoBehaviour
{
    //This script ulike the MovingObstacle uses SIN wave function for oscilation

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;//Total oscillation time

    const float tau = Mathf.PI * 2;
    float movementFactor;
    float cycles;
    float rawSinWave;

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;    
    }

    void Update()
    {
        //will give weird bug when period set to 0 since floats cannot be compared due to massive decimal places
        if (period <= Mathf.Epsilon) { return; }    //disables oscilation since Epsilion is smallest float no. | can also use Floor to floor float to 0 but use ==
        cycles = Time.time / period;
        rawSinWave = Mathf.Sin(cycles * tau);       //It generates sin wave  bracket value

        movementFactor = rawSinWave / 2f + 0.5f;    //this will determine how much distance is left to complete 1 oscilation for sin wave
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
