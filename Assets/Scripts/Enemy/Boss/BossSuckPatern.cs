using System.Collections;
using UnityEngine;

public class BossSuckPatern : MonoBehaviour
{
    [SerializeField] public float pullForce = 25f; // 빨아들이는 힘
    [SerializeField] public float suckTime = 4.5f;
    [SerializeField] CircleCollider2D Range;
    [SerializeField] CircleCollider2D DieArea;
    bool isSuck = false;
    private void Start()
    {
        Range.enabled=false;
        DieArea.enabled=false;
    }
    public void SuckAttack()
    {
        StartCoroutine(Suck());
    }

    IEnumerator Suck()
    {
        isSuck = true;
        Range.enabled = true;
        DieArea.enabled = true;
        yield return new WaitForSeconds(suckTime);
        Range.enabled = false;
        DieArea.enabled=false;
        isSuck = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
        if (playerRb != null&&isSuck)
        {
            // 공격 중심과 플레이어 위치
            Vector2 direction = (transform.position - collision.transform.position).normalized;

            // 힘 적용
            playerRb.velocity += (direction * pullForce * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")&&isSuck)
        {
            PlayerHP playerScript = collision.gameObject.GetComponent<PlayerHP>();
            if (playerScript != null )
            {
                playerScript.TakeDamage(100, transform.position); // float을 int로 변환
                Debug.Log("Suck");
            }
        }
    }

}