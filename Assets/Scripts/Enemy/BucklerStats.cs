using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BucklerStats : EnemyStats
{
    protected override void Awake()
    {
        maxHp = 9f;
        curHp = maxHp;
        damage = 2f;
    }

    public override void TakeDamage(float damage, Transform player)
    {
        if (isDie) return;
        Debug.Log("Enemy hit" + damage); ;
        if (this.gameObject.transform.rotation.y == 0)//적이 왼쪽을 보고 있을 때
        {
            if (player.transform.position.x > gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                curHp -= damage + 1;
            }
        }
        else //적이 오른쪽을 보고있을 때 
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                curHp -= damage + 1;//백어택
            }
        }

        Vector2 targetPos = new Vector2(player.transform.position.x, transform.position.y);

        // 넉백 벡터 계산 (targetPos에서 현재 위치를 빼면 벡터가 나옴)
        Vector2 knockbackDirection = ((Vector2)transform.position - targetPos).normalized;
        Debug.Log((Vector3)knockbackDirection * knockbackForce);
        // 넉백을 적용한 새로운 위치로 이동
        transform.position += (Vector3)knockbackDirection * knockbackForce;


        CheckHp();
    }
}
