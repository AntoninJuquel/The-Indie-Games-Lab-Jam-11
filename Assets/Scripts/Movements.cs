using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour
{
    //Movements variables
    [SerializeField]
    float horizontalAcceleration = 1;
    [SerializeField]
    [Range(0, 1)]
    float horizontalDampingBasic = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float horizontalDampingWhenStopping = 0.5f;
    [SerializeField]
    [Range(0, 1)]
    float horizontalDampingWhenTurning = 0.5f;
    //Jump variables
    [SerializeField] float jumpVelocity = 7f;
    [SerializeField]
    [Range(0, 1)]
    float cutJumpHeight = 0.5f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    public void Move(float horizontalInputValue)
    {
        float horizontalVelocity = rb.velocity.x;
        horizontalVelocity += horizontalInputValue * horizontalAcceleration;

        if (Mathf.Abs(horizontalInputValue) < 0.01f)
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.deltaTime * 10f);
        else if (Mathf.Sign(horizontalInputValue) != Mathf.Sign(horizontalVelocity))
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.deltaTime * 10f);
        else
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingBasic, Time.deltaTime * 10f);

        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
    }

    internal void CutJump()
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * cutJumpHeight);
        }
    }
}
