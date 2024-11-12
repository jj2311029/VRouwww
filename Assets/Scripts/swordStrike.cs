using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStrike : MonoBehaviour
{
    void Update()
    {
        this.transform.position += -transform.right * 3f;
    }
}
