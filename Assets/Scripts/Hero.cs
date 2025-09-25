using UnityEngine;

public class Hero : MonoBehaviour


{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveVector;
    [SerializeField] private float speed;
    [SerializeField] float jumpForce = 1000;
    private bool isJump;
    private bool isGround = true;
    [SerializeField] LayerMask ground;
    [SerializeField] Transform zoneGround;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
        CheckGround();
    }
    void FixedUpdate()
    {
        Walk();
        Flip();
    }

    private void Walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
        animator.SetFloat("moveVector", Mathf.Abs(moveVector.x));
    }
    private bool isLeft = false;

    private void Flip()
    {
        if ((!isLeft && moveVector.x < 0) || (isLeft && moveVector.x > 0))
        {
            transform.localScale *= new Vector2(-1, 1);
            isLeft = !isLeft;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJump = true;
            animator.SetBool("isJump", isJump);
            rb.AddForce(Vector2.up * jumpForce);
            Invoke("TimeJump", 1f);
        }
    }
    private void TimeJump()
    {
        isJump = false;
        animator.SetBool("isJump", isJump);
    }
    private void CheckGround()
    {
        Collider2D[] gr = Physics2D.OverlapCircleAll(zoneGround.position, 1.3f, ground);
        if (gr.Length > 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        animator.SetBool("isGround", isGround);
    }
}
