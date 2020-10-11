using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public event Action OnDie;
    [SerializeField] float jumpPressedRemember = 0, jumpPressedRememberTime = .2f, groundedRemember = 0f, groundedRememberTime = .2f;
    [SerializeField] LayerMask whatIsGround = new LayerMask(), whatIsInteractable = new LayerMask();
    [SerializeField] float interactionRadius = 1f;

    WireController handlingWire;


    Movements movements;
    BoxCollider2D col;
    SpriteRenderer spriteRenderer;
    Animator anim;
    private void Awake()
    {
        Instance = this;
        movements = GetComponent<Movements>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            DataManager.Instance.AddDeath();
            OnDie();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Deadly"))
        {
            DataManager.Instance.AddDeath();
            OnDie();
        }
        else if (collision.CompareTag("End"))
            GameManager.Instance.NextLevel();
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
        anim.SetBool("Grounded", groundedRemember == groundedRememberTime);
    }

    void CheckJump()
    {
        jumpPressedRemember -= (jumpPressedRemember > 0 ? Time.deltaTime : 0);
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        if (jumpPressedRemember > 0 && groundedRemember > 0)
        {
            jumpPressedRemember = 0;
            groundedRemember = 0;
            movements.Jump();

            anim.SetTrigger("Jump");
        }

        if (Input.GetButtonUp("Jump"))
        {
            movements.CutJump();
        }
    }

    void HandleSpritekOrientation()
    {
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 180, 0);
            //spriteRenderer.flipX = true;
            anim.SetBool("IsRunning", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            //spriteRenderer.flipX = false;
            anim.SetBool("IsRunning", true);
        }
        else
            anim.SetBool("IsRunning", false);
    }

    void HandleWireInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, interactionRadius, whatIsInteractable);
            foreach (Collider2D interactable in interactables)
            {
                WireController wire = interactable.GetComponentInParent<WireController>();
                Receptor receptor = interactable.GetComponentInParent<Receptor>();

                if (receptor && handlingWire != null)
                {
                    if (receptor.PlugWire(handlingWire, transform))
                    {
                        handlingWire = null;
                        return;
                    }
                }
                else if (wire && handlingWire == null)
                {
                    handlingWire = wire.GrabWire(transform);

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
        if (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0f)
            foreach (Collider2D platform in Physics2D.OverlapCircleAll(transform.position, 2.5f, whatIsGround))
            {
                if (platform.GetComponent<PlatformController>() && platform.transform.position.y >= transform.position.y)
                    StartCoroutine(platform.GetComponent<PlatformController>().LetGo());
            }
        else if(Input.GetAxisRaw("Vertical") < 0f)
            foreach (Collider2D platform in Physics2D.OverlapCircleAll(transform.position, 2.5f, whatIsGround))
            {
                if (platform.GetComponent<PlatformController>() && platform.transform.position.y <= transform.position.y)
                    StartCoroutine(platform.GetComponent<PlatformController>().LetGo());
            }
    }

    public void PlayFootstep()
    {
        AudioManager.instance.Play("Footstep");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}
