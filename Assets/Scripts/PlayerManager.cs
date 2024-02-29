using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D rb;
    //player objects
    public PlayerScripts Player;
    public GameObject stick;
    [SerializeField] private PlayerStateMachine PlayerSM;

    //monster ref
    [SerializeField] private MonsterManager MonsterMan;

    //instances for singleton
    private static PlayerManager _instance; // make a static private variable of the component data type
    public static PlayerManager Instance { get { return _instance; } } // make a public way to access the private variable\

    //variables for hunger and health decay
    [SerializeField] private float hungerDelay;
    [SerializeField] private float healthDelay;
    [SerializeField] private float slashDelay;
    [SerializeField] private int subHunger;
    [SerializeField] private int subHealth;

    //state machine vars
    public bool idle = true;
    public bool walking = false;
    public bool hurt = false;
    public bool attacking = false;

    //anims
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    public Animator anim;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody2D>();
        anim = stick.GetComponent<Animator>();
        Player.sRen.sprite = down;

        StartCoroutine(HungerDecay());

        stick.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        //setting state
        if(Input.anyKey == false)
        {
            idle = true;
        }

        if(attacking == false && hurt == false)
        {
            if (Input.anyKey)
            {
                walking = true;
                idle = false;
            }
            else
            {
                idle = true;
                walking = false;
            }
        }

        
        //slash + coroutine
        if(Input.GetMouseButtonDown(0))
        {
            Slash();
        }
    }

    //health access func
    public int GetHealth()
    {
        return Player.health;
    }

    //health manipulation func
    public void ModHealth(int mod)
    {
        Player.health += mod;

    }

    //hunger coroutine
    public IEnumerator HungerDecay()
    {
        yield return new WaitForSeconds(hungerDelay);
        Player.hunger -= subHunger;

        if (Player.hunger <= 0)
        {
            StartCoroutine(HealthDecay());
        }
        else
        {
            StartCoroutine(HungerDecay());
        }
    }

    //health coroutine
    public IEnumerator HealthDecay()
    {
        ModHealth(-subHealth);
        hurt = true;
        yield return new WaitForSeconds(healthDelay);
        hurt = false;

        if (Player.hunger <= 0 && Player.health != 0)
        {
            StartCoroutine(HealthDecay());
        }
    }

    public IEnumerator Slash()
    {
        attacking = true;
        yield return new WaitForSeconds(slashDelay);
        attacking = false;
    }

}
