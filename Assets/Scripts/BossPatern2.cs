using System.Collections;
using UnityEngine;

public class BossPatern2 : MonoBehaviour
{
    public GameObject leg;
    private GameObject legAttack;

    [SerializeField] private float speed = 100f;
    [SerializeField] private float attackTurm = 3f;

    private int legWhere;
    private int currentLegWhere = 0;

    private float randomX;
    private float randomY;
    private int direction = 1;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isMovingForward = true;

    float[,] legPositions = {
        { -22f,  3f, 1 },
        { -22f, -2f, 1 },
        { -22f, -7f, 1 },
        {  22f,  3f, -1 },
        {  22f, -2f, -1 },
        {  22f, -7f, -1 }
    };

    void Update()
    {
        if (legAttack != null)
        {
            // 왕복 이동 구현
            if (isMovingForward)
            {
                legAttack.transform.position = Vector3.MoveTowards(legAttack.transform.position, targetPos, speed * Time.deltaTime);
                if (Vector3.Distance(legAttack.transform.position, targetPos) < 0.1f)
                {
                    isMovingForward = false;  // 목표에 도달하면 반대로 이동
                }
            }
            else
            {
                legAttack.transform.position = Vector3.MoveTowards(legAttack.transform.position, startPos, speed * Time.deltaTime);
                if (Vector3.Distance(legAttack.transform.position, startPos) < 0.1f)
                {
                    isMovingForward = true;  // 다시 시작 위치로 돌아가면 앞으로 이동
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("공격");
            Attack();
        }
    }

    public void Attack()
    {
        Debug.Log("패턴2");

        // 랜덤 인덱스 선택
        legWhere = Random.Range(0, legPositions.GetLength(0));

        // 값 할당
        randomX = legPositions[legWhere, 0];
        randomY = legPositions[legWhere, 1];
        direction = (int)legPositions[legWhere, 2];

        currentLegWhere = legWhere;

        // 방향 결정
        Vector3 legDirection = leg.transform.localScale;
        if (direction == 1)
        {
            legDirection.x = Mathf.Abs(legDirection.x);
        }
        else
        {
            legDirection.x = -Mathf.Abs(legDirection.x);
        }

        // 다리 공격 생성
        Vector2 SpawnPos = new Vector2(randomX, randomY);
        legAttack = Instantiate(leg, SpawnPos, transform.rotation);
        legAttack.transform.localScale = legDirection;

        // 왕복할 목표 위치 설정
        startPos = legAttack.transform.position;
        targetPos = new Vector3(randomX + 20f * direction, randomY, startPos.z);  // 5 단위로 목표 지점 설정

        Destroy(legAttack, 3f);  // 일정 시간 후에 다리 공격 삭제
    }
}
