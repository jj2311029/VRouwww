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

    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = defaultZoom;  // 초기 줌 크기
    }

    void LateUpdate()
    {
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
            // 대상의 위치에 오프셋 적용
            Vector3 desiredPosition = target.position + offset;
            // 부드럽게 이동
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
