using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;  // 플레이어
    private Vector3 offset = new Vector3(0, 2.5f, -10);
    private float smoothSpeed = 0.125f;  // 부드러운 이동 속도
    private float lastInputTime;  // 마지막 입력 시간
    private float resetTime = 2f; // 3초 후 원래 크기로 축소
    private Camera cam;
    private float defaultZoom = 10f; // 기본 카메라 크기
    private float zoomedSize = 8f; // 확대된 카메라 크기
    private float zoomSpeed = 2f; // 줌 변환 속도

    // 흔들림 변수 추가
    public float shakeDuration = 0.5f; // 흔들리는 시간
    public float shakeMagnitude = 1f; // 흔들리는 강도
    private bool isShaking = false; // 흔들림 상태 확인

    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = defaultZoom;  // 초기 줌 크기
    }

    void LateUpdate()
    {
        if (isShaking) return; // 흔들리는 동안에는 기본 이동을 막음

        bool isMoving = false;

        if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT])) // 좌측 이동
        {
            lastInputTime = Time.time;
            isMoving = true;
        }
        else if (Input.GetKey(KeySetting.Keys[KeyAction.RIGHT])) // 우측 이동
        {
            lastInputTime = Time.time;
            isMoving = true;
        }

        // 입력 시 줌 확대
        if (isMoving)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomedSize, Time.deltaTime * zoomSpeed);
        }

        // 3초 동안 입력이 없으면 원래 크기로 부드럽게 축소
        if (Time.time - lastInputTime > resetTime)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultZoom, Time.deltaTime * zoomSpeed);
        }

        if (target != null)
        {
            // 기존 오프셋 유지하면서 이동
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    // 스킬 사용 시 화면 흔들기 함수 (기존 오프셋 유지)
    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        Vector3 basePosition = target.position + offset; // 흔들림 기준 위치를 오프셋 포함한 값으로 설정
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = basePosition + new Vector3(x, y, 0f); // 기존 위치에서만 흔들리게 설정

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = basePosition; // 흔들림 종료 후 원래 위치(오프셋 포함)로 복귀
        isShaking = false;
    }
}
