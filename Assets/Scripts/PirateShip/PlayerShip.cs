using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public GameObject CannonBall;

    [SerializeField] float shipSpeed = 6f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float cannonSpeed = 15f;
    private Quaternion originalRotation;
    private float maxY = 3.3f;
    private float maxRotationAngle = 35f;
    private bool Wait = true;

    Vector2 currentPos;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        currentPos = transform.position;

        if (Input.GetKey(KeySetting.Keys[KeyAction.UP]))
        {
            if (currentPos.y <= maxY)
            {
                transform.position += Vector3.up * shipSpeed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]))
        {
            if (currentPos.y >= -maxY)
            {
                transform.position += Vector3.down * shipSpeed * Time.deltaTime;

                // 회전 수정: 기존 회전값의 절반만 적용
                float currentZAngle = transform.rotation.eulerAngles.z;
                currentZAngle = currentZAngle > 180 ? currentZAngle - 360 : currentZAngle;

                if (currentZAngle > -120)
                {
                    transform.Rotate(new Vector3(0, 0, -15) * rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Z) && Wait)
        {
            CannonShoot();
            StartCoroutine("WaitAttack");
        }

        if (currentPos.y >= maxY || currentPos.y <= -maxY)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CannonShoot()
    {
        Quaternion currentRotation = transform.rotation;

        GameObject CB = Instantiate(CannonBall, transform.position, currentRotation);

        Rigidbody2D rb = CB.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * cannonSpeed;
        }

        Debug.Log("대포 발사!");
        Destroy(CB, 3f);
    }

    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(1f);
        Wait = true;
    }
}