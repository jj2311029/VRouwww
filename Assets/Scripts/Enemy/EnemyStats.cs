using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStats : MonoBehaviour
{
    public float curHp;
    public float maxHp=3f;
    public float damage=1f;
    public float knockbackForce = 0.3f;
    public Rigidbody2D rigid;
    protected bool isDie=false;


    public EnemyWeapon weapon;
    virtual protected void Awake()
    {
        maxHp = 3f;
        curHp = maxHp;
        damage = 1f;
        rigid = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        weapon.TriggerOffCollider();
    }
    public void TriggerOnCollider()
    {
        weapon.TriggerOnCollider();
    }
    public void TriggerOffCollider()
    {
        weapon.TriggerOffCollider();
    }
    virtual public void TakeDamage(float damage,Transform player)
    {
        if (isDie) return;
        
        
        if (this.gameObject.transform.rotation.y == 0)//적이 왼쪽을 보고 있을 때
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                Debug.Log("Enemy hit" + damage);
                curHp -= damage;
            }
            else
            {
                Debug.Log("BackAttack!! Enemy hit" + (damage+1));
                curHp -= damage + 1;
            }
        }
        else //적이 오른쪽을 보고있을 때 
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                curHp -= damage + 1;//백어택
                Debug.Log("BackAttack!! Enemy hit" + (damage + 1));
            }
            else
            {
                Debug.Log("Enemy hit" + damage);
                curHp -= damage ;
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
    
    virtual protected void CheckHp()
    {
        if (curHp <= 0)
        {
            isDie = true;
            gameObject.GetComponent<EnemyBehavior>().StopCor();
            this.StopAllCoroutines();
            Destroy(this.gameObject, 1f);
        }
    }
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHp();
        }
    }
    protected bool IsDie()//use different script
    {
        return isDie;
    }
}
