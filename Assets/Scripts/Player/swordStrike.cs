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
        if (collision.CompareTag("Enemy"))
        {
            EnemyMove ea = collision.GetComponent<EnemyMove>();
            if (ea != null)
            {
                if(pm.GetSuccessParrying())
                    ea.TakeDamage((int)damage+1);
                else
                    ea.TakeDamage((int)damage);
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
            }
        }
        if (collision.CompareTag("Boss"))
        {
            EnemyMove ea = collision.GetComponent<EnemyMove>();
            ea.TakeDamage(2);

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