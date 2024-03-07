using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVisualRadar : MonoBehaviour
{
    public MonsterManager Manager;

    private void Start()
    {
        Manager = transform.parent.parent.GetComponent<MonsterManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Seen");
        if (collision.gameObject.tag == "Player") {
            Manager.chasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Manager.chasing = false;
        }
    }
}
