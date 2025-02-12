using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float swordAttackSpeed;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float gunAttackSpeed;

    public int bulletNumber = 2;
    public int maxBullet = 2;
    public GameObject bulletUIParent; // 부모 오브젝트 (UI 전체)
    private GameObject[] bulletUI; // 자식 오브젝트 (각 상태 UI)

    private bool canSwordAttack = true;
    private bool canGunAttack = true;
    private float lastBulletChangeTime; // 마지막으로 총알 UI가 변한 시간

    Sword S;
    Gun G;

    void Start()
    {
        bulletUIParent.SetActive(false);

        S = GetComponentInChildren<Sword>();
        G = GetComponentInChildren<Gun>();

        // 부모 오브젝트의 자식들을 배열로 가져오기
        int childCount = bulletUIParent.transform.childCount;
        bulletUI = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            bulletUI[i] = bulletUIParent.transform.GetChild(i).gameObject;
        }

        UpdateBulletUI(); // 초기 UI 설정
    }

    void Update()
    {
        if (Input.GetKey(KeySetting.Keys[KeyAction.SWORD]) && canSwordAttack)
        {
            StartCoroutine(SwordAttack());
            S.doSwordAttack();
        }

        if (Input.GetKey(KeySetting.Keys[KeyAction.GUN]) && canGunAttack && (bulletNumber > 0))
        {
            StartCoroutine(GunAttack());
            StartCoroutine(Reload());
            GetComponent<PlayerMove>().anim.SetTrigger("isAttack_Gun");
            G.doGunAttack();
        }

        // UI가 2초 이상 변화가 없으면 즉시 사라짐
        if (Time.time - lastBulletChangeTime >= 1f && bulletUIParent.activeSelf)
        {
            bulletUIParent.SetActive(false);
        }
    }

    private IEnumerator SwordAttack()
    {
        canSwordAttack = false;
        yield return new WaitForSeconds(swordAttackSpeed);
        canSwordAttack = true;
    }

    private IEnumerator GunAttack()
    {
        canGunAttack = false;
        bulletNumber -= 1;
        Debug.Log("발싸!");

        UpdateBulletUI(); // UI 갱신

        yield return new WaitForSeconds(gunAttackSpeed);
        canGunAttack = true;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadSpeed);
        bulletNumber += 1;
        Debug.Log($"1발 장전 {bulletNumber}");

        UpdateBulletUI(); // UI 갱신
    }

    private void UpdateBulletUI()
    {
        bulletUIParent.SetActive(true); // 즉시 UI 표시
        lastBulletChangeTime = Time.time; // 마지막 변화 시간 업데이트

        // 모든 UI 비활성화 후, 해당하는 UI만 활성화
        for (int i = 0; i < bulletUI.Length; i++)
        {
            bulletUI[i].SetActive(i == bulletNumber);
        }

        // 현재 활성화된 UI 흔들기
        StartCoroutine(ShakeUI());
    }

    private IEnumerator ShakeUI()
    {
        if (bulletNumber < 0 || bulletNumber >= bulletUI.Length) yield break;
        RectTransform uiTransform = bulletUI[bulletNumber].GetComponent<RectTransform>();

        if (uiTransform == null) yield break;

        Vector2 originalPos = uiTransform.anchoredPosition; // 원래 위치 저장
        float shakeAmount = 5f; // 흔들림 크기
        int shakeCount = 6; // 흔들리는 횟수 (짝수로 설정)
        float shakeSpeed = 0.05f; // 한 번 움직일 때 걸리는 시간

        for (int i = 0; i < shakeCount; i++)
        {
            float offset = (i % 2 == 0) ? shakeAmount : -shakeAmount; // 좌우로 번갈아가면서 흔들림
            uiTransform.anchoredPosition = new Vector2(originalPos.x + offset, originalPos.y);
            yield return new WaitForSeconds(shakeSpeed);
        }

        uiTransform.anchoredPosition = originalPos; // 원래 위치로 복귀
    }
}