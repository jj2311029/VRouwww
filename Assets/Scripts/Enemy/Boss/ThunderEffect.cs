using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffect : MonoBehaviour
{
   Animator animator;
    private void Start()
    {
         if (animator == null)
            animator = GetComponent<Animator>();
    }
    public void TriggerThunderOn()
    {
        animator.SetBool("IsThunder",true);
    }
    public void TriggerThunderOff()
    {
        animator.SetBool("IsThunder",false);
    }
}
