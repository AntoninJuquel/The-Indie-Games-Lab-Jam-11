using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpPressedRemember = 0, jumpPressedRememberTime = .2f, groundedRemember = 0f, groundedRememberTime = .2f;
    [SerializeField] LayerMask whatIsGround = new LayerMask(), whatIsInteractable = new LayerMask(), whatIsPlatform = new LayerMask();
    [SerializeField] float interactionRadius = 1f;

    WireController handlingWire;


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
        HandleWireInteraction();
        HandlePlatformInteraction();
    }
    private void FixedUpdate()
    {
        movements.Move(Input.GetAxisRaw("Horizontal"));
    }
    void CheckGround()
    {
        Vector2 groundedBoxCheckPosition = (Vector2)transform.position + new Vector2(0, -0.01f);
        Vector2 groundedBoxCheckScale = col.size + new Vector2(-0.02f, 0);

        groundedRemember -= (groundedRemember > 0 ? Time.deltaTime : 0);

        if (Physics2D.OverlapBox(groundedBoxCheckPosition, groundedBoxCheckScale, 0, whatIsGround))
        {
            groundedRemember = groundedRememberTime;
        }
    }

    void CheckJump()
    {
        jumpPressedRemember -= (jumpPressedRemember > 0 ? Time.deltaTime : 0);
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        if (jumpPressedRemember > 0 && groundedRemember > 0)
        {
            jumpPressedRemember = 0;
            groundedRemember = 0;
            movements.Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            movements.CutJump();
        }
    }

    void HandleSpritekOrientation()
    {
        if (Input.GetAxisRaw("Horizontal") < 0f)
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 180, 0);
        //spriteRenderer.flipX = true;
        else if (Input.GetAxisRaw("Horizontal") > 0f)
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        //spriteRenderer.flipX = false;
    }

    void HandleWireInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D interactable = Physics2D.OverlapCircle(transform.position, interactionRadius, whatIsInteractable);

            if (interactable)
            {
                WireController wire = interactable.GetComponentInParent<WireController>();
                Receptor receptor = interactable.GetComponentInParent<Receptor>();
                if (wire && handlingWire == null)
                    handlingWire = wire.GrabWire(transform);
                else if (receptor && handlingWire != null)
                {
                    if (receptor.PlugWire(handlingWire, transform))
                        handlingWire = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && handlingWire)
        {
            handlingWire.DropWire();
            handlingWire = null;
        }
    }
    void HandlePlatformInteraction()
    {
        if (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") < 0f)
        {
            foreach (Collider2D platform in Physics2D.OverlapCircleAll(transform.position, 2.5f, whatIsPlatform))
            {
                StartCoroutine(platform.GetComponent<PlatformController>().LetGo());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}
