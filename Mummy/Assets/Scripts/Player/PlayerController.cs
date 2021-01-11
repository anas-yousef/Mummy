using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private BoxCollider2D boxCollider2d;
    [SerializeField] private LayerMask platformsLayerMask;
    private float horizontalMove = 0f;
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    
    void Start()
    {
        speed = 5f;
        isGrounded = true;
        isJumping = false;
        isFalling = false;
    }


    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        isGrounded = CheckIsGrounded();

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newScale = transform.localScale;
            if (newScale.x > 0)
            {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newScale = transform.localScale;
            if (newScale.x < 0)
            {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z);
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rigidbody2d.AddForce(transform.up * 100, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
            isJumping = true;
            isFalling = false;
        }

        if (rigidbody2d.velocity.y < -0.1)
        {
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

    private bool CheckIsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2d.collider != null;
    }

}
