using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class MonsterManager : MonoBehaviour
{
    // prefab of Monster
    [SerializeField] private MonsterScript MonsterPrefab;

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
        Monster = Instantiate(MonsterPrefab, Parent.transform);
        HungerDecay();
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
            HealthDecay();
        }

        HungerDecay();
    }
    public IEnumerator HealthDecay()
    {
        Monster.health -= subHealth;
        yield return new WaitForSeconds(healthDelay);
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
