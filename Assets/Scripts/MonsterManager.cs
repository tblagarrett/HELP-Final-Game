using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class MonsterManager : MonoBehaviour
{
    // variables for monster
    public MonsterScript Monster;
    public GameObject Parent;
    public CircleCollider2D VisualRadar;

    // variables for hunger and healh decay
    [SerializeField] private float hungerDelay;
    [SerializeField] private float healthDelay;
    [SerializeField] private int subHunger;
    [SerializeField] private int subHealth;


    //public PlayerManager Player;

    void Start()
    {
        // instantiate monster into the scene
        Monster = Instantiate(Monster, Parent.transform);

        // set colliders
        VisualRadar = Monster.GetComponent<CircleCollider2D>();
        VisualRadar.radius = Monster.curVisRadius;

        // start hunger and health decay
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
        } else if (Monster.hunger > 0 && Monster.health != 0)
        {
            StartCoroutine(HungerDecay());
        }
    }
    public void SetMax()
    {
        Monster.curVisRadius = Monster.maxVisRadius;
        VisualRadar.radius = Monster.curVisRadius;
    }

    public void SetMin()
    {
        Monster.curVisRadius = Monster.minVisRadius;
        VisualRadar.radius = Monster.curVisRadius;
    }

    public void ChasePlayer()
    {

    }

    public void HitPlayer()
    {

    }
}
