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

    public float a;
    public float b;

    public Vector3 startposition;
    public Vector3 finalposition;
    public Vector3 currentposition;


    void Start()
    {
        obstate = ObstacleState.going;
        startposition = transform.position;

        if (transform.rotation.x == 0)
        {
            finalposition = startposition + movementVector;
        }
        else
        {
            finalposition = startposition - movementVector;
        }
    }

    [System.Obsolete]
    void Update()
    {
        a = Mathf.Abs(transform.position.y - finalposition.y);
        b = Mathf.Abs(transform.position.y - startposition.y);
        currentposition = transform.position;
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
        if (Mathf.Abs(transform.position.y - finalposition.y) >= 0.1f)
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }
        else
            obstate = ObstacleState.leaving;
    }
    private void Leaving()
    {
        if (Mathf.Abs(transform.position.y - startposition.y) >= 0.1f)
        {
            transform.position -= transform.up * Time.deltaTime * speed;
        }
        else
            obstate = ObstacleState.going;
    }
}
