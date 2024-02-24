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
    public bool idle = true;
    public bool chasing = false;
    public bool attacking = false;
    public bool walking = false;
    public bool sleeping = false;
    public bool hurt = false;

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

        // navmeshagent
        Agent = Monster.GetComponent<NavMeshAgent>();

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

    public IEnumerator StartWalking()
    {
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
        timer = Random.Range(1f, 5f);

        Agent.SetDestination(destination);

        // end of timer stop walking
        yield return new WaitForSeconds(timer);
        Agent.ResetPath();
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
}
