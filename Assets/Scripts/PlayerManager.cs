using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using UnityEngine.Rendering.Universal;
using System.Runtime.InteropServices;
using static PlayerStateMachine;

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
    [SerializeField] private int heal;          // how much to heal when eating

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

    //vignette
    public GameObject vignetteObj;
    private VignetteClass vignetteManager;
    [SerializeField] private int vignetteAnimThreshold;

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
        // DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody2D>();
        anim = stick.GetComponent<Animator>();
        Player.sRen.sprite = down;
        vignetteManager = vignetteObj.GetComponent<VignetteClass>();

        StartCoroutine(HungerDecay());

        stick.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //setting state
        if (Input.anyKey == false)
        {
            idle = true;
        }

        if (attacking == false)
        {
            //slash + coroutine
            if (Input.GetMouseButtonDown(0))
            {
                walking = false;
                idle = false;
                StartCoroutine(Slash());
            }
        }


        //checking walking
        if (attacking == false && hurt == false)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
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
        if (Player.health > Player.maxHealth)
        {
            Player.health = Player.maxHealth;
        }

        if (Player.health <= 0)
        {
            // MUST START GAME FROM UI SCENE FOR THIS NOT TO ERROR
            UIManager.Instance.GoToMenu(GameMenu.GameOver);
        }

        // Start and stop vignette coroutine as necessary
        if (!vignetteManager.isPlaying && Player.health <= vignetteAnimThreshold)
        {
            StartCoroutine(vignetteManager.PlayVignetteHurt());
        }

        if (vignetteManager.isPlaying && Player.health > vignetteAnimThreshold)
        {
            vignetteManager.StopVignetteHurt();
        }

        // Adjust hearts UI
        UIManager.Instance.SetHearts(Player.health);
    }

    public void ModHunger(int mod)
    {
        Player.hunger += mod;
        if (Player.hunger > Player.maxHunger)
        {
            Player.hunger = Player.maxHunger;
            ModHealth(heal);
        }
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

        if (walking)
        {
            PlayerSM.TransitionToState(PlayerState.PLAY_WALK);
        } else if (hurt)
        {
            PlayerSM.TransitionToState(PlayerState.PLAY_HURT);
        } else {
            PlayerSM.TransitionToState(PlayerState.PLAY_IDLE);
        }
    }

    public void RightSwing()
    {
        anim.Play("Right Hit");
    }
    public void LeftSwing()
    {
        anim.Play("Left Hit");
    }
    public void UpSwing()
    {
        anim.Play("Up Hit");
    }
    public void DownSwing()
    {
        anim.Play("Down Hit");
    }

}
