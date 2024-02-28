using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public class PlayerManager : MonoBehaviour
{
    //movement vars
    [SerializeField] private float moveSpeed;
    float speedX, speedY;
    private Rigidbody2D rb;

    //player objects
    [SerializeField] private PlayerScripts Player;
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
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite right;

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
        Player.sRen.sprite = down;

        StartCoroutine(HungerDecay());

    }

    // Update is called once per frame
    void Update()
    {
        //x movement
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        //y movement
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector2(speedX, speedY).normalized * moveSpeed;

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

        //check sprite based on movement
        if(walking == true)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Player.sRen.sprite = up;
            }
            else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Player.sRen.sprite = down;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Player.sRen.sprite = left;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Player.sRen.sprite = right;
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
        yield return new WaitForSeconds(healthDelay);
        hurt = true;

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
