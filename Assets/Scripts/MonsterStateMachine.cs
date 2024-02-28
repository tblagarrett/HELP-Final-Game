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
        MON_WALK,
        MON_SLEEP,
        MON_ATTACK,
        MON_HURT,
        MON_CHASE,
        MON_DEATH
    }
    private void Awake()
    {
        Init(MonsterState.MON_IDLE,
            AbstractState.Create<MonIdleState, MonsterState>(MonsterState.MON_IDLE, this),
            AbstractState.Create<MonWalkState, MonsterState>(MonsterState.MON_WALK, this),
            AbstractState.Create<MonSleepState, MonsterState>(MonsterState.MON_SLEEP, this),
            AbstractState.Create<MonAttackState, MonsterState>(MonsterState.MON_ATTACK, this),
            AbstractState.Create<MonHurtState, MonsterState>(MonsterState.MON_HURT, this),
            AbstractState.Create<MonChaseState, MonsterState>(MonsterState.MON_CHASE, this),
            AbstractState.Create<MonDeathState, MonsterState>(MonsterState.MON_DEATH, this)
        );

        Manager = transform.GetComponent<MonsterManager>();
    }
    public class MonIdleState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // start idle 
            GetStateMachine<MonsterStateMachine>().Manager.StartIdle();
        }
        public override void OnUpdate()
        {
            if(GetStateMachine<MonsterStateMachine>().Manager.chasing)
            {
                GetStateMachine<MonsterStateMachine>().Manager.StopIdle();
                TransitionToState(MonsterState.MON_CHASE);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.sleeping)
            {
                TransitionToState(MonsterState.MON_SLEEP);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                GetStateMachine<MonsterStateMachine>().Manager.StopIdle();
                TransitionToState(MonsterState.MON_HURT);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.walking)
            {
                TransitionToState(MonsterState.MON_WALK);
            }
        }
        public override void OnExit()
        {
            // if hurt from hunger decay return to idle
            if (!GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                GetStateMachine<MonsterStateMachine>().Manager.idle = false;
            }
        }
    }
    public class MonWalkState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // start walking in one direction
            GetStateMachine<MonsterStateMachine>().Manager.StartWalking();
        }
        public override void OnUpdate()
        {
            //Debug.Log("Chek while walk");
            if (GetStateMachine<MonsterStateMachine>().Manager.chasing)
            {
                GetStateMachine<MonsterStateMachine>().Manager.StopWalking();
                TransitionToState(MonsterState.MON_CHASE);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.sleeping)
            {
                Debug.Log("Enter sleep");
                TransitionToState(MonsterState.MON_SLEEP);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                Debug.Log("Hurt");
                GetStateMachine<MonsterStateMachine>().Manager.StopWalking();
                TransitionToState(MonsterState.MON_HURT);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.idle)
            {
                TransitionToState(MonsterState.MON_IDLE);
            }
        }
        public override void OnExit()
        {
            // if hurt because of health decay return back to walking state
            if (!GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                GetStateMachine<MonsterStateMachine>().Manager.walking = false;
            }
        }
    }
    public class MonSleepState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // it's visibility is min
            GetStateMachine<MonsterStateMachine>().Manager.SetMin();

            // start coroutine (sleep timer)
            GetStateMachine<MonsterStateMachine>().Manager.StartSleep();
        }
        public override void OnUpdate()
        {
            if (GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                GetStateMachine<MonsterStateMachine>().Manager.StopSleep();
                TransitionToState(MonsterState.MON_HURT);
            }

            if (!GetStateMachine<MonsterStateMachine>().Manager.sleeping) // return to idle once timer is up
            {
                TransitionToState(MonsterState.MON_IDLE);
            }
        }
        public override void OnExit()
        {
            GetStateMachine<MonsterStateMachine>().Manager.SetMax();
        }
    }
    public class MonAttackState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

        }
        public override void OnUpdate()
        {
            if (GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                TransitionToState(MonsterState.MON_HURT);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.idle) // change to once anim over
            {
                TransitionToState(MonsterState.MON_IDLE); // will return to previous state
            } 
        }
        public override void OnExit()
        {
            GetStateMachine<MonsterStateMachine>().Manager.attacking = false;
        }
    }
    public class MonHurtState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim
            GetStateMachine<MonsterStateMachine>().Manager.StartHurting();
        }
        public override void OnUpdate()
        {
            if (!GetStateMachine<MonsterStateMachine>().Manager.hurt) // change to if anim over
            {
                // if health reaches 0 die
                if(GetStateMachine<MonsterStateMachine>().Manager.Monster.health == 0)
                {
                    TransitionToState(MonsterState.MON_DEATH);
                }

                Debug.Log("Back to idle");
                // send back to idle which will send it back to it's previous state
                TransitionToState(MonsterState.MON_IDLE);
            }
        }
        public override void OnExit()
        {
            //GetStateMachine<MonsterStateMachine>().Manager.hurt = false;
        }
    }

    public class MonChaseState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim
            
        }
        public override void OnUpdate()
        {
            // monster will chanse until it attacks or is hurt
            if (GetStateMachine<MonsterStateMachine>().Manager.attacking)
            {
                TransitionToState(MonsterState.MON_ATTACK);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                TransitionToState(MonsterState.MON_HURT);
            }

            // if player has left radar then return to idle
            if (!GetStateMachine<MonsterStateMachine>().Manager.chasing)
            {
                TransitionToState(MonsterState.MON_IDLE);
            }
        }
        public override void OnExit()
        {
          
        }
    }
    public class MonDeathState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim
            GetStateMachine<MonsterStateMachine>().Manager.Monster.enabled = false;
        }
        public override void OnUpdate()
        {

        }
        public override void OnExit()
        {
        }
    }
}
