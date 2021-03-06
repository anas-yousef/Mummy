﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameManager gm;
    
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private BoxCollider2D boxCollider2d;
    [SerializeField] private LayerMask platformsLayerMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementForce;
    [SerializeField] private Transform m_GroundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck; // A position marking where to check for ceilings
    const float k_GroundedRadius = 0.2f; // Radius of the overlap circle to determine if grounded
    private float horizontalMove = 0f;
    private float horizontalMovePhysics = 0f;
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private bool isMoving;
    private bool canMove;
    private bool pressJump;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    void Start()
    {
        speed = 5f;
        isGrounded = true;
        isJumping = false;
        isFalling = false;
        canMove = true;
    }


    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        horizontalMovePhysics = Input.GetAxisRaw("Horizontal") * speed;
        isGrounded = CheckIsGrounded();

        if (Input.GetKey(KeyCode.LeftArrow) && canMove)
        {
            // Debug.Log(canMove);
            isMoving = true;
            Vector3 newScale = transform.localScale;
            if (newScale.x > 0)
            {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            
            if(isMoving)
            {
               transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z);
            }
            
        }

        if (Input.GetKey(KeyCode.RightArrow) && canMove)
        {
            isMoving = true;
            Vector3 newScale = transform.localScale;
            if (newScale.x < 0)
            {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            if (isMoving)
            {
                transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z);
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            isMoving = false;
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow) && canMove)
        {
            pressJump = true;
            //rigidbody2d.AddForce(transform.up * 100, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
            isJumping = true;
            isFalling = false;
        }

        if (rigidbody2d.velocity.y < -0.1)
        {
            //Debug.Log("Falling");
            isJumping = false;
            isFalling = true;
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }

        if (isGrounded && isFalling)
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
            isFalling = false;
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (pressJump)
        {
            rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            pressJump = false;
        }
        //if (isMoving)
        //{
        //    Vector3 targetVelocity = new Vector2(horizontalMovePhysics * Time.fixedDeltaTime * movementForce, rigidbody2d.velocity.y);
        //    // And then smoothing it out and applying it to the character
        //    rigidbody2d.velocity = Vector3.SmoothDamp(rigidbody2d.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        //}
        if (!isMoving && !isFalling && !isJumping)
        {
            //rigidbody2d.velocity = Vector3.zero;
        }


    }

private bool CheckIsGrounded()
    {
        bool grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, platformsLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                int x = 0;
                grounded = true;
            }
        }
        return grounded;
       

        //RaycastHit2D boxcastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        //RaycastHit2D raycastHit2d = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + 0.1f, platformsLayerMask);
        //Color rayColor;
        //if (raycastHit2d.collider != null)
        //{
        //    rayColor = Color.green;
        //}
        //else
        //{
        //    rayColor = Color.red;
        //}

        //Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + 0.1f), rayColor);

        //return boxcastHit2d.collider != null && raycastHit2d.collider != null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Water"))
        {
            Debug.Log("Hit the water");
            
            // Restart level. 
            gm.RestartLevel();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Exit"))
        {
            gm.SwitchLevel();
        }
    }

    public void StopMovement()
    {
        rigidbody2d.velocity = new Vector2(0, 0);
    }

    public void SetCanMove(bool set)
    {
        canMove = set;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public bool InAir()
    {
        return isJumping || isFalling;
    }

}
