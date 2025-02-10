using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] int savePos = 0;

    private bool usedSave = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!usedSave)
            {
                SaveLoad.savePointIndex = savePos;
                usedSave = true;
                Debug.Log(SaveLoad.savePointIndex);
            }
        }
    }
}
