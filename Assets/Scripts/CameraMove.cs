using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;  // 플레이어
    public Vector3 offset = new Vector3(0, 3f, -10); // 기본 위치
    public Vector3 downOffset = new Vector3(0, 3f, -10); // 아래 방향 이동 시 위치

    public float smoothSpeed = 0.125f;  // 부드러운 이동 속도

    public float normalZoom = 8f;  // 기본 줌
    public float idleZoom = 10f;   // 플레이어가 가만히 있을 때 줌
    public float downZoom = 12f;   // 아래 방향키를 눌렀을 때 줌인 값
    public float zoomSpeed = 2f;   // 줌 변화 속도
    public float idleDelay = 2f;   // 정지 상태 후 줌 아웃까지 걸리는 시간

    private Camera cam;
    private float targetZoom;
    private bool isMoving = false; // 플레이어가 이동 중인지 판별
    private float idleTimer = 0f;  // 멈춘 상태에서 경과 시간

    // 화면 흔들기 관련 변수 추가
    private bool isShaking = false;
    private float shakeDuration = 0.5f;  // 흔들리는 지속 시간
    private float shakeMagnitude = 0.2f; // 흔들리는 강도
    private Vector3 originalPosition;

    // 아래 방향키 감지 관련 변수
    private float downKeyHoldTime = 0f;
    private float holdDuration = 1f; // 1초 이상 눌러야 효과 적용
    private bool isLookingDown = false;

    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = normalZoom;  // 초기 줌 설정
        targetZoom = normalZoom;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            if (!isShaking) // 흔들리는 동안에는 카메라 움직임 방해받지 않도록
            {
                // 아래 방향키 감지 및 적용
                if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]))
                {
                    downKeyHoldTime += Time.deltaTime;
                    if (downKeyHoldTime >= holdDuration)
                    {
                        isLookingDown = true;
                    }
                }
                else
                {
                    downKeyHoldTime = 0f;
                    isLookingDown = false;
                }

                // 플레이어 이동 여부 판별
                if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT]) || Input.GetKey(KeySetting.Keys[KeyAction.RIGHT]))
                {
                    isMoving = true;
                    idleTimer = 0f; // 이동하면 타이머 초기화
                }
                else
                {
                    if (!isMoving)
                    {
                        idleTimer += Time.deltaTime; // 멈춘 상태에서 시간 누적
                    }
                    isMoving = false;
                }

                // 카메라 줌 설정
                if (isLookingDown)
                {
                    targetZoom = downZoom; // 아래 방향키 누르면 줌인
                }
                else
                {
                    targetZoom = (idleTimer >= idleDelay) ? idleZoom : normalZoom;
                }

                // 줌을 부드럽게 변경
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

                // 카메라 위치 설정
                Vector3 desiredPosition = target.position + (isLookingDown ? downOffset : offset);
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
    }

    // 화면 흔들기 기능 추가
    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        originalPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
            float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // 원래 위치로 복귀
        isShaking = false;
    }
}
