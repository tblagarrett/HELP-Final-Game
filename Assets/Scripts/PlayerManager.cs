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

    //instances for singleton
    private static PlayerManager _instance; // make a static private variable of the component data type
    public static PlayerManager Instance { get { return _instance; } } // make a public way to access the private variable\

    //variables for hunger and health decay
    [SerializeField] private float hungerDelay;
    [SerializeField] private float healthDelay;
    [SerializeField] private int subHunger;
    [SerializeField] private int subHealth;

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
    }

    public int GetHealth()
    {
        return Player.health;
    }

    public void ModHealth(int mod)
    {
        Player.health += mod;

    }

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

    public IEnumerator HealthDecay()
    {
        ModHealth(-subHealth);
        yield return new WaitForSeconds(healthDelay);

        if (Player.hunger <= 0 && Player.health != 0)
        {
            StartCoroutine(HealthDecay());
        }
    }

}
