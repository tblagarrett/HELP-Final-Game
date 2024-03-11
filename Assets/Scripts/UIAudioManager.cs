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


    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = clip1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound()
    {
        StartCoroutine(Click());
    }

    public IEnumerator Click()
    {
        aud.Play();
        yield return new WaitForSeconds(0.14f);
        aud.Stop();
    }
}
