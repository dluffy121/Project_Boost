using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //User Editable Fields
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip startSound;
    //Unlike AudioSource AudioClip.volume doesnot exists

    //Constant Fields
    const float minVol = 0.3f;
    public int currentLvl;
    public enum State { Alive, Dying, Transcending }
    public static State state;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        currentLvl = SceneManager.GetActiveScene().buildIndex;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        LaunchScript.lvlStart = true;
        state = State.Alive;
    }
    // Update is called once per frame
    void Update()
    {
        if (state != State.Dying)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return;}
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
        Invoke("LoadSameLvl", 2f);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        Invoke("LoadNextLvl", 1f);
    }

    private void LoadSameLvl()
    {
        SceneManager.LoadScene(currentLvl);
    }
    private void LoadNextLvl()
    {
        SceneManager.LoadScene(currentLvl + 1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);//'up' and 'forward' have unit 1
            VolumeControl();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //audioSource.Stop();                       //|tag2|
            VolumeControl(minVol);                      //|tag1|
        }
    }
    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;//To rotate naturally without physics

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; //Resume physics Control
    }

    //If not using AudioClip use this                   //|tag1|
    private void VolumeControl(float minVol)
    {
        StartCoroutine(AudioController.FadeOut(audioSource, 0.6f, minVol));//Use StartCoroutine to use another class function
    }
    private void VolumeControl()
    {
        if (!audioSource.isPlaying)//so it doesnt Layer //|tip|
        {
            audioSource.PlayOneShot(mainEngine);        //|tag2|
            audioSource.Play();
        }
        if (audioSource.volume <= minVol)
        {
            audioSource.volume = 1f;
        }
    }
}