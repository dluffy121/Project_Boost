using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MovingObstacle : MonoBehaviour
{
    //to transform globally
    //Vector3 offset = speed * movementVector;
    //transform.position = startingPos + offset;

    //To transform Locally
    //transform.position += transform.right * movementVector.x;
    //transform.position += transform.up * movementVector.y;
    //transform.position += transform.forward * movementVector.z;

    [SerializeField] Vector3 movementVector;
    enum ObstacleState { going, leaving}
    [Range(0,20)] [SerializeField] float speed;
    ObstacleState obstate;

    Vector3 startposition;
    Vector3 finalposition;

    void Start()
    {
        obstate = ObstacleState.going;
        startposition = transform.position;
        finalposition = startposition + movementVector;
    }

    [System.Obsolete]
    void Update()
    {
        if (Rocket.state == Rocket.State.Moving)
        {
            switch (obstate.ToString())
            {
                case "going":
                    Going();
                    break;
                case "leaving":                   
                    Leaving();
                    break;
            }
        }
    }

    private void Going()
    {
        if (transform.position.y <= finalposition.y)
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }
        else
            obstate = ObstacleState.leaving;
    }
    private void Leaving()
    {
        if (transform.position.y >= startposition.y)
        {
            transform.position -= transform.up * Time.deltaTime * speed;
        }
        else
            obstate = ObstacleState.going;
    }
}
