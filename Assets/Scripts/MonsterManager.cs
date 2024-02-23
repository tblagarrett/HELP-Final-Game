using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class MonsterManager : MonoBehaviour
{
    public MonsterScript Monster;
    public GameObject Parent;

    // variables for hunger and healh decay
    [SerializeField] private float hungerDelay;
    [SerializeField] private float healthDelay;
    [SerializeField] private int subHunger;
    [SerializeField] private int subHealth;

    //public PlayerManager Player;

    void Start()
    {
        Monster = Instantiate(Monster, Parent.transform);
        StartCoroutine(HungerDecay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator HungerDecay()
    {
        yield return new WaitForSeconds(hungerDelay);
        Monster.hunger -= subHunger;

        if (Monster.hunger <= 0)
        {
            StartCoroutine(HealthDecay());
        }
        else
        {
            StartCoroutine(HungerDecay());
        }
    }
    public IEnumerator HealthDecay()
    {
        Monster.health -= subHealth;
        yield return new WaitForSeconds(healthDelay);

        if (Monster.hunger <= 0 && Monster.health != 0)
        {
            StartCoroutine(HealthDecay());
        }
    }
    public void SetMax()
    {
        Monster.curVisRadius = Monster.maxVisRadius;
    }

    public void SetMin()
    {
        Monster.curVisRadius = Monster.minVisRadius;
    }
}
