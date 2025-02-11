using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLeg : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected Transform foot;
    protected Animator anim; // 애니메이션 추가
    //[SerializeField] private int attackPower; // 공격력 설정
    //[SerializeField] private float pushBackForce = 5f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0f; // 현재 쿨타임 상태
    [SerializeField] private float attackDelay = 1.5f; // 공격 딜레이
    bool isAttack=false;

    [SerializeField] protected float attackRange = 5f;    //공격 준비 거리

    [Header("Hp")]
    [SerializeField] protected int Hp = 3;
    //[SerializeField] protected float knockbackForce = 10f;   // 넉백 힘

    [Header("About Player")]
    [SerializeField] protected Transform player;//플레이어 위치
    [SerializeField] protected Renderer playerRenderer;
    protected bool isPlayerOnSamePlatform=false;
    

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>(); // BoxCollider2D 컴포넌트 가져오기
        player = GameObject.FindWithTag("Player").transform;
        if(player) playerRenderer = player.GetComponent<Renderer>();
        anim = GetComponent<Animator>(); // Animator 가져오기
    }

    protected void FixedUpdate()
    {
        attackCooldown += Time.deltaTime;
        CheckPlatform();
        if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldown > attackDelay&&isAttack==false) 
        {
            for (int i = 0;i<10;i++)
                Debug.Log("Attack");
            Attack();
            attackCooldown = 0f;
            isAttack =true;
        }
        Turn();
    }
    protected void Attack()
    {
        anim.SetBool("IsAttack",true);
        anim.SetBool("IsIdle", false);
        Debug.Log("Attack");
    }
    public void TriggerAttackFinish()
    {
        anim.SetBool("IsAttack", false);
        anim.SetBool("IsIdle", true);
        isAttack = false;
        attackCooldown = 0f;
        Debug.Log("FinshAttack");
    }
    protected void Turn()
    {
        if (player != null)
        {
            if (player.transform.position.x < transform.position.x) render.flipX = true;
            else render.flipX = false;
        }
    }

    protected void CheckPlatform()
    {
        isPlayerOnSamePlatform = Mathf.Abs(player.position.y - foot.position.y) < 0.5f;
    }

    // 충돌 처리 (플레이어의 공격)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            TakeDamage(1); // 데미지 1
        }
    }

    // 데미지 처리 및 사망
    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;

        Debug.Log("적이 데미지를 받음. 현재 HP: " + Hp);

        // 넉백 방향 계산
        Vector2 knockbackDirection = (transform.position - player.position).normalized;

        // 넉백 적용
        rigid.velocity = Vector2.zero; // 현재 속도 초기화
        //rigid.AddForce(new Vector2(knockbackDirection.x * knockbackForce, rigid.velocity.y), ForceMode2D.Impulse);

        if (Hp <= 0)
        {
            Debug.Log("적이 사망했습니다.");
            Destroy(this.gameObject);
        }
    }

    //이동 애니메이션 관리
    protected virtual void StartMoving()
    {
        if (anim != null)
            anim.SetBool("isMoving", true);
    }

    protected virtual void StopMoving()
    {
        if (anim != null)
            anim.SetBool("isMoving", false);
    }

    


}