using UnityEngine;

public class Npc_3 : MonoBehaviour
{
    [Header("Настройки бота")]
    [SerializeField] private GameObject newBotPrefab;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundMask;

    private GameObject activeBot;
    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = true;
    private bool startMoving = false;

    private void Awake()
    {
        activeBot = gameObject;
        rb = activeBot.GetComponent<Rigidbody2D>();
        animator = activeBot.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!startMoving || activeBot == null) return;

        HandleMovement();
        CheckGrounded();
    }

    public void StartBotMovement()
    {
        startMoving = true;
    }

    private void HandleMovement()
    {
        if (Vector2.Distance(activeBot.transform.position, targetPosition) < 0.05f)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("running", false);
            startMoving = false;

            SpawnNewBot();
            return;
        }

        Vector2 dir = (targetPosition - activeBot.transform.position).normalized;
        Vector2 lv = rb.linearVelocity;

        lv.x = dir.x * moveSpeed;

        if (dir.y > 0.1f && isGrounded)
        {
            lv.y = jumpForce;
            isGrounded = false;
        }

        rb.linearVelocity = lv;
        animator.SetBool("running", Mathf.Abs(lv.x) > 0.1f);
        FlipSprite(dir.x);
    }

    private void SpawnNewBot()
    {
        if (newBotPrefab == null || activeBot == null) return;

        Vector3 spawnPos = activeBot.transform.position;

        activeBot.SetActive(false);

        activeBot = Instantiate(newBotPrefab, spawnPos, Quaternion.identity);
        rb = activeBot.GetComponent<Rigidbody2D>();
        animator = activeBot.GetComponent<Animator>();
    }

    private void FlipSprite(float dirX)
    {
        if (dirX > 0)
            activeBot.transform.localScale = new Vector3(1, 1, 1);
        else if (dirX < 0)
            activeBot.transform.localScale = new Vector3(-1, 1, 1);
    }

    private void CheckGrounded()
    {
        if (activeBot == null) return;
        Collider2D groundCheck = Physics2D.OverlapCircle(activeBot.transform.position, 0.1f, groundMask);
        if (groundCheck != null)
            isGrounded = true;
    }
}
