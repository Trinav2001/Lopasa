using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // for Start function
    private Rigidbody2D _rd;
    private Animator _rAnimation;
    private Collider2D _collider2D;
    private double cherries = (double)PlayerPrefs.GetInt("Cherry"); 
    private Text numberofCherries ;
    [SerializeField]private int kills = 0;
    private int prevKill = PlayerPrefs.GetInt("Kill");
    private int HurtForce = 10;
    private double trial ;
    [SerializeField] private Animator enemy;
    [SerializeField] public int SceneName  = 2;
    
    // Finite State Machine of the game
    private enum State { Idle, Running, Jumping, Falling, Hurt, Die };//creating FSM
    private State _state= State.Idle;

    // movement variables
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float power = 9f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Vector3 dummyPosition;

    [SerializeField] public int OldScene;
    [SerializeField] public int CurrentScene;
    private Transform player;
    private void Start()
    {
        _rd = GetComponent<Rigidbody2D>();
        _rAnimation = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        player = GetComponent<Transform>();
        PlayerPrefs.SetInt( "PrevScene",PlayerPrefs.GetInt("Scene"));
        OldScene = PlayerPrefs.GetInt("PrevScene");
    }
    
    private void Update()
    {
        PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
        CurrentScene = PlayerPrefs.GetInt("Scene");
        dummyPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
        Movement();
        GetState();
        Animate();
        if (_state == State.Die)
        {
            Destroy(this); // We to link a new main screen over here 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectables")
        {
            Destroy(collision.gameObject);
            cherries++;
            PlayerPrefs.SetInt("Cherry",(int)cherries);
            numberofCherries.text = " " + cherries;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll) //killing     an      enemy
    {
        prevKill = kills;
        FrogAI frog = coll.gameObject.GetComponent<FrogAI>();
        if (coll.gameObject.tag == "enemy")
        {
            
            if (_state == State.Falling)
            {
                frog.Death();
                
                kills++;
                PlayerPrefs.SetInt("Kill",kills);
            }
            else
            {
                if (coll.gameObject.transform.position.x < transform.position.x)
                {
                    _rd.velocity = new Vector2(HurtForce, _rd.velocity.y);
                    _state = State.Hurt;
                }
                else
                {
                    _rd.velocity = new Vector2(-HurtForce, _rd.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        float hDxN= Input.GetAxis("Horizontal");
        if (hDxN < 0 )
        {
            _rd.transform.localScale= new Vector3(-1,1,1);
            _rd.velocity = new Vector2(-3*speed - (int)cherries , _rd.velocity.y);
        } // moving to the left
        else if ( hDxN > 0 )
        {
            _rd.transform.localScale= new Vector3(1,1,1);
            _rd.velocity = new Vector2(3*speed + (int)cherries, _rd.velocity.y);
        } // moving to the right
        bool isJumping = (Input.GetButtonDown("Jump") && _collider2D.IsTouchingLayers(ground));
        if(isJumping)
        {
            _rd.velocity = new Vector2(_rd.velocity.x, (power+1) * 5f);
            
        }

        float vDxn = Input.GetAxis("Vertical");
        if (vDxn < 0)
        {
            _rd.velocity = new Vector2(_rd.velocity.x, _rd.velocity.y-1*((int)cherries));
        }
        
        // this section states about jumping when we are going to create the unlocks and abilities then we are going to change this code section
    } // determines the movement of the character
    private void Animate()
    {
        if (_state == State.Die)
        {
            _rAnimation.speed = 1;
        }
        else
        {
            _rAnimation.speed = 1 ;// we have to increase the speed of animation here
        }
        _rAnimation.SetInteger("State",(int)_state);
    } // to animate 
    private void GetState(){
        if (!_collider2D.IsTouchingLayers(ground))
        {
            if (_rd.velocity.y > 0)
            {
                _state = State.Jumping;
            }
            else if (_rd.transform.position.y < -12f)
            {
                _state = State.Die;
            }
            else
            {
                _state = State.Falling;
            }
        }

        else if (Mathf.Abs(_rd.velocity.x) > 1f && _collider2D.IsTouchingLayers(ground))
        {
            _state = State.Running;
        }
        else
        {
            _state = State.Idle;
        }
    } // to get the state of the character
    
    private void Dead()
    {
        Destroy(this.gameObject);
    }

}

