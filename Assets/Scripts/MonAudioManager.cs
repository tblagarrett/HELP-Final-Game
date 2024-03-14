using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonAudioManager : MonoBehaviour
{
    //audio source
    public AudioSource aud;

    //monster manager
    public MonsterManager MonMan;

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
        MonMan = this.GetComponentInParent<MonsterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MonMan.hurt)
        {
            Debug.Log("Hurtsound");
            aud.clip = hurt;
            aud.loop = false;
            aud.volume = 1;
            instate = true;
        }
        else if (MonMan.walking && !MonMan.eat)
        {
            aud.clip = walk;
            aud.loop = true;
            aud.volume = 0.5f;
            instate = true;

        }
        else if (MonMan.chasingAttack)
        {
            Debug.Log("Attack sound");
            aud.clip = atk;
            aud.loop = false;
            aud.volume = 1;
            instate = true;
        }
        else if (MonMan.eat)
        {
            aud.clip = eat;
            aud.loop = false;
            aud.volume = 1;
            instate = true;
            MonMan.eat = false;
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
