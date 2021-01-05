using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    private float horizontalMove = 0f;
    void Start()
    {
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }
}
