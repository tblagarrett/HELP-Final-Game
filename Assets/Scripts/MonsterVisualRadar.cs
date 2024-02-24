using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVisualRadar : MonoBehaviour
{
    public MonsterManager Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Manager.ChasePlayer();
        }
    }
}
