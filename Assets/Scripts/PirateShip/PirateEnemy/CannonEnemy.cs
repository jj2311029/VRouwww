using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : BasicEnemy
{
    public GameObject cannonField;
    private Vector3 spawnPosition;
    
    protected override void Attack()
    {
        if (Hp <= 0)
        {
            CancelInvoke("Attack");
            return;
        }
        Invoke("CreateAttack", 1f);
        StartCoroutine(PlayAnimation());
        Invoke("Attack", 4f);
    }

    private void CreateAttack()
    {
        GameObject CF = Instantiate(cannonField, transform.position, transform.rotation);
        SoundManager.Instance.PlaySFX(11);
        Destroy(CF, 5f);
    }
}
