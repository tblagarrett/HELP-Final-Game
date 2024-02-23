using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using System.Threading;
public class MonsterStateMachine : AbstractFiniteStateMachine
{
    public MonsterManager Manager { get; set; }
    public enum MonsterState
    {
        MON_IDLE,
        MON_RUN,
        MON_SLEEP,
        MON_ATTACK
    }
    private void Awake()
    {
        Init(MonsterState.MON_IDLE,
            AbstractState.Create<MonIdleState, MonsterState>(MonsterState.MON_IDLE, this),
            AbstractState.Create<MonRunState, MonsterState>(MonsterState.MON_RUN, this),
            AbstractState.Create<MonSleepState, MonsterState>(MonsterState.MON_SLEEP, this),
            AbstractState.Create<MonAttackState, MonsterState>(MonsterState.MON_ATTACK, this)
        );
    }
    public class MonIdleState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // it's visibility is max
            GetStateMachine<MonsterStateMachine>().Manager.SetMax();
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
    public class MonRunState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // it's visibility is max
            GetStateMachine<MonsterStateMachine>().Manager.SetMax();
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
    public class MonSleepState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // it's visibility is min
            GetStateMachine<MonsterStateMachine>().Manager.SetMin();
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
    public class MonAttackState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // it's visibility is max
            GetStateMachine<MonsterStateMachine>().Manager.SetMax();
        }
        public override void OnUpdate()
        {
        }
        public override void OnExit()
        {
        }
    }
}
