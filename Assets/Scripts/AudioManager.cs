using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //audio source
    AudioSource aud;

    //player and monster managers
    public PlayerManager PlayMan { get; set; }
    public MonsterManager MonMan { get; set; }

    //checkers
    float m_dist;
    bool isEat;

    //audio clips
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip atk;
    [SerializeField] private AudioClip walk;

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
            aud.clip = hurt;
            aud.Play();
        }
        else
        {
            aud.Stop();
        }

    }
}
