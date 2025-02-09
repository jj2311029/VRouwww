using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interval : MonoBehaviour
{
    [SerializeField] private GameObject Size;

    public int numOfEnemies = 0;
    public bool isPlayerInterval = false;
    public bool stageClear = false;
    public bool doorOpen = false;
    public GameObject[] door;
    private float doorMoveTime = 0.3f;
    private float fadeDuration = 1f;

    void Start()
    {
        DetectEnemies();

        SpriteRenderer spr = Size.GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            Color color = spr.color;
            color.a = 0f;
            spr.color = color;
        }

        foreach (GameObject d in door)
        {
            if (d != null)
            {
                d.transform.position -= new Vector3(0, 20f, 0); // 처음에 문을 아래로 이동
                d.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (!isPlayerInterval) return;

        DetectEnemies();

        if (!doorOpen)
        {
            doorOpen = true;
            foreach (GameObject d in door)
            {
                if (d != null)
                {
                    d.SetActive(true);
                    StartCoroutine(MoveDoorSmoothly(d, 20f));
                }
            }
        }

        if (numOfEnemies <= 0)
            stageClear = true;

        if (stageClear)
        {
            foreach (GameObject d in door)
            {
                if (d != null)
                {
                    StartCoroutine(FadeOutAndDestroy(d));
                }
            }
        }
    }

    void DetectEnemies()
    {
        int previousEnemyCount = numOfEnemies;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(Size.transform.position, Size.transform.localScale, 0f);
        int currentEnemy = 0;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                currentEnemy++;
            }
        }
        numOfEnemies = currentEnemy;

        // 적 숫자가 변했을 때만 로그 출력 (디버깅 편의성)
        if (previousEnemyCount != numOfEnemies)
        {
            Debug.Log($"현재 남은 적: {numOfEnemies}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInterval = true;
        }
    }

    // 문을 서서히 올리는 코루틴
    IEnumerator MoveDoorSmoothly(GameObject door, float moveAmount)
    {
        if (door == null) yield break;

        Vector3 startPosition = door.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, moveAmount, 0);
        float elapsedTime = 0f;

        while (elapsedTime < doorMoveTime)
        {
            if (door == null) yield break; // 문이 삭제되면 중단

            door.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / doorMoveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (door != null)
        {
            door.transform.position = targetPosition; // 최종 위치 보정
        }
    }

    IEnumerator FadeOutAndDestroy(GameObject door)
    {
        if (door == null) yield break;

        // 부모 오브젝트의 SpriteRenderer 가져오기
        SpriteRenderer parentSR = door.GetComponent<SpriteRenderer>();

        // 자식 오브젝트들의 모든 SpriteRenderer 가져오기
        SpriteRenderer[] childSRs = door.GetComponentsInChildren<SpriteRenderer>();

        if (parentSR == null && childSRs.Length == 0) yield break; // 아무것도 없으면 중단

        float elapsedTime = 0f;
        float startAlpha = parentSR != null ? parentSR.color.a : 1f; // 부모 알파값

        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);

            // 부모 페이드 아웃
            if (parentSR != null)
            {
                Color parentColor = parentSR.color;
                parentColor.a = newAlpha;
                parentSR.color = parentColor;
            }

            // 자식들도 페이드 아웃
            foreach (SpriteRenderer sr in childSRs)
            {
                if (sr != null)
                {
                    Color childColor = sr.color;
                    childColor.a = newAlpha;
                    sr.color = childColor;
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 알파값 0으로 확정 후 삭제
        if (door != null)
        {
            if (parentSR != null)
            {
                Color parentColor = parentSR.color;
                parentColor.a = 0f;
                parentSR.color = parentColor;
            }

            foreach (SpriteRenderer sr in childSRs)
            {
                if (sr != null)
                {
                    Color childColor = sr.color;
                    childColor.a = 0f;
                    sr.color = childColor;
                }
            }

            door.SetActive(false); // 자연스럽게 사라지게 하기 위해 비활성화
            Destroy(door, 0.5f); // 0.5초 후 삭제 (바로 사라지는 느낌 방지)
        }
    }
}