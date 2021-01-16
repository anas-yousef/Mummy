using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gm;

    [SerializeField] private Vector3 targetTransform;
    [SerializeField] private Vector2 swinging;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private BoxCollider2D boxCollider2d;
    [SerializeField] private LayerMask platformsLayerMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float downForce;
    [SerializeField] private float swingingForce;
    [SerializeField] private float movementForce;
    [SerializeField] private Transform m_GroundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck; // A position marking where to check for ceilings
    public float k_GroundedRadius = 0.4f; // Radius of the overlap circle to determine if grounded
    private float horizontalMove = 0f;
    private float horizontalMovePhysics = 0f;
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private bool isMoving;
    private bool canMove;
    private bool pressJump;
    private bool isSwingnig;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    void Awake()
    {
        speed = 5f;
        isGrounded = true;
        isJumping = false;
        isFalling = false;
        canMove = true;
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }


    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        horizontalMovePhysics = Input.GetAxisRaw("Horizontal");
        CheckIsGrounded();

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

            if (isMoving)
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

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow) && canMove)
        {
            pressJump = true;
            isJumping = true;
            isFalling = false;
            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsFalling", isFalling);
           
        }

        if (rigidbody2d.velocity.y < -0.1)
        {
            isJumping = false;
            isFalling = true;
            animator.SetBool("IsFalling", isFalling);
            animator.SetBool("IsJumping", isJumping);
        }
    }

    public void OnLanding()
    {
        isFalling = false;
        isJumping = false;
        animator.SetBool("IsFalling", isFalling);
        animator.SetBool("IsJumping", isJumping);
    }

    private void FixedUpdate()
    {
        if (pressJump)
        {
            rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            pressJump = false;
        }

        if (isSwingnig)
        {
            Vector3 vectorDirection = targetTransform - transform.position;
            //Vector2 swingingDirection = new Vector2(vectorDirection.y, -vectorDirection.x);
            
            Vector2 swingingDirection = new Vector2(vectorDirection.x, vectorDirection.y) + new Vector2(0, -downForce);
            //swingingDirection += new Vector2(0, -downForce);
            swinging = swingingDirection;
            rigidbody2d.AddRelativeForce(Vector2.right * horizontalMovePhysics * swingingForce, ForceMode2D.Impulse);
            //rigidbody2d.AddForce(swingingDirection * horizontalMovePhysics * swingingForce, ForceMode2D.Impulse);
        }
    }

    private void CheckIsGrounded()
    {
        Color rayColor;
        rayColor = Color.red;
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, platformsLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded || isFalling)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
        //Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + 0.1f), rayColor);
        //return grounded;


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

    public void SetCanMove(bool newMove)
    {
        canMove = newMove;
    }

    public bool CanMove => canMove;

    public void SetIsSwinging(bool newSwing)
    {
        isSwingnig = newSwing;
    }

    public bool GetIsSwinging() => isSwingnig;

    public void SetTargetTransform(Vector3 transform)
    {
        targetTransform = transform;
    }

    public bool InAir()
    {
        return isJumping || isFalling;
    }
}
