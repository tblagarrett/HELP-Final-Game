using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    // number of this foods placement in the active food array
    public int foodArray;

    public MapManager Manager;

    private int hungerIncrease = 2;

    //player audio manager
    public AudioManager p_aud;

    // Start is called before the first frame update
    void Start()
    {
        Manager = transform.parent.parent.GetComponent<MapManager>();
        p_aud = FindFirstObjectByType<AudioManager>();
    }
     
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Monster")
        {
            var ps = GetComponent<ParticleSystem>();
            ps.Play();
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            // set its place in the array to null 
            Manager.activeFood[foodArray] = new Vector2(-10000,-10000);

            if(collision.gameObject.tag == "Player")
            {
                PlayerManager.Instance.ModHunger(hungerIncrease);

                // for the audio manager
                PlayerManager.Instance.eat = true;
            }

            if (collision.gameObject.tag == "Monster")
            {
                Manager.MonManager.ModHunger(hungerIncrease);
            }
        }
    }
}
