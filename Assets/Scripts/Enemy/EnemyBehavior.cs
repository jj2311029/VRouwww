using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBehavior : MonoBehaviour
{
    #region Inspector Variables    
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Transform enemySprite;
    [SerializeField] protected float timer;//attack cooltime
    public GameObject hotZone;
    public GameObject triggerArea;
    public float curveTime = 0f;//돌아보는 시간
    #endregion

    #region unvisible Variables

    protected Transform leftLimit;
    protected Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    protected Animator animator;
    protected float distance;
    protected bool attackMode;
    protected GameObject Ground;
    [HideInInspector] public bool cooling;
    protected float intTimer;
    #endregion


    protected void Awake()
    {
        GameObject leftObj = new GameObject("LeftLimit");
        GameObject rightObj = new GameObject("RightLimit");
        leftLimit = leftObj.transform;
        rightLimit = rightObj.transform;

        SelectTarget();
        intTimer=timer;
        animator = GetComponent<Animator>();
    }
    protected void Update()
    {
        if(!attackMode)
        { 
            Move();
            Debug.Log("Move");
        }

        if (!InsideOfLimits() && !inRange )
        {
            SelectTarget();
            Debug.Log("Select Target");
        }
        /*if (inRange)
        {
            hit = Physics2D.Raycast(raycast.position, transform.right, -raycastLength, raycastMask);
            RaycastDebugger();
        }
        
        if (hit.collider != null)
        {
            EnemyLogic();
            Flip();
            Debug.Log("Enemy Logic");
        }
        else if(hit.collider == null)
        {
            inRange = false;
            Debug.Log("Raycast null");
        }*/


        if (inRange /*== false*/)
        {
            /*StopAttack();
            
            Debug.Log("Out Range");*/
            EnemyLogic();
        }
    }

    /*protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player"))
        {
            target = collision.gameObject;
            inRange = true;
            Invoke("Flip", curveTime);
        }
    }*/
    /*protected void OnTriggerStay2D(Collider2D collision)
    {
        if(target==null&&collision.gameObject.tag=="Player")
        {
            target = collision.transform.position;
            inRange = true;
            Invoke("Flip", curveTime);
        }
    }*/
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer==6)
        {
            Ground=collision.gameObject;
            SpriteRenderer spriteRenderer = collision.collider.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                return;
            }

            Bounds bounds = spriteRenderer.bounds;

            leftLimit.transform.position = new Vector2(bounds.min.x, bounds.max.y);
            rightLimit.transform.position= new Vector2(bounds.max.x, bounds.max.y);
        }
        
    }


    protected void EnemyLogic()
    {
        distance = Vector2.Distance(enemySprite.transform.position, target.transform.position);
        Debug.Log(enemySprite.transform.position+"and"+ target);
        if (distance > attackDistance)
        {
            StopAttack();
            Debug.Log("Stop Attack");
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
            Debug.Log("Attack");
        }
        if (cooling)
        {
            CoolDown();
            animator.SetBool("Attack", false);
            Debug.Log("cooling");
        }
    }
    public bool CheckPlatform()
    {
        if (target != null)
        {
            return Mathf.Abs(target.transform.position.y - transform.position.y) < 0.5f;
        }
        return false;
    }

    protected void Attack()
    {
        timer=intTimer;
        attackMode=true;
        animator.SetBool("Attack", true);
        animator.SetBool("CanWalk", false);
        
    }

    protected void StopAttack()
    {
        cooling = false;
        attackMode =false;
        animator.SetBool("Attack",false);
    }
    protected void Move()
    {
        animator.SetBool("CanWalk",true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            Vector2 targetPos = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position,targetPos,moveSpeed*Time.deltaTime);
        }
    }

    /*protected void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(raycast.position,transform.right*-raycastLength,Color.red);
        }
        else if(attackDistance>distance)
        {
            Debug.DrawRay(raycast.position, transform.right * -raycastLength, Color.green);
        }

    }*/
     protected void CoolDown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }
    protected void TriggerCooling()
    {
        cooling=true;
    }
    protected bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.transform.position.x && transform.position.x < rightLimit.transform.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.transform.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.transform.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit.transform;
        }
        else
        {
            target = rightLimit.transform;
        }
        
        Flip();


    }
    public void Flip()
    {
        Vector3 rotation=transform.eulerAngles;
        if (transform.position.x < target.transform.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }
    public bool GetAttackMode()
    {
        return attackMode;  
    }
}
