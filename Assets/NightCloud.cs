using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightCloud : MonoBehaviour
{
    private bool isMoving = false;
    private float moveSpeed = 2f;

    private void Update()
    {
        if (isMoving)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 들어왔는지 확인
        {
            isMoving = true;
        }
    }
}
