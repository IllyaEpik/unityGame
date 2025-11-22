using UnityEngine;

public class DialogueMoverBot : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Target Position (X, Y)")]
    [SerializeField] private Vector2 targetPosition;

    [Header("Dialogue Settings")]
    [SerializeField] private int startMoveAfterLine = 7;

    [Header("Bot Replacement")]
    [SerializeField] private GameObject newBotPrefab;

    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool hasSpawned = false;
    private bool isGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEndedPublic += OnDialogueEnded;
    }

    private void Update()
    {
        CheckGround();

        if (!isMoving && DialogueManager.Instance != null)
        {
            int currentLine = DialogueManager.Instance.GetCurrentLineIndex();
            if (currentLine >= startMoveAfterLine)
                isMoving = true;
        }

        if (isMoving)
            MoveToTarget();
    }

    private void CheckGround()
    {
        if (groundCheckPoint != null)
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        else
            isGrounded = true;
    }

    private void MoveToTarget()
    {
        if (hasSpawned) return;

        Vector2 pos = rb.position;
        float distance = Vector2.Distance(pos, targetPosition);

        if (distance > stopDistance)
        {
            Vector2 dir = (targetPosition - pos).normalized;
            rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);
            if (isGrounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.right * Mathf.Sign(dir.x), 0.5f, groundLayer);
                if (hit.collider != null)
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            SpawnNewBot();
        }
    }

    private void OnDialogueEnded(int lastLine)
    {
        if (lastLine >= startMoveAfterLine)
            isMoving = true;
    }

    private void SpawnNewBot()
    {
        if (hasSpawned) return;

        if (newBotPrefab != null)
            Instantiate(newBotPrefab, transform.position, Quaternion.identity);

        hasSpawned = true;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEndedPublic -= OnDialogueEnded;
    }
}
