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
                BucklerStats BS = collision.gameObject.GetComponentInParent<BucklerStats>();


                if (BS != null)
                {
                    if (pm.GetSuccessParrying())
                        BS.TakeDamage(damage + 1f, pm.gameObject.transform);
                    else
                    {
                        BS.TakeDamage(damage, pm.gameObject.transform);
                    }

                }
                else
                {
                    EnemyStats ES = collision.gameObject.GetComponentInParent<EnemyStats>();
                    if (pm.GetSuccessParrying())
                        ES.TakeDamage(damage + 1f, pm.gameObject.transform);
                    else
                    {
                        ES.TakeDamage(damage, pm.gameObject.transform);
                    }
                }



            }
        }
    }
}