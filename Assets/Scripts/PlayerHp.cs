using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerHP : MonoBehaviour
{
    [SerializeField] private Slider HpBarSlider; // UI �����̴�
    public float maxHP = 100;     // �ִ� HP
    private float currentHP;      // ���� HP

    PlayerMove Playerscript;

    void Start()
    {
        // �ʱ� HP ����
        currentHP = maxHP;

        HpBarSlider.minValue = 0;
        HpBarSlider.maxValue = maxHP;

        // �����̴��� �ʱ� �� ����
        HpBarSlider.value = currentHP;

        Playerscript = GetComponent<PlayerMove>();
    }

    public void TakeDamage(float damage, Vector2 targetpos)
    {
        currentHP -= damage;
        // ü���� 0 �������� Ȯ��
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die(); // ��� ó��
        }
        CheckHp(); // ü�� UI ����
        Playerscript.OnDamaged(targetpos);//�˹�
    }

    private void Die()
    {
        Debug.Log("�÷��̾� ���!");
        SceneManager.LoadScene("Gameover");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //DecreaseHP(1);  // �浹 �� HP 1 ����*
        }
    }

    public void CheckHp() //hp ��� ������Ʈ
    {
        if (HpBarSlider != null)
            HpBarSlider.value = currentHP;
    }
}