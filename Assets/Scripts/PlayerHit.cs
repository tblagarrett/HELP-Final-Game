using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] private BoxCollider2D MonsterBody;
    [SerializeField] private MonsterManager Manager;
    public MonsterScript monster;

    public int playerDamage;

    // Start is called before the first frame update
    private void Start()
    {
        Manager = FindAnyObjectByType<MonsterManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("Hit Monster");
            Manager.HurtMonster(playerDamage);
            Manager.curHit--;
        }
    }
}
