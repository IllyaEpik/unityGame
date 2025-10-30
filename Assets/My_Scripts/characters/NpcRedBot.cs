using UnityEngine;

public class RedBot : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueLine[] dialogueLines;

    [Header("Behavior")]
    public float attackSpeed = 2.5f;
    public Transform firePoint;
    public Transform detectionLeft;
    public Transform detectionRight;
    public LayerMask heroMask;
    public GameObject plasmaPrefab;
    public float attackCooldown = 2f;
    public Vector2 detectionSize = new Vector2(25, 3);

    private float currentCooldown;
    private bool isAggressive = false;
    private Transform target;

    private void Start()
    {
        StartDialogue();
    }

    private void Update()
    {
        currentCooldown -= Time.deltaTime;

        if (isAggressive && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, attackSpeed * Time.deltaTime);
            DetectAndAttack();
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines != null && dialogueLines.Length > 0)
            DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd);
    }

    private void OnDialogueEnd(int lastLineIndex)
    {
        // Пример: после определённой реплики делаем агрессивным
        if (lastLineIndex == dialogueLines.Length - 1)
            BecomeAggressive();
    }

    public void BecomeAggressive()
    {
        if (isAggressive) return;
        isAggressive = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.red;

        GameObject playerGo = GameObject.FindGameObjectWithTag("hero");
        if (playerGo != null) target = playerGo.transform;
    }

    private void DetectAndAttack()
    {
        if (detectionLeft == null || detectionRight == null) return;

        Collider2D[] right = Physics2D.OverlapBoxAll(detectionRight.position, detectionSize, 0f, heroMask);
        Collider2D[] left = Physics2D.OverlapBoxAll(detectionLeft.position, detectionSize, 0f, heroMask);

        if (right.Length > 0 && currentCooldown <= 0f) AttackHero(0);
        if (left.Length > 0 && currentCooldown <= 0f) AttackHero(180);
    }

    private void AttackHero(int angle)
    {
        if (plasmaPrefab != null && firePoint != null)
            Instantiate(plasmaPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        currentCooldown = attackCooldown;
    }
}