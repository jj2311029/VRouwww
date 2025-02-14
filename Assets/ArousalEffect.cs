using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArousalEffect : MonoBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TriggerArousalEffect()
    {
        animator.SetBool("ArousalEffect", false);
    }
}
