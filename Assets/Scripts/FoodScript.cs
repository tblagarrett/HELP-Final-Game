using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    // number of this foods placement in the active food array
    public int foodArray;

    public MapManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        Manager = transform.parent.parent.GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Monster")
        {
            this.gameObject.SetActive(false);

            // set its place in the array to null 
            Manager.activeFood[foodArray] = new Vector2(-10000,-10000);
        }
    }
}
