using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public static string lastSceneName = "";
    public GameObject heartPrefab;  // 하트 프리팹
    public GameObject diePanel; // 사망 패널
    public int maxHP = 30;  // 최대 체력
    public int currentHP = 30;  // 현재 체력
    public List<GameObject> heartObjects = new List<GameObject>();  // 하트 GameObject 리스트

    private bool isInvincible = false;  // 무적 상태 여부
    public float invincibilityDuration = 1f;  // 무적 상태 지속 시간
    public Rigidbody2D rb;
    public PlayerMove pm;

    void Start()
    {
        CreateHearts();  // 게임 시작 시 하트 객체 생성
        UpdateHearts();  // 초기 하트 이미지 업데이트
        rb=gameObject.GetComponent<Rigidbody2D>();
        if (pm==null) pm=gameObject.GetComponent<PlayerMove>();
    }

    // 하트 프리팹을 이용해 하트 객체 생성
    void CreateHearts()
    {
        for (int i = 0; i < maxHP; i++)
        {
            //GameObject heart = Instantiate(heartPrefab, transform); // 프리팹을 인스턴스화하여 부모로 설정
            //heartObjects.Add(heart);  // 하트 객체 리스트에 추가
        }
    }

    // 체력 감소 처리
    public void TakeDamage(int damage, Vector2 targetpos)
    {
        Debug.Log("맞음");
        // 무적 상태일 경우 데미지 무효화
        if (isInvincible) return;
        if (pm.GetParrying())
        {
            StartCoroutine(pm.ParryingSuccess());
            return;
        }
        return;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        // 넉백 방향 계산 (목표와 현재 위치의 차이)
        Vector2 knockbackDirection = ((Vector2)transform.position - targetpos).normalized;

        // 넉백 강도 설정 (값을 조정하여 넉백의 강도를 결정)
        float knockbackStrength = 7f;

        // 현재 Rigidbody2D의 velocity에 반대 방향으로 넉백 적용
        rb.velocity += knockbackDirection * knockbackStrength;


        // 하트 업데이트
        UpdateHearts();

        // 무적 상태 시작
        StartCoroutine(InvincibilityCoroutine());
    }

    // 하트 상태 업데이트
    public void UpdateHearts()
    {
        for (int i = 0; i < heartObjects.Count; i++)
        {
            // 현재 체력에 맞는 하트를 활성화 또는 비활성화
            heartObjects[i].SetActive(i < currentHP);
        }
        if (currentHP <= 1)
        {
            SoundManager.Instance.PlaySFX(1);
        }
    }

    // 플레이어 사망 처리
    public void Die()
    {
        Debug.Log("플레이어 사망");
        lastSceneName = SceneManager.GetActiveScene().name;
        if (diePanel != null)
        {
            DiePanel panelScript = diePanel.GetComponent<DiePanel>();
            if (panelScript != null)
            {
                panelScript.Bravo6(); // 애니메이션 실행
            }
            else
            {
                Debug.LogWarning("diePanel에 DiePanel 스크립트가 없음.");
            }
        }
        else
        {
            Debug.LogWarning("diePanel이 할당되지 않았음.");
        }

        Invoke("LoadGameOverScene", 0.5f);
    }

    // 게임 오버 씬 로드
    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("Gameover");
    }

    // 무적 상태 코루틴 (서서히 빨개졌다가 원래 색으로 복귀)
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color; // 원래 색상 저장
            Color targetColor = new Color(1f, 0.2f, 0.2f, originalColor.a); // 빨간색 (R값 강조)

            float elapsedTime = 0f;
            float duration = invincibilityDuration / 3f; // 색 변환 속도 조절

            // 서서히 빨간색으로 변화
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = targetColor; // 최종 빨간색 적용

            elapsedTime = 0f;

            // 서서히 원래 색으로 복귀
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(targetColor, originalColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = originalColor; // 최종 원래 색상 복귀
        }

        yield return new WaitForSeconds(invincibilityDuration / 3f); // 남은 무적 시간 유지
        isInvincible = false;
    }
}