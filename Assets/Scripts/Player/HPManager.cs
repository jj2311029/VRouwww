using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPManager : MonoBehaviour
{

    public static int hp = 5;

    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject life4;
    public GameObject life5;

    // Use this for initialization
    void Awake()
    {
        life1.GetComponent<Image>().enabled = true;
        life2.GetComponent<Image>().enabled = true;
        life3.GetComponent<Image>().enabled = true;
        life4.GetComponent<Image>().enabled = true;
        life5.GetComponent<Image>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (hp)
        {
            case 4:
                life5.GetComponent<Image>().enabled = false;
                break;
            case 3:
                life4.GetComponent<Image>().enabled = false;
                break;
            case 2:
                life3.GetComponent<Image>().enabled = false;
                break;
            case 1:
                life2.GetComponent<Image>().enabled = false;
                break;
            case 0:
                life1.GetComponent<Image>().enabled = false;
                //game over
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("hit");

            hp -= 1;
            Destroy(gameObject);
        }
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;  // 데미지만큼 체력 감소

        if (hp <= 0)
        {
            Die();  // 체력이 0 이하로 떨어지면 사망
        }

        UpdateLifeUI();  // 체력 UI 업데이트
    }

    private void UpdateLifeUI()
    {
        // HP에 따른 UI 생명 아이콘 비활성화
        life1.GetComponent<Image>().enabled = hp >= 1;
        life2.GetComponent<Image>().enabled = hp >= 2;
        life3.GetComponent<Image>().enabled = hp >= 3;
        life4.GetComponent<Image>().enabled = hp >= 4;
        life5.GetComponent<Image>().enabled = hp >= 5;
    }


    private void Die()
    {
        Debug.Log("플레이어 사망");
        // 플레이어 사망 처리 (게임 오버 처리 등).
        Destroy(gameObject);
    }
}