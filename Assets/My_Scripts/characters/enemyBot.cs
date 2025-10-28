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
    [SerializeField] private Hero heroObject;
    [SerializeField] private GameObject plasmaPrefab;

    [Header("Stats")]
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float safeDistance = 3f;
    [SerializeField] private float damagePerHit = 50f;

    [Header("Zone Sizes")]
    public Vector2 attackZoneSize = new Vector2(25, 8);
    public Vector2 detectionZoneSize = new Vector2(75, 24);

    public bool isAggressive = true;
    public System.Action dyingEvent = delegate { };

    private float maxHealth = 100f;
    private float currentHealth;
    private Vector2 lastPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        lastPos = transform.position;
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
        bool heroInRightDetection = Physics2D.OverlapBox(RightZoneDetection.position, detectionZoneSize, 0, heroMask);
        bool heroInLeftAttack = Physics2D.OverlapBox(LeftZone.position, attackZoneSize, 0, heroMask);
        bool heroInRightAttack = Physics2D.OverlapBox(RightZone.position, attackZoneSize, 0, heroMask);

        float distance = Vector2.Distance(transform.position, heroObject.transform.position);

        Vector2 lv = rb.linearVelocity;

        // если герой в зоне Detection, но не в атаке
        if ((heroInLeftDetection || heroInRightDetection) && !(heroInLeftAttack || heroInRightAttack))
        {
            if (distance > safeDistance)
            {
                Vector2 dir = (heroObject.transform.position - transform.position).normalized;
                lv.x = dir.x * moveSpeed;
                FlipSprite(dir.x);
            }
            else
            {
                lv.x = 0; // слишком близко — стоим
            }
        }
        else
        {
            lv.x = 0; // герой вне detection-зоны
        }

        rb.linearVelocity = lv;
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
        float moved = Mathf.Abs(transform.position.x - lastPos.x);
        Vector2 lv = rb.linearVelocity;

        if (moved < 0.01f && Mathf.Abs(lv.x) > 0.1f && isGrounded)
        {
            lv.y = jumpForce;
            rb.linearVelocity = lv;
            isGrounded = false;
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
        rb.linearVelocity = Vector2.zero;
        dyingEvent();
        Destroy(gameObject, 1f);
    }

    public void OnHit()
    {
        TakeDamage(damagePerHit);
    }

    // Старый метод для совместимости со старыми скриптами
    public void idk() { }
}