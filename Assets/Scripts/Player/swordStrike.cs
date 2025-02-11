using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordStrike : MonoBehaviour
{
    PlayerMove pm;
    float damage = 2f;
    
    private void Start()
    {
        pm=FindObjectOfType<PlayerMove>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyMove EA = collision.GetComponent<EnemyMove>();
            if (EA != null)
            {
                if(pm.GetSuccessParrying())
                    EA.TakeDamage((int)damage+1);
                else
                    EA.TakeDamage((int)damage);
            }
            else
            {
                EnemyStats es = collision.gameObject.GetComponentInParent<EnemyStats>();
                if (es != null)
                {
                    if (pm.GetSuccessParrying())
                        es.TakeDamage(damage + 1f, pm.gameObject.transform);
                    else
                    {
                        es.TakeDamage(damage, pm.gameObject.transform);
                    }
                }
                else
                {
                    FixedLeg fixedLeg=collision.gameObject.GetComponent<FixedLeg>();
                    fixedLeg.TakeDamage(1);
                }
            }
        }
        if (collision.tag == "Boss")
        {
            EnemyMove EA = collision.GetComponent<EnemyMove>();
            EA.TakeDamage(2);

            Boss bossScript = collision.GetComponent<Boss>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(2);
            }
            else
            {
                bossScript=collision.gameObject.GetComponentInParent<Boss>();
                if (bossScript != null)
                {
                    bossScript.TakeDamage(2);
                }
                else Debug.Log("NullRefference of bossScript");
            }
        }
    }
}