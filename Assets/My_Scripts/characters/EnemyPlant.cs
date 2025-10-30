using UnityEngine;

public class EnemyPlant : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;
    public bool isAlive = true;

    [Header("Attack")]
    public bool isAttack = false;          // оставляем для совместимости
    private bool attacking = false;        // оставляем для совместимости
    [SerializeField] private float attackInterval = 1f; // интервал атаки
    private float attackTimer = 0f;

    [Header("Detection")]
    [SerializeField] private Transform detectionZone;
    [SerializeField] private Transform detectionZoneRight; // оставляем для совместимости
    [SerializeField] private float radius = 5f;
    private LayerMask heroLayer;

    [Header("References")]
    [SerializeField] private Hero heroObject;
    [SerializeField] private GameObject healthBarPrefab;
    private EnemyHealthBar healthBar;

    private Animator animator;
    private bool isLeft = false;

    void Start()
    {
        heroObject = FindFirstObjectByType<Hero>();
        heroLayer = 1 << heroObject.gameObject.layer;

        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Создаём полоску здоровья
        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            healthBar = bar.GetComponent<EnemyHealthBar>();
            healthBar.target = transform;
            healthBar.offset = new Vector3(0, 5f, 0);
        }
    }

    void Update()
    {
        if (!isAlive) return;

        Flip();
        AttackLogic();
    }

    private void Flip()
    {
        Collider2D player = Physics2D.OverlapCircle(detectionZone.position, radius, heroLayer);
        if (player != null)
        {
            if (player.transform.position.x < transform.position.x && !isLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isLeft = true;
            }
            else if (player.transform.position.x > transform.position.x && isLeft)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isLeft = false;
            }
        }
    }

    private void AttackLogic()
    {
        Collider2D player = Physics2D.OverlapCircle(detectionZone.position, radius, heroLayer);

        if (player != null)
        {
            isAttack = true;
            animator.SetBool("isAttack", true);

            // Наносим урон с интервалом
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                heroObject.getDamage();
                attackTimer = 0f;
            }
        }
        else
        {
            isAttack = false;
            animator.SetBool("isAttack", false);
            attackTimer = 0f;
        }
    }

    public void getDamageForPlant()
    {
        TakeDamage(50); // оставляем для совместимости
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("isDead");

        if (healthBar != null)
            Destroy(healthBar.gameObject);

        Destroy(gameObject, 1f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}