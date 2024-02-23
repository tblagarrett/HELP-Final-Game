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

    //public PlayerManager Player;

    void Start()
    {
        Monster = Instantiate(MonsterPrefab, Parent.transform);
    }

    // Update is called once per frame
    void Update()
    {

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
