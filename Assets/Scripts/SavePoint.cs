using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private int savePos;

    public static int diePoint; // 현재 저장된 체크포인트 위치
    private bool usedSave = false; // 개별 저장 포인트가 사용되었는지 체크

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 태그 비교 방식 유지
        {
            if (!usedSave)
            {
                SaveLoad.savePointIndex = savePos;
                diePoint = savePos; // 현재 체크포인트 업데이트
                usedSave = true; // 중복 저장 방지

                // 플레이어 체력 회복
                PlayerHP hp = collision.GetComponent<PlayerHP>();
                if (hp != null)
                {
                    hp.currentHP = hp.maxHP;
                }
                else
                {
                    Debug.LogError("PlayerHP 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }
    }
}
