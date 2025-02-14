using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public GameObject player;  // 플레이어 오브젝트
    public GameObject parring; // 패링 이펙트 오브젝트 (필요 시 사용)

    private Animator anim;

    private void Start()
    {
        anim = player.GetComponent<Animator>();  // 플레이어의 Animator 컴포넌트 가져오기
    }

    void Update()
    {
        // 패링 성공 시 애니메이션 실행
        if (Input.GetKeyDown(KeyCode.P)) // 예시: P 키를 누르면 패링 실행
        {
            ParrySuccess();
        }
    }

    void ParrySuccess()
    {
        anim.SetTrigger("Parry"); // "Parry" 애니메이션 실행
    }
}