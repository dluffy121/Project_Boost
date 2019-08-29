using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTracker : MonoBehaviour
{
    public static class Constants
    {
        public static readonly Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
    }

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;
    public Vector3 offset= new Vector3(0.0f,1f,0.0f);

    const float speed = 0.5f;

    float yangle = 0.0f;
    float xangle = 0.0f;
    float xdistance = 0.0f;
    float zdistance = 15f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        KeyUpCameraMovement();
        KeyPressCameraMovement();
    }

    private void KeyUpCameraMovement()
    {
        if(!Input.GetKey(KeyCode.Space))
        {
            if (zdistance >= 15f)
                zdistance -= 0.05f;
        }
        if (!Input.GetKey(KeyCode.D))
        {
            if (xdistance >= 0f)
                xdistance -= 0.05f * speed;
            if (yangle >= 0f && xangle<=0f)
            {
                yangle -= 0.5f;
                xangle += 0.5f;
            }
        }
        if (!Input.GetKey(KeyCode.A))
        {
            if (xdistance <= 0f)
                xdistance += 0.05f * speed;
            if (yangle <= 0f && xangle <=0f)
            {
                yangle += 0.5f;
                xangle += 0.5f;
            }
        }
    }
    private void KeyPressCameraMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (zdistance <= 17f)
                zdistance += 0.1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (xdistance <= 3f)
                xdistance += 0.1f;
            while (yangle <= 5f && xangle >= 5f)
            {
                yangle += 0.1f;
                xangle -= 0.1f;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (xdistance >= -3f)
                xdistance -= 0.1f;
            while (yangle >= 5f && xangle >=5f)
            {
                yangle -= 0.1f;
                xangle -= 0.1f;
            }
        }
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        Vector3 dir = new Vector3(xdistance, 0, -zdistance);
        Quaternion rotation = Quaternion.Euler(xangle, yangle, -xangle);
        camTransform.position = lookAt.position + Constants.offset + rotation * dir;
    }
}
