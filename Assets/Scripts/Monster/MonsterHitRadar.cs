using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitRadar : MonoBehaviour
{
    public MonsterManager Manager;
    private void Start()
    {
        Manager = transform.parent.parent.GetComponent<MonsterManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.attacking = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.attacking = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.attacking = false;
        }
    }
}