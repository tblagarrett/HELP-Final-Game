using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using System.Threading;

public class PlayerStateMachine : AbstractFiniteStateMachine
{
    //movement vars
    [SerializeField] private float moveSpeed;
    float speedX, speedY;
    private Rigidbody2D rb;
    public PlayerManager PlayMan { get; set; }
    public enum PlayerState
    {
        PLAY_IDLE,
        PLAY_WALK,
        PLAY_HURT,
        PLAY_ATTACK
    }

    private void Awake()
    {
        Init(PlayerState.PLAY_IDLE,
            AbstractState.Create<PlayIdleState, PlayerState>(PlayerState.PLAY_IDLE, this),
            AbstractState.Create<PlayWalkState, PlayerState>(PlayerState.PLAY_WALK, this),
            AbstractState.Create<PlayHurtState, PlayerState>(PlayerState.PLAY_HURT, this),
            AbstractState.Create<PlayAttackState, PlayerState>(PlayerState.PLAY_ATTACK, this)
        );

        PlayMan = transform.GetComponent<PlayerManager>();
        rb = PlayMan.Player.GetComponent<Rigidbody2D>();
    }

    public class PlayIdleState : AbstractState
    {
        public override void OnEnter()
        {
            //start anim

        }
        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerStateMachine>().PlayMan.walking)
            {
                TransitionToState(PlayerState.PLAY_WALK);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.hurt)
            {
                TransitionToState(PlayerState.PLAY_HURT);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.attacking)
            {
                TransitionToState(PlayerState.PLAY_ATTACK);
            }
        }
        public override void OnExit()
        {
            //do stuff
        }
    }

    public class PlayWalkState : AbstractState
    {
        public override void OnEnter()
        {
            //start anim

        }
        public override void OnUpdate()
        {
            //x movement
            GetStateMachine<PlayerStateMachine>().speedX = Input.GetAxisRaw("Horizontal") * GetStateMachine<PlayerStateMachine>().moveSpeed;
            //y movement
            GetStateMachine<PlayerStateMachine>().speedY = Input.GetAxisRaw("Vertical") * GetStateMachine<PlayerStateMachine>().moveSpeed;
            GetStateMachine<PlayerStateMachine>().rb.velocity = new Vector2(GetStateMachine<PlayerStateMachine>().speedX, GetStateMachine<PlayerStateMachine>().speedY).normalized * GetStateMachine<PlayerStateMachine>().moveSpeed;

            //check sprite based on movement
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                GetStateMachine<PlayerStateMachine>().PlayMan.Player.sRen.sprite = GetStateMachine<PlayerStateMachine>().PlayMan.up;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                GetStateMachine<PlayerStateMachine>().PlayMan.Player.sRen.sprite = GetStateMachine<PlayerStateMachine>().PlayMan.down;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                GetStateMachine<PlayerStateMachine>().PlayMan.Player.sRen.sprite = GetStateMachine<PlayerStateMachine>().PlayMan.left;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                GetStateMachine<PlayerStateMachine>().PlayMan.Player.sRen.sprite = GetStateMachine<PlayerStateMachine>().PlayMan.right;
            }


            if (GetStateMachine<PlayerStateMachine>().PlayMan.idle)
            {
                TransitionToState(PlayerState.PLAY_IDLE);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.hurt)
            {
                TransitionToState(PlayerState.PLAY_HURT);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.attacking)
            {
                TransitionToState(PlayerState.PLAY_ATTACK);
            }
        }
        public override void OnExit()
        {
            //do stuff
        }
    }

    public class PlayHurtState : AbstractState
    {
        public override void OnEnter()
        {
            //start anim

        }
        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerStateMachine>().PlayMan.walking)
            {
                TransitionToState(PlayerState.PLAY_WALK);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.idle)
            {
                TransitionToState(PlayerState.PLAY_IDLE);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.attacking)
            {
                TransitionToState(PlayerState.PLAY_ATTACK);
            }
        }
        public override void OnExit()
        {
            //do stuff
        }
    }

    public class PlayAttackState : AbstractState
    {
        public override void OnEnter()
        {
            //start anim

        }
        public override void OnUpdate()
        {
            if (GetStateMachine<PlayerStateMachine>().PlayMan.walking)
            {
                TransitionToState(PlayerState.PLAY_WALK);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.hurt)
            {
                TransitionToState(PlayerState.PLAY_HURT);
            }
            if (GetStateMachine<PlayerStateMachine>().PlayMan.idle)
            {
                TransitionToState(PlayerState.PLAY_IDLE);
            }
        }
        public override void OnExit()
        {
            //do stuff
        }
    }
/*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
