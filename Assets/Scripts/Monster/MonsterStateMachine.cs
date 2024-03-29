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
        MON_WAKEUP,
        MON_ATTACK,
        MON_HURT,
        MON_CHASE,
        MON_RUN,
        MON_DEATH
    }
    private void Awake()
    {
        Init(MonsterState.MON_IDLE,
            AbstractState.Create<MonIdleState, MonsterState>(MonsterState.MON_IDLE, this),
            AbstractState.Create<MonWalkState, MonsterState>(MonsterState.MON_WALK, this),
            AbstractState.Create<MonSleepState, MonsterState>(MonsterState.MON_SLEEP, this),
            AbstractState.Create<MonWakeState, MonsterState>(MonsterState.MON_WAKEUP, this),
            AbstractState.Create<MonAttackState, MonsterState>(MonsterState.MON_ATTACK, this),
            AbstractState.Create<MonHurtState, MonsterState>(MonsterState.MON_HURT, this),
            AbstractState.Create<MonChaseState, MonsterState>(MonsterState.MON_CHASE, this),
            AbstractState.Create<MonRunState, MonsterState>(MonsterState.MON_RUN, this),
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
                if (GetStateMachine<MonsterStateMachine>().Manager.idle)
                {
                    GetStateMachine<MonsterStateMachine>().Manager.StopIdle();
                }
                Debug.Log("From idle to chase");
                TransitionToState(MonsterState.MON_CHASE);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.sleeping)
            {
                TransitionToState(MonsterState.MON_SLEEP);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.hurt)
            {
                if (GetStateMachine<MonsterStateMachine>().Manager.idle)
                {
                    GetStateMachine<MonsterStateMachine>().Manager.StopIdle();
                    GetStateMachine<MonsterStateMachine>().Manager.idle = true;
                }
                TransitionToState(MonsterState.MON_HURT);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.walking)
            {
                TransitionToState(MonsterState.MON_WALK);
            }
        }
        public override void OnExit()
        {

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
            //Play eating sound
            if (GetStateMachine<MonsterStateMachine>().Manager.eat)
            {
                GetStateMachine<MonsterStateMachine>().Manager.Eating();
            }

            // check for direction
            float angle = GetStateMachine<MonsterStateMachine>().Manager.Agent.transform.eulerAngles.z;
            if (angle == 270)
            {
                GetStateMachine<MonsterStateMachine>().Manager.RightIdle();
            }
            else if (angle == 180)
            {
                GetStateMachine<MonsterStateMachine>().Manager.FrontIdle();
            }
            else if (angle == 90)
            {
                GetStateMachine<MonsterStateMachine>().Manager.LeftIdle();
            }
            else if (angle == 0)
            {
                GetStateMachine<MonsterStateMachine>().Manager.BackIdle();
            }

            // switch states
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
                GetStateMachine<MonsterStateMachine>().Manager.SetMax();
                GetStateMachine<MonsterStateMachine>().Manager.StopSleep();
                TransitionToState(MonsterState.MON_HURT);
            }

            if (GetStateMachine<MonsterStateMachine>().Manager.chasing)
            {
                Debug.Log("Start Wake up");
                TransitionToState(MonsterState.MON_WAKEUP);
            }

            if (!GetStateMachine<MonsterStateMachine>().Manager.sleeping) // return to idle once timer is up
            {
                GetStateMachine<MonsterStateMachine>().Manager.SetMax();
                TransitionToState(MonsterState.MON_IDLE);
            }
        }
    }
    public class MonWakeState : AbstractState
    {
        public override void OnEnter()
        {
            // start coroutine (wake up timer)
            GetStateMachine<MonsterStateMachine>().Manager.StartWakeup();
        }
        public override void OnUpdate()
        {
            if(GetStateMachine<MonsterStateMachine>().Manager.awaken)
            {
                GetStateMachine<MonsterStateMachine>().Manager.StopSleep();
                TransitionToState(MonsterState.MON_CHASE);
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
            GetStateMachine<MonsterStateMachine>().Manager.StartAttacking();
            Debug.Log("in attack state");
        }
        public override void OnUpdate()
        {
            // checking for collision and attack
            // Debug.Log("Chasing attack is " + GetStateMachine<MonsterStateMachine>().Manager.chasingAttack);
            GetStateMachine<MonsterStateMachine>().Manager.HurtPlayer();

            if (GetStateMachine<MonsterStateMachine>().Manager.tempattackcool) // change to once anim over
            {
                TransitionToState(MonsterState.MON_CHASE); // will return to previous state
            } 
        }
        public override void OnExit()
        {
            Debug.Log("leave attack state");
            GetStateMachine<MonsterStateMachine>().Manager.EndAttack();
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
                if(GetStateMachine<MonsterStateMachine>().Manager.Monster.health <= 0)
                {
                    Debug.Log("Dead");
                    TransitionToState(MonsterState.MON_DEATH);
                } else if (GetStateMachine<MonsterStateMachine>().Manager.running)
                {
                    TransitionToState(MonsterState.MON_RUN);
                } else
                {
                    Debug.Log("Back to idle");
                    // send back to idle which will send it back to it's previous state
                    TransitionToState(MonsterState.MON_IDLE);
                }
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
            Debug.Log("Enter Chasing");
            // speed up
            GetStateMachine<MonsterStateMachine>().Manager.Agent.speed = 4;
            GetStateMachine<MonsterStateMachine>().Manager.WalkingAud();

            // choose how many times it will get hit before leaving
            // only set if this is a new encounter
            if (GetStateMachine<MonsterStateMachine>().Manager.curHit == 0)
            {
                GetStateMachine<MonsterStateMachine>().Manager.curHit = Random.Range(GetStateMachine<MonsterStateMachine>().Manager.minHit, GetStateMachine<MonsterStateMachine>().Manager.maxHit);
                Debug.Log("Curhit = " + GetStateMachine<MonsterStateMachine>().Manager.curHit);
            }
        }
        public override void OnUpdate()
        {
            //Play eating sound
            if (GetStateMachine<MonsterStateMachine>().Manager.eat)
            {
                GetStateMachine<MonsterStateMachine>().Manager.Eating();
            }

            // chase player
            GetStateMachine<MonsterStateMachine>().Manager.Chasing();

            // check for direction
            float angle = GetStateMachine<MonsterStateMachine>().Manager.Agent.transform.eulerAngles.z;
            if (angle == 270)
            {
                GetStateMachine<MonsterStateMachine>().Manager.RightIdle();
            }
            else if (angle == 180)
            {
                GetStateMachine<MonsterStateMachine>().Manager.FrontIdle();
            }
            else if (angle == 90)
            {
                GetStateMachine<MonsterStateMachine>().Manager.LeftIdle();
            }
            else if (angle == 0)
            {
                GetStateMachine<MonsterStateMachine>().Manager.BackIdle();
            }

            // monster will chanse until it attacks or is hurt
            if (GetStateMachine<MonsterStateMachine>().Manager.chasingAttack)
            {
                Debug.Log("Attack now");
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
            GetStateMachine<MonsterStateMachine>().Manager.StopChasing();
        }
    }
    public class MonRunState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim

            // speed up
            GetStateMachine<MonsterStateMachine>().Manager.Agent.speed = 6;

            // start running
            GetStateMachine<MonsterStateMachine>().Manager.StartRun();
            GetStateMachine<MonsterStateMachine>().Manager.WalkingAud();
        }
        public override void OnUpdate()
        {
            //Play eating sound
            if (GetStateMachine<MonsterStateMachine>().Manager.eat)
            {
                GetStateMachine<MonsterStateMachine>().Manager.Eating();
            }

            float angle = GetStateMachine<MonsterStateMachine>().Manager.Agent.transform.eulerAngles.z;
            if (angle == 270)
            {
                GetStateMachine<MonsterStateMachine>().Manager.RightIdle();
            }
            else if (angle == 180)
            {
                GetStateMachine<MonsterStateMachine>().Manager.FrontIdle();
            }
            else if (angle == 90)
            {
                GetStateMachine<MonsterStateMachine>().Manager.LeftIdle();
            }
            else if (angle == 0)
            {
                GetStateMachine<MonsterStateMachine>().Manager.BackIdle();
            }

            if (!GetStateMachine<MonsterStateMachine>().Manager.running)
            {
                TransitionToState(MonsterState.MON_IDLE);
            }
        }
    }
    public class MonDeathState : AbstractState
    {
        public override void OnEnter()
        {
            // start anim
            GetStateMachine<MonsterStateMachine>().Manager.gameObject.SetActive(false);
            UIManager.Instance.SetWinScreen(true);
            UIManager.Instance.GoToMenu(GameMenu.GameOver);
        }
    }
}
