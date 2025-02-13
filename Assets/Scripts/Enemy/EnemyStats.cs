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
    virtual protected void Awake()
    {
        maxHp = 3f;
        curHp = maxHp;
        damage = 1f;
        rigid = GetComponent<Rigidbody2D>();
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

            StartCoroutine(FadeOutAndDestroy()); // 추가: 서서히 사라지도록 함

            Destroy(this.gameObject, 0.5f); // 기존 코드 유지
        }
    }
    private IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 0.5f; // 투명화 속도 (기존 삭제 타이밍과 동일하게 설정)
        float elapsedTime = 0f;
        SpriteRenderer render = GetComponent<SpriteRenderer>();

        if (render == null) yield break; // 예외 처리: 렌더러가 없으면 중단

        Color originalColor = render.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            render.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
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
