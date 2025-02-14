using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] protected float hp = 100;
    [SerializeField] protected float attackSpeed = 3f;

    protected int page = 1;
    private int attackPatern = 0;
    private bool canAttack = true;
    private int attackStack = 0;
    bool eyeattack = false;
    public Animator anim; 

    public Collider2D headCollider;

    private float groggyTime = 10f;
    bool isGroggy=false;


    [Header("Leg Patern")]
    public GameObject patern2;//Leg Patern
    BossPatern2 legAttack;


    [Header("Fixed Leg")]
    [SerializeField] private GameObject FixedLegPrefab;
    private GameObject fixedLeg1;
    private GameObject fixedLeg2;
    private FixedLeg fLeg1;
    private FixedLeg fLeg2;//--------------------------------------------------------------------------고정다리 스크립트
    [SerializeField] List<Vector2> leftFixedLegSpawnPoint;
    [SerializeField] List<Vector2> RightFixedLegSpawnPoint;


    [Header("Arousal Patern")]
    [SerializeField] private BossEyePattern eyeAttack;
    [SerializeField] private ThunderEffect thunderEffect;

    [Header("FallDown Patern")]
    [SerializeField] private BossFallDownPatern fallDownAttack;

    [Header("Suck Patern")]
    [SerializeField] private BossSuckPatern suckAttack;


    private void Awake()
    {
        legAttack = patern2.GetComponent<BossPatern2>();
        InstantiateFixedLeg();
        if (anim != null) GetComponent<Animator>();
        canAttack = false;
        CanAttack();
    }
    //공격 명령
    private void Update()
    {
        if (canAttack)
        {
            Patern();
            StartCoroutine(CanAttack());
        }
        //두 다리 모두 Hp가 0보다 작을 경우 그로기 상태
        if (fLeg1== null && fLeg2== null && page == 1&&isGroggy==false) //-------------------------------------------------------------------
        {
            Debug.Log("Groggy");
            StartCoroutine(Groggy());
        }


    }
    //패턴 고르기
    private void Patern()
    {
        if(page==1)
        {
            attackPatern = Random.Range(1, 4);//1~3까지 포함
            switch (attackPatern)
            {
                case 1:
                case 2:
                    legAttack.Attack();
                    break;
                case 3:
                    fallDownAttack.Attack();
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
                        eyeattack = true;
                        eyeAttack.SpawnEye(this.gameObject);
                        StartCoroutine(ReinforceAttack());
                        break;
                        
                    case 1:
                        anim.SetBool("IsSuck", true);
                        suckAttack.SuckAttack();
                        anim.SetBool("IsSuck", false);
                        break;

                    case 2:
                        fallDownAttack.Attack();
                        break;
                }
            }
            else//a-a-b
            {
                legAttack.Attack();
                attackStack++;
            }
        }

    }
    //피격
    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 60 && page == 1) 
        {
            Debug.Log("Page 2");
            headCollider.enabled = true;
            page = 2;
        }
        if (hp <= 0)
        {
            Debug.Log("Boss die");


            //효과음
            SoundManager.Instance.PlaySFX(23);

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

        canAttack = false;
        isGroggy = true;
        headCollider.enabled = true;
        Debug.LogError("Start Groggy");
        anim.SetBool("IsGroggy",true);
        yield return new WaitForSeconds(groggyTime);
        headCollider.enabled = false;
        canAttack = true;
        isGroggy = false;
        anim.SetBool("IsGroggy", false);
        Debug.LogError("End Groggy");
        if (hp >= 70) 
        {
            InstantiateFixedLeg();
        }
    }

    private void InstantiateFixedLeg()
    {
        int randomNum1 = Random.Range(0, leftFixedLegSpawnPoint.Count);
        int randomNum2 = Random.Range(0, RightFixedLegSpawnPoint.Count);
        Debug.Log(randomNum1 + " " + randomNum2);
        fixedLeg1 = Instantiate(FixedLegPrefab, leftFixedLegSpawnPoint[randomNum1], Quaternion.identity);
        fixedLeg2 = Instantiate(FixedLegPrefab, RightFixedLegSpawnPoint[randomNum2], Quaternion.identity);

        fLeg1 = fixedLeg1.GetComponent<FixedLeg>();
        fLeg2 = fixedLeg2.GetComponent<FixedLeg>();
        fLeg1.SetParent(this);
        fLeg2.SetParent(this);
    }

    public IEnumerator ReinforceAttack()
    {
        Debug.Log("보스 공격 강화");

        float prevAtkSpeed = attackSpeed;

        anim.SetBool("IsArousal", true);
        thunderEffect.TriggerThunderOn();
        attackSpeed = 2f;

        yield return new WaitForSeconds(prevAtkSpeed);

        anim.SetBool("IsArousal",false);
        thunderEffect.TriggerThunderOff();
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
    public void DieLeg(FixedLeg fixedLeg)
    {
        if(fLeg1==fixedLeg)
        {
            fLeg1=null;
            fixedLeg1=null;
            Debug.Log("leg1 die");
        }
        else
        {
            fLeg2=null;
            fixedLeg2=null;
            Debug.Log("leg2 die");
        }
    }
}
