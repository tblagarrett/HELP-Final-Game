using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private BoxCollider2D hitCollider;
    public MonsterManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        hitCollider = GetComponent<BoxCollider2D>();
        Manager = transform.parent.parent.GetComponent<MonsterManager>();
    }

    public void TurnOnAttack()
    {
        Debug.Log("Collider on");
        hitCollider.enabled = true;
    }
    public void TurnOffAttack()
    {
        Debug.Log("Collider off");
        hitCollider.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.stay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.stay = false;
        }
    }
}
