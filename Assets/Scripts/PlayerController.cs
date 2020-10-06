using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpPressedRemember = 0, jumpPressedRememberTime = .2f, groundedRemember = 0f, groundedRememberTime = .2f;
    [SerializeField] LayerMask whatIsGround = new LayerMask();

    Movements movements;
    BoxCollider2D col;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        movements = GetComponent<Movements>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        CheckGround();
        CheckJump();
        HandleSpritekOrientation();
    }
    private void FixedUpdate()
    {
        movements.Move(Input.GetAxisRaw("Horizontal"));
    }
    void CheckGround()
    {
        Vector2 groundedBoxCheckPosition = (Vector2)transform.position + new Vector2(0, -0.01f);
        Vector2 groundedBoxCheckScale = col.size + new Vector2(-0.02f, 0);

        groundedRemember -= (groundedRemember>0 ? Time.deltaTime : 0);

        if (Physics2D.OverlapBox(groundedBoxCheckPosition, groundedBoxCheckScale, 0, whatIsGround))
        {
            groundedRemember = groundedRememberTime;
        }
    }

    void CheckJump()
    {
        jumpPressedRemember -= (jumpPressedRemember>0 ? Time.deltaTime : 0);
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        if (jumpPressedRemember > 0 && groundedRemember > 0)
        {
            jumpPressedRemember = 0;
            movements.Jump();
        }
    }

    void HandleSpritekOrientation()
    {
        if (Input.GetAxisRaw("Horizontal") < 0f)
            spriteRenderer.transform.rotation = Quaternion.Euler(0,180,0);
            //spriteRenderer.flipX = true;
        else if(Input.GetAxisRaw("Horizontal") > 0f)
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            //spriteRenderer.flipX = false;
    }
}
