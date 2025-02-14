using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFallDownPatern : MonoBehaviour
{
    public GameObject leftLegCollider;
    public GameObject rightLegCollider;
    public Animator anim;
    private void Start()
    {
        leftLegCollider.SetActive(false);
        rightLegCollider.SetActive(false);
    }
    public void TriggerActiveCollider()
    {
        leftLegCollider.SetActive(true);
        rightLegCollider.SetActive(true);
    }
    public void TriggerDestroy()
    {
        anim.SetBool("IsFalldown", false);
        leftLegCollider.SetActive(false);
        rightLegCollider.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerHP PH = collision.GetComponent<PlayerHP>();
            PH.TakeDamage(1, transform.position);
        }
    }
    public void Attack()
    {
        anim.SetBool("IsFalldown", true);

        //È¿°úÀ½
        SoundManager.Instance.PlaySFX(20);

    }

}
