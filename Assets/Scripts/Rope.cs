using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject tail;//마지막 원

    static public Rigidbody2D FindBefore(HingeJoint2D linkedHinge)
    {
        HingeJoint2D cur = linkedHinge;
        Rigidbody2D connectedRigidbody = linkedHinge.connectedBody;
        Rope headRope;// 유니티에서 Static Rope의 Ropescript

        while (true)//Rope 스크립트가 들어간 오브젝트를 찾을 때 까지 루프
        {
            connectedRigidbody = cur.connectedBody;
            if (connectedRigidbody.GetComponent<Rope>() != null) break;
            cur = connectedRigidbody.GetComponent<HingeJoint2D>();
        }//Found head node

        headRope = connectedRigidbody.gameObject.GetComponent<Rope>();
        HingeJoint2D tailHj = headRope.tail.GetComponent<HingeJoint2D>();
        Rigidbody2D prevObj = headRope.tail.GetComponent<Rigidbody2D>();

        while (tailHj != linkedHinge)//linkedHinge의 전에 있는 오브젝트를 찾을 때 까지 루프
        {
            prevObj = connectedRigidbody;
            connectedRigidbody = tailHj.connectedBody;
            tailHj = connectedRigidbody.GetComponent<HingeJoint2D>();
        }
        return prevObj;
    }
    static public Rigidbody2D FindHead(HingeJoint2D linkedHinge)//FindBefore부분의 Rope스크립트가 들어간 오브젝트를 찾는 구간의 스크립트
    {
        HingeJoint2D cur = linkedHinge;
        Rigidbody2D connectedRigidbody = linkedHinge.connectedBody;

        while (true)
        {
            connectedRigidbody = cur.connectedBody;
            if (connectedRigidbody.GetComponent<Rope>() != null) break;
            cur = connectedRigidbody.GetComponent<HingeJoint2D>();
        }//Found head node
        return connectedRigidbody;
    }
    public GameObject GetTail()
    {
        return tail;
    }
}
