using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float rightOffset = 3f;
    [SerializeField] private float leftOffset = -3f;
    [SerializeField] private float speed = 2f;

    private Vector3 rightPoint;
    private Vector3 leftPoint;
    private bool movingRight = true;

    [Header("Стрельба")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private float bulletDamage = 50f;
    private float fireTimer;

    [Header("Здоровье")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private EnemyHealthBar healthBar;

    [Header("Смерть и падение")]
    [SerializeField] private float deathFallDelay = 0.5f; // задержка перед падением
    [SerializeField] private float fallSpeed = 5f;         // скорость падения
    [SerializeField] private float destroyAfter = 12f;    // время удаления объекта после падения

    private bool isAlive = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D alienCollider;

    void Start()
    {
        rightPoint = transform.position + Vector3.right * rightOffset;
        leftPoint = transform.position + Vector3.right * leftOffset;

        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        alienCollider = GetComponent<Collider2D>();

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (!isAlive) return;

        MoveAlien();
        HandleShooting();
    }

    void MoveAlien()
    {
        if (movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPoint, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, rightPoint) < 0.01f)
                movingRight = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPoint, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, leftPoint) < 0.01f)
                movingRight = true;
        }

        transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
    }

    void HandleShooting()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireCooldown)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, 90));
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        if (rbBullet == null)
        {
            rbBullet = bullet.AddComponent<Rigidbody2D>();
            rbBullet.gravityScale = 0;
            rbBullet.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        rbBullet.linearVelocity = Vector2.down * bulletSpeed;

        if (bullet.GetComponent<Collider2D>() == null)
        {
            CircleCollider2D colBullet = bullet.AddComponent<CircleCollider2D>();
            colBullet.isTrigger = true;
            colBullet.radius = 0.2f;
        }

        AlienBullet logic = bullet.GetComponent<AlienBullet>();
        if (logic == null)
            logic = bullet.AddComponent<AlienBullet>();
        logic.damage = bulletDamage;

        Destroy(bullet, 5f);
    }

    public void TakeDamage(float dmg)
    {
        if (!isAlive) return;

        currentHealth -= dmg;

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (!isAlive) return;
        isAlive = false;

        if (animator != null)
            animator.SetBool("isAlive", false);

        StartCoroutine(FallUntilGround());
    }

    private IEnumerator FallUntilGround()
    {
        yield return new WaitForSeconds(deathFallDelay);

        while (true)
        {
            // Raycast вниз
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("ground"));
            if (hit.collider != null)
            {
                // Ставим тарелку так, чтобы нижняя часть касалась земли
                float alienHalfHeight = alienCollider.bounds.extents.y;
                float groundTop = hit.collider.bounds.max.y;
                transform.position = new Vector3(transform.position.x, groundTop + alienHalfHeight, transform.position.z);
                break;
            }

            // Падаем вниз
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject, destroyAfter);
    }
}

// Пуля тарелки
public class AlienBullet : MonoBehaviour
{
    public float damage = 50f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Урон герою
        Hero hero = collision.GetComponent<Hero>();
        if (hero != null)
        {
            hero.getDamage();
            Destroy(gameObject);
            return;
        }

        // Пуля сталкивается с землей
        if (collision.CompareTag("ground"))
        {
            Destroy(gameObject);
            return;
        }

        // Пули игрока
        plasma playerBullet = collision.GetComponent<plasma>();
        if (playerBullet != null)
        {
            Destroy(playerBullet.gameObject);
            Destroy(gameObject);
        }
    }
}