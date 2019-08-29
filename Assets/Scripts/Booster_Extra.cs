using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Obsolete]
public class Booster_Extra : MonoBehaviour
{
    ParticleSystem[] boosterFlame;
    public static float flameSize;
    public static float flameSpeed;
    static float maxFlameSize = 0.6f;
    static float maxFLameLength = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        boosterFlame = gameObject.GetComponentsInChildren<ParticleSystem>();
        GetInitialValues();
    }

    private void GetInitialValues()
    {
        flameSize = boosterFlame[0].startSize;
        flameSpeed = boosterFlame[0].startSpeed;
    }

    //This function is for increasing flame on continous input for better visual
    public static void FlameSizeShapeBehaviour(ParticleSystem p)
    {
        if (p.gameObject.tag == "BoosterEffect")
        {
            if (p.startSize == flameSize && p.time >= 2f)
            {
                p.startSize = maxFlameSize;
                p.startSpeed = maxFLameLength;
            }
            if (p.startSize < flameSize)
            {
                p.startSize = flameSize;
                p.startSpeed = flameSpeed;
                p.time = 0f;
            }
        }
    }
}
