using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //audio source
    public AudioSource aud;

    //player and monster managers
    public PlayerManager PlayMan;

    //audio clips
    public AudioClip hurt;
    public AudioClip eat;
    public AudioClip atk;
    public AudioClip walk;

    //checking playing
    bool instate;
    bool toggleplay;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayMan.hurt)
        {
            aud.clip = hurt;
            aud.loop = false;
            instate = true;
        }else if (PlayMan.walking)
        {
            aud.clip = walk;
            aud.loop = true;
            aud.volume = 0.5f;
            instate = true;

        }
        else if (PlayMan.attacking)
        {
            aud.clip = atk;
            aud.loop = false;
            aud.volume = 1;
            instate = true;
        }
        else if (PlayMan.eat)
        {
            aud.clip = eat;
            aud.loop = false;
            aud.volume = 1;
            instate = true;
        }
        else
        {
            instate = false;
        }

        if (instate == true && toggleplay == false)
        {
            aud.Play();
            toggleplay = true;
        }
        else if (instate == false)
        {
            aud.Stop();
            toggleplay = false;
        }


        if (aud.clip == eat)
        {
            aud.loop = false;
        }
    }
}
