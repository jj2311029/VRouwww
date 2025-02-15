using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [SerializeField] private Transform player; // 플레이어의 위치
    [SerializeField] private float zoomSize = 15f; // 기본 줌 크기
    [SerializeField] private float followStrength = 0.1f; // 플레이어를 따라가는 강도 (낮을수록 덜 움직임)
    [SerializeField] private Vector2 moveLimit = new Vector2(2f, 1f); // 카메라가 움직일 수 있는 최대 한도 (X, Y)

    private Vector3 initialPosition; // 카메라의 초기 위치

    void Start()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(9);
        SoundManager.Instance.PlaySFX(17);
        Camera.main.orthographicSize = zoomSize; // 기본 줌 설정
        initialPosition = transform.position; // 초기 위치 저장
        Invoke("KrakenCry", 7f);
    }
    void KrakenCry()
    {
        SoundManager.Instance.PlaySFX(18);
        Invoke("KrakenCry", 120f);
    }
    void LateUpdate()
    {
        if (player == null) return;

        // 플레이어와의 거리 계산 (X, Y축만 반영)
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

        // 초기 위치와의 거리 제한 적용
        float limitedX = Mathf.Clamp(targetPosition.x, initialPosition.x - moveLimit.x, initialPosition.x + moveLimit.x);
        float limitedY = Mathf.Clamp(targetPosition.y, initialPosition.y - moveLimit.y, initialPosition.y + moveLimit.y);

        // 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(limitedX, limitedY, transform.position.z), followStrength);

        transform.position = smoothedPosition; // 최종 위치 적용
    }
}