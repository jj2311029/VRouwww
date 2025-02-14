using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLeg : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;
    [SerializeField] protected BoxCollider2D boxCollider;
    protected Animator anim; // 애니메이션 추가
    //[SerializeField] private int attackPower; // 공격력 설정
    //[SerializeField] private float pushBackForce = 5f;
    [SerializeField] Boss boss;

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

        //공격 효과음
        SoundManager.Instance.PlaySFX(19);

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
            if (player.transform.position.x < transform.position.x)
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            else
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    // 충돌 처리 (플레이어의 공격)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            TakeDamage(1); // 데미지 1
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&isAttack==false)
        {
            //TakeDamage(1); // 데미지 1
            PlayerHP playerScript = collision.gameObject.GetComponent<PlayerHP>();
            if (playerScript != null )
            {
                playerScript.TakeDamage(1, transform.position); // float을 int로 변환
                Debug.Log("Enemy hit Player  Damage: 1" );
            }
        }
    }

    // 데미지 처리 및 사망
    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;
        Debug.Log("고정다리가 데미지를 받음. 현재 HP: " + Hp);

        // 피격 시 빨간색 효과 추가
        StartCoroutine(InvincibilityEffectCoroutine());

        if (Hp <= 0)
        {
            Debug.Log("고정다리 사망.");
            SoundManager.Instance.PlaySFX(20); // 사망 효과음 재생
            boss.DieLeg(this);
            StartCoroutine(FadeOutAndDestroy()); // 투명도 감소 후 제거
        }
    }

    // 피격 시 빨간색으로 변하고 원래 색으로 돌아오는 코루틴 추가
    private IEnumerator InvincibilityEffectCoroutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null)
            yield break;

        Color originalColor = sr.color;
        Color hitColor = new Color(1f, 0.2f, 0.2f, originalColor.a); // 빨간색 강조

        float elapsedTime = 0f;
        float duration = 0.2f; // 색상 변경 지속 시간

        // 서서히 빨간색으로 변화
        while (elapsedTime < duration)
        {
            sr.color = Color.Lerp(originalColor, hitColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sr.color = hitColor; // 최종 빨간색 적용

        elapsedTime = 0f;

        // 서서히 원래 색으로 복귀
        while (elapsedTime < duration)
        {
            sr.color = Color.Lerp(hitColor, originalColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sr.color = originalColor; // 최종 원래 색 복귀
    }

    // 사망 시 서서히 투명해지면서 삭제되는 코루틴 (기존 코드 유지)
    private IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float fadeDuration = 1.5f;
        float elapsedTime = 0f;
        Color originalColor = sr.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(gameObject);
    }
    public bool GetIsAttack()
    {
        return isAttack;
    }
    public void SetParent(Boss bs)
    {
        boss = bs;
    }
}