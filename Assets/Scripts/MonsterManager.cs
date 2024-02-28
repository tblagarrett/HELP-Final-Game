using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    // access Managers
    public MapManager MapManager;
    public PlayerManager PlayerManager;

    // variables for monster
    public MonsterScript Monster;
    public GameObject Parent;
    public CircleCollider2D VisualRadar;
    private NavMeshAgent Agent;

    // variables for hunger and healh decay
    [SerializeField] private float hungerDelay;
    [SerializeField] private float healthDelay;
    [SerializeField] private int subHunger;
    [SerializeField] private int subHealth;

    // variables for state machine
    public bool idle = true;        // 1
    public bool sleeping = false;   // 2
    public bool walking = false;    // 3, 4
    public bool attacking = false;
    public bool chasing = false;
    public bool hurt = false;

    // variables to store coroutines
    private Coroutine IdleCo;
    private Coroutine SleepCo;
    private Coroutine WalkCo;

    // random timer for states
    private float timer;

    // randomly choose axis
    // 0 = x axis
    // 1 = y axis;
    private int axis;

    //public PlayerManager Player;

    void Start()
    {
        // instantiate monster into the scene
        Monster = Instantiate(Monster, Parent.transform);
        Monster.GetComponent<BoxCollider2D>().tag = "Monster";

        // navmeshagent
        Agent = Monster.GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        // for more information https://github.com/h8man/NavMeshPlus/wiki/HOW-TO#nav-mesh-basics

        // set colliders
        VisualRadar = Monster.transform.Find("VisualRadar").GetComponent<CircleCollider2D>();
        VisualRadar.radius = Monster.curVisRadius;
        Debug.Log("set vis");

        // start hunger and health decay
        StartCoroutine(HungerDecay());
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
        yield return new WaitForSeconds(healthDelay);
        Monster.health -= subHealth;
        hurt = true;

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

    public IEnumerator Idle()
    {
        Debug.Log("Idle");
        if (idle) // if entering idle state to be idle
        {
            timer = Random.Range(1f, 5f);
            yield return new WaitForSeconds(timer);
        } // else only entering idle to switch to another state

        int state = Random.Range(2, 5); // choose to walk or to sleep
        if(state > 2) { walking = true; } else { sleeping = true; }
    }

    public IEnumerator Sleep()
    {
        Debug.Log("Sleep");
        timer = Random.Range(1f, 5f);
        yield return new WaitForSeconds(timer);

        // finish sleeping
        sleeping = false;
    }

    public IEnumerator Walking()
    {
        Debug.Log("Walking");
        // decide destination
        Vector2 destination = NearestFood();

        // decide an axis
        axis = Random.Range(0, 2);

        // create destination for axis
        if(axis == 0)
        {
            destination = new Vector2(destination.x, Monster.transform.position.y);
        } else
        {
            destination = new Vector2(Monster.transform.position.x, destination.y);
        }

        // decide how long to walk in this direction
        timer = Random.Range(2f, 7f);

        Debug.Log(destination);
        Debug.Log(Monster.transform.position);
        Agent.SetDestination(destination);

        // end of timer stop walking
        yield return new WaitForSeconds(timer);
        Agent.ResetPath();

        Debug.Log("Stop Walk");
        // choose new state
        int state = Random.Range(1, 5); // choose to walk, sleep, or idle
        Debug.Log(state);
        if (state > 2) { StartWalking(); } else if (state == 2) { sleeping = true; Debug.Log("seelpign truw"); } else { idle = true; }
    }

    // iterates through all active food
    // also checks player location
    // returns closest one
    private Vector2 NearestFood()
    {
        Vector2 distance;
        Vector2 closest = new Vector2(0,0);
        int curFood = -1;

        for (int i = 0; i < MapManager.activeFood.Length; i++)
        {
            // skip food that is no longer active
            if(MapManager.activeFood[i] == new Vector2(-10000, -10000)) { continue; }

            distance = new Vector2(Monster.transform.position.x, Monster.transform.position.y) - MapManager.activeFood[i];
            
            // if the current closest food is farther away then replace
            if (curFood < 0 || ((Mathf.Abs(closest.x) + Mathf.Abs(closest.y)) > (Mathf.Abs(distance.x) + Mathf.Abs(distance.y)))) 
            {
                closest = distance;
                curFood = i;
            }
        }

        // check player distance
        distance = Monster.transform.position - PlayerManager.transform.position;
        if (((Mathf.Abs(closest.x) + Mathf.Abs(closest.y)) > (Mathf.Abs(distance.x) + Mathf.Abs(distance.y))))
        {
            closest = PlayerManager.transform.position; // just to convert vector3 to vector 2
            return closest; // return player position
        } else
        {
            return MapManager.activeFood[curFood]; // return closest food
        }
    }

    public void Chasing()
    {
        Debug.Log("Chasing");
        Agent.SetDestination(PlayerManager.transform.position);
    }

    public void Attacking()
    {
        // instantiate attack collider where monster is facing
    }

    // temp hurt timer
    public IEnumerator Hurting()
    {
        Debug.Log("Start hurting");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Stop hurting");
        hurt = false;
    }

    // for player to use when attacking monster
    public void HurtMonster(int damage)
    {
        Monster.health -= damage;
        hurt = true;
    }

    // functions for statemachine to start and stop coroutines
    public void StartIdle()
    {
        IdleCo = StartCoroutine(Idle());
    }

    public void StopIdle()
    {
        StopCoroutine(IdleCo);
    }
    public void StartSleep()
    {
        SleepCo = StartCoroutine(Sleep());
    }

    public void StopSleep()
    {
        sleeping = false;
        StopCoroutine(SleepCo);
    }
    public void StartWalking()
    {
        WalkCo = StartCoroutine(Walking());
    }

    public void StopWalking()
    {
        Debug.Log("Stop Walking coroutine");
        Agent.ResetPath();
        StopCoroutine(WalkCo);
    }

    public void StartHurting()
    {
        StartCoroutine(Hurting());
    }
}
