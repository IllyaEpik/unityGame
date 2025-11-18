using System;
using System.Threading;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;

public class enemyBot : MonoBehaviour
{
    private float cooldown = 2f;
    private float cooldownCurrent = 0;
    private float canJump = 1;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isAlive = true;

    [Header("References")]
    [SerializeField] private Transform FirePoint;
    [SerializeField] private Transform LeftZone;             
    [SerializeField] private Transform RightZone;            
    [SerializeField] private Transform LeftZoneDetection;    
    [SerializeField] private Transform RightZoneDetection;   
    [SerializeField] private LayerMask heroMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Hero heroObject;
    [SerializeField] private GameObject plasmaPrefab;

    [Header("Audio")]
    [SerializeField] private AudioSource shotSound;


    [Header("Stats")]
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float safeDistance = 3f;
    [SerializeField] private float damagePerHit = 50f;

    [Header("Zone Sizes")]
    public Vector2 attackZoneSize = new Vector2(25, 1);
    public Vector2 FirePointSize = new Vector2(3, 3);
    public Vector2 detectionZoneSize = new Vector2(75, 24);

    public bool isAggressive = true;
    public System.Action dyingEvent = delegate { };

    private float maxHealth = 100f;
    private float currentHealth;
    private Vector2 lastPos;

    [SerializeField] private GameObject healthBarPrefab; // префаб полоски HP 
    private EnemyHealthBar healthBar; // ссылка на созданный бар
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        lastPos = transform.position;
        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            healthBar = bar.GetComponent<EnemyHealthBar>();
            healthBar.target = transform;
            healthBar.offset = new Vector3(0, 5f, 0);
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive || heroObject == null || !isAggressive) return;

        cooldownCurrent -= Time.fixedDeltaTime;

        HandleMovement();
        HandleAttack();
        CheckStuckAndJump();
    }

    private void HandleMovement()
    {
        bool heroInLeftDetection = Physics2D.OverlapBox(LeftZoneDetection.position, detectionZoneSize, 0, heroMask);
        bool heroInLeftAttack = Physics2D.OverlapBox(LeftZone.position, attackZoneSize, 0, heroMask);
        bool groundInLeftPoint = Physics2D.OverlapBox(LeftZone.position, attackZoneSize, 0, groundMask);
        bool heroInRightDetection = Physics2D.OverlapBox(RightZoneDetection.position, detectionZoneSize, 0, heroMask);
        bool heroInRightAttack = Physics2D.OverlapBox(RightZone.position, attackZoneSize, 0, heroMask);
        bool groundInRightPoint = Physics2D.OverlapBox(RightZone.position, attackZoneSize, 0, groundMask);

        bool groundInFirePoint = Physics2D.OverlapBox(FirePoint.position, FirePointSize, 0, groundMask);
        float distance = Vector2.Distance(transform.position, heroObject.transform.position);

        Vector2 lv = rb.linearVelocity;

        // если герой в зоне Detection, но не в атаке
        if ((heroInLeftDetection || heroInRightDetection) && !(heroInLeftAttack || heroInRightAttack) && !groundInFirePoint)
        {
            if (distance > safeDistance)
            {
                Vector2 dir = (heroObject.transform.position - transform.position).normalized;
                lv.x = dir.x * moveSpeed;
                FlipSprite(dir.x);
            }
            else
            {
                // Vector2 dir = (heroObject.transform.position - transform.position).normalized;
                // lv.x = dir.x * -moveSpeed;
                lv.x = 0; // слишком близко — стоим
            }
        }
        else
        {
            lv.x = 0; // герой вне detection-зоны
        }

        rb.linearVelocity = lv;

        float horizontalSpeed = Mathf.Abs(lv.x);
        // animator.SetFloat("moveVector", horizontalSpeed);
        
        animator.SetBool("running",horizontalSpeed > 0.1);
        
    }

    private void HandleAttack()
    {
        bool heroInLeftAttack = Physics2D.OverlapBox(LeftZone.position, attackZoneSize, 0, heroMask);
        bool heroInRightAttack = Physics2D.OverlapBox(RightZone.position, attackZoneSize, 0, heroMask);

        if (heroInLeftAttack)
            Attack(-180);
        else if (heroInRightAttack)
            Attack(0);
    }

    private void Attack(int direction)
    {
        if (cooldownCurrent <= 0)
        {
            Instantiate(plasmaPrefab, FirePoint.position, Quaternion.Euler(0, 0, direction));

            if (shotSound != null)
                shotSound.Play();
            
            cooldownCurrent = cooldown;
        }
    }

    private void FlipSprite(float dirX)
    {
        if (dirX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dirX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void CheckStuckAndJump()
    {
        bool heroInRightAttack = Physics2D.OverlapBox(RightZone.position, attackZoneSize, 0, heroMask);
        bool groundInFirePoint = Physics2D.OverlapBox(FirePoint.position, FirePointSize, 0, groundMask);
        
        bool groundInRightAttack = Physics2D.OverlapBox(LeftZone.position, attackZoneSize, 0, groundMask);
        Debug.Log(groundInRightAttack);
        // float moved = Mathf.Abs(transform.position.x - lastPos.x);
        Vector2 lv = rb.linearVelocity;
        // FirePoint
        // Debug.Log(isGrounded);
        if (groundInFirePoint)
        {
            if (isGrounded && !heroInRightAttack)
            {
                lv.y = jumpForce;
                Vector2 dir = (heroObject.transform.position - transform.position).normalized;
                lv.x = dir.x * -moveSpeed;
                rb.linearVelocity = lv;
                isGrounded = false;
            }
        }

        lastPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
            isGrounded = true;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();

    }

private void Die()
{
    isAlive = false;
    animator.SetTrigger("isDead");
    if (healthBar != null)
        Destroy(healthBar.gameObject); // удалить бар вместе с врагом
    dyingEvent();
    rb.linearVelocity = Vector2.zero;
    Destroy(gameObject, 1f);
    gameObject.layer = LayerMask.NameToLayer("Default");
}

    public void OnHit()
    {
        TakeDamage(damagePerHit);
    }

    // Старый метод для совместимости со старыми скриптами
    public void idk() { }
}