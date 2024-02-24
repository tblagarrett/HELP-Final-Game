using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitRadar : MonoBehaviour
{
    public MonsterManager Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.attacking = true;
        }
    }
}