using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Physical
{
    [SerializeField] Vector3 input;
    [SerializeField] int jumpForce;
    [SerializeField] int speed;
    [SerializeField] Volume BlastZone;
    Action<Physical> Power;
    bool isPushing = true;

    private void Start()
    {
        isGrounded = true;
        Power += Repulsion;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W))
            {
                input += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                input += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                input += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                input += Vector3.right;
            }
            input.Normalize();
            movementDir = input;

            movementSpeed += 1;
            if (movementSpeed > 5) movementSpeed = 5;
        }
        else input = Vector3.zero;

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NewApplyForce(Vector3.up * jumpForce);
                isGrounded = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Pew");
            BlastZone.ActionOnObjectsInRange(Power);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Cycling Power");
            if (isPushing)
            {
                Power = Spring;
                isPushing = false;
            }
            else
            {
                Power = Repulsion;
                isPushing = true;
            }
        }
    }

    void Repulsion(Physical target)
    {
        target.NewApplyForceFrom(transform.position, 10);
    }

    void Spring(Physical target)
    {
        StartCoroutine(SpringDelay(target));
        //target.NewApplyForce(Vector3.up * 10);
    }

    IEnumerator SpringDelay(Physical target)
    {
        yield return new WaitForSeconds(2);
        target.NewApplyForce(Vector3.up * 10);

    }

}
