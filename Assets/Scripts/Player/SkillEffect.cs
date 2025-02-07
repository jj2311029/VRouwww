using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlaySkillEffect()
    {
        Debug.LogError("SkillEffect 컴포넌트를 찾을!");
        if (anim != null )
        {
            Debug.Log("tlqkf 있는데");
        }
        anim.SetTrigger("doSkill");
    }
}
