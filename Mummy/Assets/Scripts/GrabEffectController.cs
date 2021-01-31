using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabEffectController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void StartAnimation()
    {
        animator.Play("Grab Effect", -1, 0f);
    }
}
