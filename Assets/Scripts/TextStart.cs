using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TextStart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¸Ô¾î¿ë");
            PlayerMove playerMove = collision.GetComponent<PlayerMove>();
            playerMove.UnlockSkill();
            Destroy(this.gameObject);
        }
    }
}
