using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public GameObject parring; // 패링 이펙트 오브젝트 (필요 시 사용)
    private bool whereParry;
    private Animator anim;

    private void Start()
    {
        anim = parring.GetComponent<Animator>();  // 플레이어의 Animator 컴포넌트 가져오기
    }

    void Update()
    {
        if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT])) // 왼쪽 이동
        {
            whereParry = true;
        }
        else if (Input.GetKey(KeySetting.Keys[KeyAction.RIGHT])) // 오른쪽 이동
        {
            whereParry = false;
        }
    }

    public void ParrySuccess()
    {
        Debug.Log("패링애니");
        if (whereParry)
        {
            anim.SetTrigger("Parry2"); // "Parry" 애니메이션 실행
        }
        else
        {
            anim.SetTrigger("Parry"); // "Parry" 애니메이션 실행
        }
    }
}