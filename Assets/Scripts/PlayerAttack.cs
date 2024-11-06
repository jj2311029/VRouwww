using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{    
    
    [SerializeField] private float swordAttackSpeed;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float gunAttackSpeed;

    private float swordAttackCool = 1f;//1f/swordAttackSpeed;
    
    public int bulletNumber = 2;
    public int maxBullet = 2;

    private bool canSwordAttack = true;
    private bool canGunAttack = true;
    
    //총칼에 명령전달
    Sword S;
    Gun G;
    
    void Start()
    {
        S = GetComponentInChildren<Sword>();
        G = GetComponentInChildren<Gun>();
    }
    
    //zx공격
    void Update()
    {
        if( Input.GetKey(KeyCode.Z) && canSwordAttack)
        {
            StartCoroutine("SwordAttack");
            S.doSwordAttack();
        }
        if ( Input.GetKey(KeyCode.X) && canGunAttack && (bulletNumber > 0))
        {
            StartCoroutine("GunAttack");
            StartCoroutine("Reload");
            G.doGunAttack();
        }
    }

    //총칼 공격속도 & 재장전 속도
    private IEnumerator SwordAttack()
    {
        canSwordAttack = false;
        yield return new WaitForSeconds(swordAttackCool);
        canSwordAttack = true;
    }

    private IEnumerator GunAttack()
    {
        canGunAttack = false;
        bulletNumber -= 1;
        Debug.Log("발싸!");
        yield return new WaitForSeconds(gunAttackSpeed);
        canGunAttack = true;
    }
    
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadSpeed);
        bulletNumber += 1;
        Debug.Log("1발장전");
    }
}
