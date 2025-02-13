using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // 이동 속도

    void Update()
    {
        // 구름을 왼쪽으로 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
