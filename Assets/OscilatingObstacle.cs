using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilatingObstacle : MonoBehaviour
{
    //This script ulike the MovingObstacle uses SIN wave function for oscilation

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;//Total oscillation time

    // todo remove from inspector later
    [Range(0,1)][SerializeField] float movementFactor;

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;    
    }

    void Update()
    {
        float cycles = Time.time / period; //this will determine how much time is left to complete 1 oscilation for sin wave
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau); // the time left will be multiplied with tau to get the reamining distance for that oscillation

        movementFactor = rawSinWave / 2f + 0.5f;
        print(movementFactor);
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
