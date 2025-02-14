using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAttackCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHP playerScript = collision.gameObject.GetComponent<PlayerHP>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1, transform.position);
                Debug.Log("Leg Attack");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHP playerScript = collision.gameObject.GetComponent<PlayerHP>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1, transform.position);
                Debug.Log("Leg Attack");
            }
        }
    }
}
