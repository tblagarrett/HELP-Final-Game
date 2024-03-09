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

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayMan.hurt == true)
        {
            aud.resource = hurt;
            aud.Play();
        }
        else
        {
            aud.Stop();
        }

        if(aud.resource = eat)
        {
            aud.loop = false;
        }

        if(PlayMan.walking == true)
        {
            aud.resource = walk;
            aud.loop = true;
            aud.Play();
        }
        else
        {
            aud.Stop();
        }

        if (PlayMan.attacking == true)
        {
            aud.resource = atk;
            aud.loop = false;
            aud.Play();
        }
        else
        {
            aud.Stop();
        }

    }
}
