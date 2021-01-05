using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    private float horizontalMove = 0f;
    
    void Start()
    {
        speed = 5f;
    }


    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newScale = transform.localScale;
            if (newScale.x > 0)
            {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
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
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z);
        }

        
    }
}
