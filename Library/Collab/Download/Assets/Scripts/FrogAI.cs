using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAI : MonoBehaviour
{
    [SerializeField] private float speed =5;
    [SerializeField] private float moveDxn =-1;
    [SerializeField] private float PEDistance;
    [SerializeField] private float actualDistance;
    [SerializeField]private bool isPlayerAtLeft = true;
    [SerializeField] private Transform groundCP;
    [SerializeField] private Transform wallCP;
    [SerializeField] private LayerMask ground;
    [SerializeField] private bool checkGr;
    [SerializeField] private bool checkWl;

    [Header("Ground check and Jumping")] 
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private Transform player;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private bool isGrounded = true;

    [Header("Other")]
    private Rigidbody2D _rb;

    [Header("Adding animation")] 
    private Animator frogAnimation;

    [Header("Finite animation State Machine")] 
    [SerializeField] private int stateNum;
    private enum State {Idle, jump, fall, Die};
    private State _state;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        frogAnimation = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        checkGr = Physics2D.OverlapCircle(groundCP.position, .20f, ground);
        checkWl = Physics2D.OverlapCircle(wallCP.position, .20f, ground);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0,ground);
        Range();
        GetState();
        Animate();

    }

    private void Range()
    {
        PEDistance = player.transform.position.x -transform.position.x;
        actualDistance = PEDistance;
        if (PEDistance < 0)
        {
            PEDistance *= -1;
            isPlayerAtLeft = true;
        }
        else
        {
            PEDistance *= 1;
            isPlayerAtLeft = false;
        }
        if (PEDistance > 5 && isGrounded)
        {
            Petrolling();
        }
        else
        {
            JumpAttack();
        }
        
        
    }

    private void Petrolling()
    {
        if(!checkGr || checkWl)
        {
            turn();
        }
        _rb.velocity = new Vector2(moveDxn*speed, _rb.velocity.y); 
    }

    private void turn()
    {
        _rb.transform.Rotate(0, 180, 0);
        moveDxn *= -1;
    }

    private void JumpAttack()
    {
        if (isPlayerAtLeft == true && moveDxn == 1)
        {
            turn();
        }
        else if (isPlayerAtLeft == false && moveDxn == -1)
        {
            turn();
        }
        
        if (isGrounded)
        {
            _rb.velocity = new Vector2(actualDistance*5, 50);
            //_rb.AddForce(new Vector2(PEDistance,10),ForceMode2D.Impulse);
        }
    }

    private void GetState()
    {
        if (isGrounded)
        {
            _state = State.Idle;
        }
        else if (_rb.velocity.y > 0)
        {
            _state = State.jump;
        }
        else if (_rb.velocity.y <0)
        {
            _state = State.fall;
        }
        if (_rb.transform.position.y < -12f)
        {
            _state = State.Die;
        }
    }


    private void Animate()
    {
        if (_state != State.Die)
        {
            stateNum = (int) _state;
            frogAnimation.SetInteger("State", stateNum);
        }
        else
        {
            frogAnimation.SetTrigger("Death");
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCP.position, 0.20f);
        Gizmos.DrawWireSphere(wallCP.position,0.20f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheck.position,boxSize);
    }
}