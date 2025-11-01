using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Splines;

public class RedBot : MonoBehaviour
{
    // [Header("Диалоговые данные")]
    // [SerializeField] private DialogueLine[] dialogueLines;

    // [Header("UI Elements")]
    // [SerializeField] private GameObject dialoguePanel;
    // [SerializeField] private TMP_Text dialogueText;
    // [SerializeField] private Button talkButton;

    // [Header("Choices")]
    // [SerializeField] private Button[] choiceButtons; 
    // [SerializeField] private TMP_Text[] choiceTexts;

    [Header("Behavior")]
    [SerializeField] private float attackSpeed = 2.5f;

    [Header("Attack Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform detectionLeft;
    [SerializeField] private Transform detectionRight;
    [SerializeField] private LayerMask heroMask;
    [SerializeField] private GameObject plasmaPrefab;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Vector2 detectionSize = new Vector2(25, 3);

    [Header("Health & Reinforcements")]
    [SerializeField] public float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private GameObject reinforcementPrefab;
    [SerializeField] private int reinforcementCount = 10;
    [SerializeField] private float reinforcementRadius = 3f;

    private float currentCooldown;
    private bool dialogueActive = false;
    private bool isAggressive = false;
    private bool playerInRange = false;
    private Transform target;
    private DialogueManager dialogScript;
    private enemyBot attackScript;
    void Start()
    {
        currentHealth = maxHealth;
        dialogScript = GetComponent<DialogueManager>();
        attackScript = GetComponent<enemyBot>();
        attackScript.isAggressive = false;
        attackScript.dyingEvent = Die;
        // if (talkButton != null) talkButton.gameObject.SetActive(false);
        // if (dialoguePanel != null) dialoguePanel.SetActive(false);
        // if (talkButton != null) talkButton.onClick.AddListener(OnTalkButtonPressed);

        // if (choiceButtons != null)
        //     foreach (var b in choiceButtons)
        //         if (b != null) b.gameObject.SetActive(false);
        // dialogScript.endAction = new Action<int>(endDialog);
    }
    private void endDialog(int currentLine)
    {
        Debug.Log(currentLine);
        if (currentLine == 3)
        {
            // isAggressive = true;
            attackScript.isAggressive = true;
        }
    }
    void Update()
    {
        currentCooldown -= Time.deltaTime;

        if (isAggressive)
        {
            DetectAndAttack();

            if (target != null)
                transform.position = Vector2.MoveTowards(transform.position, target.position, attackSpeed * Time.deltaTime);
        }
    }

    // --- Dialogue ---
    void StartDialogue()
    {
        // dialogueActive = true;
        // if (dialoguePanel != null) dialoguePanel.SetActive(true);
        // if (dialogueText != null)
        //     dialogueText.text = "Задание: уничтожить синего бота. Он может притворяться, что он твой друг, так что не дай ему договорить.";

        // ShowChoices();
    }

    void ShowChoices()
    {
        // string[] options = { "Согласиться", "Отказаться" };
        // for (int i = 0; i < choiceButtons.Length; i++)
        // {
        //     if (i < options.Length)
        //     {
        //         choiceButtons[i].gameObject.SetActive(true);
        //         choiceTexts[i].text = options[i];

        //         int idx = i;
        //         choiceButtons[i].onClick.RemoveAllListeners();
        //         choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(idx));
        //     }
        //     else
        //     {
        //         choiceButtons[i].gameObject.SetActive(false);
        //     }
        // }
    }

    void OnChoiceSelected(int index)
    {
        // if (index == 0)
        // {
        //     dialogueText.text = "Хорошо. Следи за ним.";
        //     EndDialogue();
        // }
        // else if (index == 1)
        // {
        //     dialogueText.text = "Ты ослушался приказа... Теперь ты враг!";
        //     EndDialogue();
        //     BecomeAggressive();
        // }
    }

    void EndDialogue()
    {
        // dialogueActive = false;
        // if (dialoguePanel != null) dialoguePanel.SetActive(false);

        // if (choiceButtons != null)
        //     foreach (var b in choiceButtons)
        //         b.gameObject.SetActive(false);
    }

    public void BecomeAggressive()
    {
        if (isAggressive) return;
        isAggressive = true;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.red;

        GameObject playerGo = GameObject.FindGameObjectWithTag("hero");
        if (playerGo != null) target = playerGo.transform;
    }

    void DetectAndAttack()
    {
        if (detectionLeft == null || detectionRight == null) return;

        Collider2D[] right = Physics2D.OverlapBoxAll(detectionRight.position, detectionSize, 0f, heroMask);
        Collider2D[] left = Physics2D.OverlapBoxAll(detectionLeft.position, detectionSize, 0f, heroMask);

        if (right.Length > 0 && currentCooldown <= 0f) AttackHero(0);
        if (left.Length > 0 && currentCooldown <= 0f) AttackHero(180);
    }

    void AttackHero(int angle)
    {
        if (plasmaPrefab != null && firePoint != null)
            Instantiate(plasmaPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        currentCooldown = attackCooldown;
    }

    // --- Damage & Death ---
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        EnemyHealthBar hb = GetComponentInChildren<EnemyHealthBar>();
        if (hb != null) hb.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        SpawnReinforcements();
        attackScript.dyingEvent = attackScript.idk;
        // Destroy(gameObject, 1f);
    }

    void SpawnReinforcements()
    {
        if (reinforcementPrefab == null) return;

        Vector2 center = transform.position;
        for (int i = 0; i < reinforcementCount; i++)
        {
            Vector2 rnd = UnityEngine.Random.insideUnitCircle * reinforcementRadius;
            Vector2 spawnPos = center + rnd;
            Instantiate(reinforcementPrefab, spawnPos, Quaternion.identity);
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("hero"))
    //     {
    //         playerInRange = true;
    //         if (talkButton != null) talkButton.gameObject.SetActive(true);
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("hero"))
    //     {
    //         playerInRange = false;
    //         if (talkButton != null) talkButton.gameObject.SetActive(false);
    //         EndDialogue();
    //     }
    // }

    // void OnTalkButtonPressed()
    // {
    //     if (!dialogueActive) StartDialogue();
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (detectionLeft != null) Gizmos.DrawWireCube(detectionLeft.position, detectionSize);
        if (detectionRight != null) Gizmos.DrawWireCube(detectionRight.position, detectionSize);
    }
}