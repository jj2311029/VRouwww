using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantshipEnemy : BasicEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f; // �߻� ����
    [SerializeField] private float bulletSpeed = 20f;
    protected override void Attack()
    {
        InvokeRepeating("Shoot", 0f, fireInterval);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // �Ѿ� ����
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // ������ �Ѿ��� ShipBullet ��ũ��Ʈ�� ������
        ShipBullet bulletScript = bullet.GetComponent<ShipBullet>();

        if (bulletScript != null)
        {
            // �÷��̾� ���� ���
            Vector3 playerPosition = FindPlayerPosition();
            Vector3 fireDirection = (playerPosition - firePoint.position).normalized;

            // �Ѿ� ���� ����
            bulletScript.SetDirection(fireDirection);
        }
    }


    // �÷��̾� ��ġ�� ã�� ���� �Լ�
    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }
}
