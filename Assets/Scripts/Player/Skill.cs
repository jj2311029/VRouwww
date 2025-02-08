using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private CameraMove cameraMove; // 카메라 흔들기용 변수 추가

    private void Start()
    {
        // 카메라 찾기
        cameraMove = FindObjectOfType<CameraMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyMove enemyMove = collision.GetComponent<EnemyMove>();
                EnemyBehavior enemyBehavior = collision.GetComponentInParent<EnemyBehavior>();

                if (enemyMove != null)
                {
                    enemyMove.Stun(3f);
                }
                else if (enemyBehavior != null)
                {
                    enemyBehavior.Stun(3f);
                }
                else
                {
                    Debug.Log("없어시발");
                }

                // 적을 맞췄을 때 화면 흔들기 실행
                if (cameraMove != null)
                {
                    cameraMove.StartShake();
                }
                else
                {
                    Debug.LogWarning("카메라 못 찾음");
                }
            }
        }
    }
}
