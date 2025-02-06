using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyMove enemyMove = collision.GetComponent<EnemyMove>();
                EnemyBehavior enemyBehavior = collision.GetComponentInParent<EnemyBehavior>();

                if (enemyMove != null)
                {
                    enemyMove.Stun(3f);
                }
                else if (enemyBehavior != null)
                {
                    enemyBehavior.Stun(3f);
                }
                else
                {
                    Debug.Log($"적 {collision.gameObject.name}에는 EnemyMove 또는 EnemyBehavior가 없습니다.");
                }
            }
        }
    }
}
