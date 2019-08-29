using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    Quaternion initRot;

    void Start()
    {
        initRot = transform.rotation; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = initRot;
    }
}
