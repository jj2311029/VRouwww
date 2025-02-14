using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantshipEnemy : BasicEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f;
    protected override void Attack()
    {
        InvokeRepeating("Shoot", 0f, fireInterval);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector3 playerPosition = FindPlayerPosition();

        // Y축 방향으로 약간 아래로 내리기
        playerPosition.y -= 0.5f; // 값 조정 가능

        Vector3 fireDirection = (playerPosition - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        ShipBullet bulletScript = bullet.GetComponent<ShipBullet>();

        bulletScript.SetDirection(fireDirection);
        StartCoroutine(PlayAnimation());
        SoundManager.Instance.PlaySFX(13);
    }

    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }
}
