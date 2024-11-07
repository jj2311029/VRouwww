using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private int whatSwordAttack = 0;
    public void doSwordAttack()
    {
        if (whatSwordAttack == 0)
        {
            firstSword();
            whatSwordAttack++;
            StartCoroutine("whatAttack1");

        }
        else if (whatSwordAttack == 1)
        {
            secondSword();
            whatSwordAttack++;
            StopCoroutine("whatAttack1");
            StartCoroutine("whatAttack2");
        }
        else if (whatSwordAttack == 2)
        {
            thirdSword();
            whatSwordAttack = 0;
        }
    }
    private IEnumerator whatAttack1()
    {
        yield return new WaitForSeconds(1.5f);
        whatSwordAttack = 0;
    }
    private IEnumerator whatAttack2()
    {
        yield return new WaitForSeconds(1.5f);
        whatSwordAttack = 0;
    }
    private void firstSword()
    {
        Debug.Log("첫번째");
    }
    private void secondSword()
    {
        Debug.Log("두번째");
    }
    private void thirdSword()
    {
        Debug.Log("세번째");
    }
}
