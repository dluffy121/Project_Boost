using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Obsolete]
public class BoosterVisible : MonoBehaviour
{
    ParticleSystem[] boosterFlame;
    Light[] lights;
    float flameSize;
    float flameSpeed;
    float maxFlameSize = 0.6f;
    float maxFLameLength = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        boosterFlame = gameObject.GetComponentsInChildren<ParticleSystem>();
        lights = gameObject.GetComponentsInChildren<Light>();
        GetInitialValues();
    }

    private void GetInitialValues()
    {
        flameSize = boosterFlame[0].startSize;
        flameSpeed = boosterFlame[0].startSpeed;
        LightDeactivate();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Rocket.state == Rocket.State.Dying)
        {
            LightDeactivate();
            NoThrustFlameBehaviour();
            return;
        }
        else
        {
            ThrustFlameBehaviour();
            NoThrustFlameBehaviour();
        }
    }

    private void LightDeactivate()
    {
        foreach (Light l in lights)
        {
            if(l.enabled)
                l.enabled = !l.enabled;
        }
    }

    private void LightActivate()
    {
        foreach (Light l in lights)
        {
            if (!l.enabled)
                l.enabled = !l.enabled;
        }
    }

    private void NoThrustFlameBehaviour()
    {
        if (Input.GetKeyUp(KeyCode.Space) || !Input.GetKey(KeyCode.Space) || Rocket.state == Rocket.State.Dying)
        {
            foreach (ParticleSystem p in boosterFlame)
            {
                if (p.startSize >= 0 && p.time >= 0.3f)
                {
                    p.startSize = 0;
                    LightDeactivate();
                }
            }
        }
    }

    private void ThrustFlameBehaviour()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (ParticleSystem p in boosterFlame)
            {
                if (!p.isPlaying)
                {
                    p.Play();
                }
                FlameSizeShapeBehaviour(p);
                LightActivate();
            }
        }
    }

    private void FlameSizeShapeBehaviour(ParticleSystem p)
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
