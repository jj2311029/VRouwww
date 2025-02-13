using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Object
{
    [SerializeField] PirateManager pirateManager;

    protected override void Awake()
    {
        base.Awake();
        speed = 2f;
        pirateManager = GameObject.Find("GameManager").GetComponent<PirateManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            SoundManager.Instance.PlaySFX(14);
            pirateManager.UpHeart();
        }
    }
}
