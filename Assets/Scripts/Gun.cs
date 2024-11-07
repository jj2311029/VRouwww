using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//총 공격 명령 시행시 작동, 총알 복사본 제작
public class Gun : MonoBehaviour
{
    //public Text Ammo;
    //Ammo.text
    [SerializeField] private float bulletSpeed;

    public GameObject bullet;

    public void doGunAttack()
    {
        Debug.Log("총");

        GameObject cpy_bullet = Instantiate(bullet, new Vector2(bullet.transform.position.x * 10f, bullet.transform.position.y), bullet.transform.rotation);
        Destroy(cpy_bullet, 10f);
    }
}
