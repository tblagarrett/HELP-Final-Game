using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using UnityEngine.AI;
using System.IO;

public class MonsterManager : MonoBehaviour
{
    // access Managers
    public MapManager MapManager;
    public PlayerManager PlayerManager;
    public GameObject Player;

    // variables for monster
    public MonsterScript Monster;
    public GameObject Parent;
    public CircleCollider2D VisualRadar;
    public NavMeshAgent Agent;
    public MonsterAttack MonAttack;

    // variables for hunger and healh decay
    [SerializeField] private float hungerDelay;
    [SerializeField] private float healthDelay;
    [SerializeField] private int subHunger;
    [SerializeField] private int subHealth;

    // variables for state machine
    public bool idle = true;        
    public bool sleeping = false;   
    public bool walking = false;    
    public bool chasing = false;
    public bool hurt = false;

    // random selected state
    [SerializeField] private List<string> states;
    [SerializeField] private List<float> weights;

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

    // variables for attacking
    public bool attacking = false;  // in attacking distance - can transition to state - turned on/off by MonsterHitRadar.cs
    public bool stay = false;       // Player is in attacking area - turned on/off by MonsterAttack.cs
    public bool chasingAttack = false;  // if attack cooldown is over transition to attack state - only checked to leave chasing state
    [SerializeField] private float attackDelay; // delay hurting the player (for animation purposes)
    [SerializeField] private int attackDamage;  // how much damage dealt
    [SerializeField] private int coolDownDelay; // delay next attack - in chasing state in mean time
    private bool attackCoolDown = true;         // can you attack? - waiting for cooldown
    public int maxHit;          // max amount of times the monster will tolerate being hit
    public int minHit;          // min amount of times the monster will tolerate being hit
    public int curHit = 0;      // current amount of time monster will be hit this encounter
    public bool tempattackcool = false; // delete later

    // sprites for directions
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    IEnumerator Start()
    {
        // wait for navmeshsurface to be created
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
      
        // instantiate monster into the scene
        Monster = Instantiate(Monster, Parent.transform);

        // navmeshagent
        Agent = Monster.GetComponent<NavMeshAgent>();
        Agent.updateUpAxis = false;
        // for more information https://github.com/h8man/NavMeshPlus/wiki/HOW-TO#nav-mesh-basics

        // set colliders
        VisualRadar = Monster.transform.Find("VisualRadar").GetComponent<CircleCollider2D>();
        VisualRadar.radius = Monster.curVisRadius;

        // get monster attack
        GameObject MonAttackChild = Monster.transform.GetChild(2).gameObject;
        MonAttack = MonAttackChild.GetComponent<MonsterAttack>();

        // get player
        Player = GameObject.FindGameObjectWithTag("Player");

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

    // referenced from chatGBT
    private string SelectState()
    {
        float randomNum = Random.Range(0f, SumOfWeights());

        float cumWeight = 0f;
        for (int i = 0; i < states.Count; i++)
        {
            cumWeight += weights[i];
            if(randomNum <= cumWeight)
            {
                return states[i];
            }
        }

        // in case
        return states[states.Count - 1];
    }
    private float SumOfWeights()
    {
        float sum = 0f;
        foreach(float weight in weights)
        {
            sum += weight;
        }
        return sum;
    }

    public IEnumerator Idle()
    {
        Debug.Log("Idle");
        if (idle) // if entering idle state to be idle
        {
            timer = Random.Range(1f, 5f);
            yield return new WaitForSeconds(timer);
            idle = false;
        } // else only entering idle to switch to another state

        string state = SelectState();
        if(state == "walk") { walking = true; } else if (state == "sleep") { sleeping = true; } else { StartIdle(); }
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
        Debug.Log(destination);

        /*/ path code referenced from ChatGBT
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(Agent.transform.position, destination, NavMesh.AllAreas, path);

        // turn agent towards direction
        if(path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 1)
        {
            // decide an axis
            Vector3 direction = path.corners[1] - Agent.transform.position;
        */
        Vector2 direction = destination - new Vector2(Agent.transform.position.x, Agent.transform.position.y);
        // turn to direction referenced from https://discussions.unity.com/t/prevent-navmesh-from-moving-diagonally/213824
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            Debug.Log(destination.x + ", " + Agent.transform.position.y);
            if (!Agent.SetDestination(new Vector2(destination.x, Agent.transform.position.y)))
            {
                Debug.Log("failed");
                Debug.Log(Agent.SetDestination(new Vector2(Agent.transform.position.x, destination.y)));
                direction.x = 0f;
            } else
            {
                direction.y = 0f;
            }
        } else
        {
            Debug.Log(Agent.transform.position.x + ", " + destination.y);
            if (!Agent.SetDestination(new Vector2(Agent.transform.position.x, destination.y)))
            {
                Debug.Log("failed");
                Debug.Log(Agent.SetDestination(new Vector2(destination.x, Agent.transform.position.y)));
                direction.y = 0f;
            }
            else
            {
                direction.x = 0f;
            }
        }
        if(direction != Vector2.zero)
        {
            Agent.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: direction.normalized); 
            Debug.Log(direction);
        }
        

