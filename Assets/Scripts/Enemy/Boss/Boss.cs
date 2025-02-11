using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject patern2;
    

    [SerializeField] protected float hp = 100;
    [SerializeField] protected float attackSpeed = 10f;

    protected int page = 1;
    private int attackPatern = 0;
    private bool canAttack = true;
    private int attackStack = 0;
    bool eyeattack = false;

    private float groggyTime = 10f;


    [Header("Fixed Leg")]
    private GameObject fixedLeg1;
    private GameObject fixedLeg2;
    private FixedLeg fLeg1;
    private FixedLeg fLeg2;//--------------------------------------------------------------------------고정다리 스크립트
    [SerializeField] List<Vector2> fixedLegSpawnPoint;

    [SerializeField] private GameObject FixedLegPrefab;

    [SerializeField] private GameObject bossHead;

    //패턴 스크립트 받기
    BossPatern2 legAttack;
    [SerializeField] private BossEyePattern eyeAttack;


    private void Awake()
    {
        legAttack = patern2.GetComponent<BossPatern2>();
        InstantiateFixedLeg();
    }
    //공격 명령
    private void Update()
    {
        if (canAttack)
        {
            Patern();
            StartCoroutine("CanAttack");
        }
        //두 다리 모두 Hp가 0보다 작을 경우 그로기 상태
        if (fLeg1== null && fLeg2== null && page == 1) //-------------------------------------------------------------------
        {
            Groggy();
        }


    }
    //패턴 고르기
    private void Patern()
    {
        attackPatern = 2;//Random.Range(1, 5);
        if(page==1)
        {
            attackPatern = Random.Range(1, 4);//1~3까지 포함
            switch (attackPatern)
            {
                case 1://고정다리 패턴인데 
                case 2://일반 다리 패턴//case1과 2일 때 일반 다리 패턴이 나가게 설정 했습니다.
                    legAttack.Attack();
                    break;
                case 3://편의상 5번째 내려치기 패턴을 3번으로 지정
                    //B5.Attack();
                    break;
                default:
                    break;

            }
        }
        else if(page==2) 
        {
            if(attackStack==2)
            {
                attackPatern = Random.Range(0, 3);//0~2까지 포함
                attackStack = 0;
                if (eyeattack) attackPatern = 2;//각성 패턴 중에는 내려치기와 기본공격만 실행

                switch(attackPatern)
                {
                    case 0:
                        //각성 패턴
                        eyeAttack.SpawnEye(this.gameObject);
                        StartCoroutine(ReinforceAttack());
                        break;
                        
                    case 1:
                        //흡입 패턴
                        break;

                    case 2:
                        //내려치기
                        break;
                }
            }
            else//a-a-b
            {
                legAttack.Attack();
                attackStack++;
            }
        }

        /*switch (attackPatern)
        {
            case 1:
                break;
            case 2:
                B2.Attack();
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }*/

    }
    //피격
    public virtual void TakeDamage(float damage)
    {
        /*if (page == 2)//공격 받는 경우 1. 그로기 상태일 때 2. 2페이즈일 때 플레이어의 공격이 collider와 겹쳤을 때
            //bossHead에 collider를 박아놓고 그로기 상태 또는 2페이즈 일때 setActive를 해서 하게 설정
        {
            hp -= damage;
            if (hp <= 0)
                Destroy(this.gameObject);
        }*/
        hp -= damage;
        if (hp <= 60 && page == 1) 
        {
            Debug.Log("Page 2");
            page = 2;
            bossHead.SetActive(true);
        }
        if (hp <= 0)
        {
            Debug.Log("Boss die");
            Destroy(this.gameObject);

        }
           
    }
    //공격 텀
    private IEnumerator CanAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    public IEnumerator Groggy()//각 고정 다리가 사라졌을 경우 이거 실행됨// 고정다리 부분에서 이거 실행시켜야됨
    {
        bossHead.SetActive(true);
        canAttack=false;
        yield return new WaitForSeconds(groggyTime);
        bossHead.SetActive(false);
        canAttack=true;
        if (hp >= 70) 
        {
            InstantiateFixedLeg();
        }
    }

    private void InstantiateFixedLeg()
    {
        int randomNum1 = Random.Range(0, fixedLegSpawnPoint.Count);
        int randomNum2 = Random.Range(0, fixedLegSpawnPoint.Count);
        Debug.Log(randomNum1 + " " + randomNum2);   
        if(randomNum1==randomNum2)
        {
            if (randomNum2 == fixedLegSpawnPoint.Count - 1)
                randomNum2--;
            else
                randomNum2++;
        }
        fixedLeg1 = Instantiate(FixedLegPrefab, fixedLegSpawnPoint[randomNum1], Quaternion.identity);
        fixedLeg2 = Instantiate(FixedLegPrefab, fixedLegSpawnPoint[randomNum2], Quaternion.identity);

        fLeg1 = fixedLeg1.GetComponent<FixedLeg>();
        fLeg2 = fixedLeg2.GetComponent<FixedLeg>();
    }

    public IEnumerator ReinforceAttack()
    {
        Debug.Log("보스 공격 강화");
        float prevAtkSpeed = attackSpeed;
        attackSpeed = 3f;
        yield return new WaitForSeconds(prevAtkSpeed);
        attackSpeed = prevAtkSpeed;
    }

    public float GetAtkSpeed()
    {
        return attackSpeed;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
