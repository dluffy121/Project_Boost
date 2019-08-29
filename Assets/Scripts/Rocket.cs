﻿using UnityEngine;
using System;
using UnityEngine.SceneManagement;
[System.Obsolete]

public class Rocket : MonoBehaviour
{
    //User Editable Fields
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float lvlLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip startSound;//Unlike AudioSource AudioClip.volume doesnot exists

    ParticleSystem[] allParticles;
    Light[] allLights;

    //Constant Fields
    const float minVol = 0.3f;
    bool thrustFlag = true;
    bool collisionsEnabled = true;
    float flamesize;
    public int currentLvl;
    public enum State { Alive , Moving, Dying, Transcending }
    public static State state;

    Rigidbody rigidBody;
    AudioSource audioSource;

    void Start()
    {
        allParticles = gameObject.GetComponentsInChildren<ParticleSystem>();
        allLights = gameObject.GetComponentsInChildren<Light>();
        currentLvl = SceneManager.GetActiveScene().buildIndex;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        LaunchScript.lvlStart = true;
        LightDeactivate();
        state = State.Alive;
    }

    void Update()
    {
        if (state != State.Dying)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    //Debugging Options
    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLvl();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    //Collision Control
    public void OnCollisionEnter(Collision collision)
    {
        if(state == State.Dying || !collisionsEnabled) { return;}
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Fuel":
                print("Tank Full");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        DeathParticles();
        NoThrustParticles();
        LightDeactivate();
        Invoke("LoadSameLvl", lvlLoadDelay);
    }
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        Invoke("LoadNextLvl", lvlLoadDelay);
    }

    //Scene Control
    private void LoadSameLvl()
    {
        SceneManager.LoadScene(currentLvl);
    }
    private void LoadNextLvl()
    {
        if(currentLvl + 1 == SceneManager.sceneCountInBuildSettings)
            currentLvl = -1;
        SceneManager.LoadScene(currentLvl + 1);
    }

    //Input Control
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            state = State.Moving;
            ThrustParticles();
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);//'up' and 'forward' have unit 1
            VolumeControl();
            LightActivate();
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        { 
            VolumeControl(minVol);                      //|tag1|
            NoThrustParticles();
            LightDeactivate();
        }
    }
    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            FreezeRotation(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            FreezeRotation(-rcsThrust * Time.deltaTime);
        }
    }
    private void FreezeRotation(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;    //To rotate naturally without physics
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false;   //Resume physics Control
    }

    //Volume Control
    //If not using AudioClip use this                   //|tag1|
    private void VolumeControl(float minVol)
    {
        StartCoroutine(AudioController.FadeOut(audioSource, 0.6f, minVol));//Use StartCoroutine to use another class function
    }
    private void VolumeControl()
    {
        if (!audioSource.isPlaying)//so it doesnt Layer //|tip|
        {
            //audioSource.PlayOneShot(mainEngine);        //|tag2|
            audioSource.Play();
        }
        if (audioSource.volume <= minVol)
        {
            audioSource.volume = 1f;
        }
    }

    //Particle Effects and Triggers
    private void DeathParticles()
    {
        foreach (ParticleSystem p in allParticles)
        {
            if (p.gameObject.tag == "ExplosionEffect")
            {
                if (!p.isPlaying)
                {
                    p.Play();
                }
            }
        }
    }
    private void ThrustParticles()
    {
        foreach (ParticleSystem p in allParticles)
        {
            if (p.gameObject.tag == "BoosterEffect")
            {
                flamesize = p.startSize;
                if (!p.isPlaying)
                {
                    p.startSize = Booster_Extra.flameSize;
                    p.startSpeed = Booster_Extra.flameSpeed;
                    p.Play();
                }
                Booster_Extra.FlameSizeShapeBehaviour(p);
            }
        }
    }
    private void NoThrustParticles()
    {
        foreach (ParticleSystem p in allParticles)
        {
            if (p.gameObject.tag == "BoosterEffect")
            {
                if (p.startSize >= 0 && p.time >= 0.3f)
                {
                    p.startSize = 0;
                }
            }
        }
    }

    //Light
    private void LightDeactivate()
    {
        foreach (Light l in allLights)
        {
            if(l.gameObject.tag == "BoosterEffect")
            {
                if (l.enabled)
                    l.enabled = !l.enabled;
            }
        }
    }
    private void LightActivate()
    {
        foreach (Light l in allLights)
        {
            if (l.gameObject.tag == "BoosterEffect")
            {
                if (!l.enabled)
                    l.enabled = !l.enabled;
            }
        }
    }
}