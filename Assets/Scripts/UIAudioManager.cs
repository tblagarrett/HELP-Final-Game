using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    //audio source
    public AudioSource aud;

    //UI Manager
    public UIManager man;

    //click audio clips
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;

    //checking playing
    bool toggleplay;


    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