        //RaycastHit2D hit = Physics2D.Raycast(Agent.transform.position, Agent.transform.TransformDirection(Vector3.forward), Random.Range(20, 30));

        // decide how long to walk in this direction
        timer = Random.Range(2f, 7f);

        // end of timer stop walking
        yield return new WaitForSeconds(timer);
        Agent.ResetPath();

        Debug.Log("Stop Walk");
        // choose new state
        string state = SelectState();
        if (state == "idle") { idle = true; } else if (state == "sleep") { sleeping = true; } else { StartWalking(); }
    }

    // iterates through all active food
    // also checks player location
    // returns closest one
    private Vector2 NearestFood()
    {
        Vector2 distance;
        Vector2 closest = Vector2.zero;
        Vector2 nullFood = new Vector2(-10000, -10000);
        int curFood = -1;

        for (int i = 0; i < MapManager.activeFood.Length; i++)
        {
            // skip food that is no longer active
            if(MapManager.activeFood[i] == nullFood) { continue; }

            distance = new Vector2(Monster.transform.position.x, Monster.transform.position.y) - MapManager.activeFood[i];
            
            // if the current closest food is farther away then replace
            if (curFood < 0 || ((Mathf.Abs(closest.x) + Mathf.Abs(closest.y)) > (Mathf.Abs(distance.x) + Mathf.Abs(distance.y)))) 
            {
                closest = distance;
                curFood = i;
            }
        }

        if (closest == Vector2.zero)
        {
            closest = Player.transform.position; // just to convert vector3 to vector 2
            return closest;
        }

        // check player distance
        distance = Monster.transform.position - Player.transform.position;
        if (((Mathf.Abs(closest.x) + Mathf.Abs(closest.y)) > (Mathf.Abs(distance.x) + Mathf.Abs(distance.y))))
        {
            closest = Player.transform.position; // just to convert vector3 to vector 2
            return closest; // return player position
        } else
        {
            return MapManager.activeFood[curFood]; // return closest food
        }
    }

    // follows the player
    public void Chasing()
    {
        if(curHit == 0)
        {
            chasing = false;
        }
        //Debug.Log("Chasing");
        Agent.SetDestination(Player.transform.position);

        Vector3 direction = Player.transform.position - Agent.transform.position;
        Agent.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: direction.normalized);
        
        // check for attacking
        ChaseAttack();
    }

    // checking if monster can attack
    public void ChaseAttack()
    {
        if(attacking && attackCoolDown) { 
            chasingAttack = true;
            tempattackcool = false;
        }
    }

    private IEnumerator AttackCool()
    {
        // delay next attack
        attackCoolDown = false;
        yield return new WaitForSeconds(coolDownDelay);
        attackCoolDown = true;
    }

    public IEnumerator Attacking()
    {
        // start attack anim
        MonAttack.TurnOnAttack();

        // delay attack
        Debug.Log("Turns attack off");
        chasingAttack = false;
        yield return new WaitForSeconds(attackDelay);

        // now start checking for collision
        Debug.Log("Turns attack on");
        chasingAttack = true;
    }

    public void EndAttack()
    {
        chasingAttack = false;
        MonAttack.TurnOffAttack();

        // delay next attack
        StartCoroutine(AttackCool());
    }

    public void HurtPlayer()
    {
        // if in range and not in cooldown/delay
        if (chasingAttack && stay)
        {
            chasingAttack = false;
            Debug.Log("Hurt PLayer");
            PlayerManager.ModHealth(-attackDamage); 
        }
    }

    public IEnumerator TempAttack() {
        yield return new WaitForSeconds(1);
        tempattackcool = true;
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
    // negative damage will heal monster
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

    public void StopChasing()
    {
        Agent.ResetPath();
        Agent.speed /= 3;
        //Agent.angularSpeed /= 2;
        chasingAttack = false;

        // choose next state if not going back to chasing
        if(chasing == false)
        {
            string state = SelectState();
            if (state == "walk") { walking = true; } else if (state == "sleep") { sleeping = true; } else { idle = true; }

            // reset hit max
            curHit = 0;
        }
    }
    public void StartAttacking()
    {
        StartCoroutine(Attacking());
        StartCoroutine(TempAttack());
    }
}
