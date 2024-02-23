using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class MonsterManager : MonoBehaviour
{
    public MonsterScript Monster;
    public MonsterStateMachine MonsterStateMachine;

    //public PlayerManager Player;

    void Start()
    {
        MonsterStateMachine = Monster.GetComponent<MonsterStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
