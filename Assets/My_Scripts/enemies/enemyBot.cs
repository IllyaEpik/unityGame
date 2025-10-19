using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;

public class enemyBot : MonoBehaviour
{
    // private bool isLeft = false;
    Vector2 size = new Vector2(25, 3);
    private float cooldown = 2f;
    private float cooldownCurrent = 0;
    private Animator animator;
    private int isLeft = 1;

    private bool isAlive = true;


    [SerializeField] Transform FirePoint;
    [SerializeField] Transform detectionZone;
    [SerializeField] Transform detectionZoneRight;
    [SerializeField] LayerMask hero;
    [SerializeField] Hero heroObject;
    [SerializeField] private GameObject plasmaPrefab;

    [SerializeField] private float damagePerHit = 50f; // половина здоровья

    public float safeDistance = 3f;
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    // [SerializeField] private HealthBar bar;



    public float maxHealth = 100;
    public float currentHealth;

    [SerializeField] private EnemyHealthBar healthBar;

    private void Start()
    {
        // Die();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();




        currentHealth = maxHealth;

        // Создаём префаб
        // GameObject bar = Instantiate(Resources.Load<GameObject>("HealthBar"), null);
        // healthBar = bar.GetComponent<HealthBar>();
        healthBar.target = transform; // Привязываем к этому врагу

        
    }

    private void FixedUpdate()
    {
        cooldownCurrent -= Time.fixedDeltaTime;
        Flip();
        // CheckPlayerDistance();
    }
    public void OnPlayerShoot()
    {
        if (isGrounded)
        {
            // , ForceMode2D.Impulse
            
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
    }
    // private void CheckPlayerDistance()
    // {
    //     if (heroObject == null) return;

    //     float distance = Vector2.Distance(transform.position, heroObject.transform.position);
    //     if (distance < safeDistance)
    //     {
    //         Vector2 dir = (transform.position - heroObject.transform.position).normalized;
    //         transform.position += (Vector3)(dir * moveSpeed * Time.fixedDeltaTime);
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
            isGrounded = true;
    }

    // public void getDamageForBot()
    // {
    //     Destroy(gameObject);
    // }

    private void Flip()
    {
        Collider2D[] right = Physics2D.OverlapBoxAll(detectionZoneRight.position, size, 0f, hero);
        Collider2D[] left = Physics2D.OverlapBoxAll(detectionZone.position, size, 0f, hero);

        if (right.Length != 0 || left.Length != 0)
        {
            foreach (Collider2D col in right)
            {
                if (col.CompareTag("heroAttack"))
                {
                    OnPlayerShoot();
                }
                if (col.CompareTag("hero"))
                {
                    // animator.SetBool("isAttack", true);
                    attackHero(isLeft*180);
                    // isLeft = 
                    // transform.localScale *= new Vector2(1, 1);
                }
            }
            foreach (Collider2D col in left)
            {
                if (col.CompareTag("heroAttack"))
                {
                    OnPlayerShoot();
                }
                if (col.CompareTag("hero"))
                {
                    // animator.SetBool("isAttack", true);
                    transform.localScale *= new Vector2(-1, 1);
                    if (isLeft == 0)
                    {
                        isLeft = 1;
                    }
                    else
                    {
                        isLeft = 0;
                    }
                    
                    // attackHero(isLeft*180);
                    Debug.Log(transform.localScale.x);
                }
            }
        }
        else
        {
            // animator.SetBool("isAttack", false);
        }
    }

    private void attackHero(int left)
    {
        if (cooldownCurrent <= 0)
        {
            Instantiate(plasmaPrefab, FirePoint.position, Quaternion.Euler(0, 0, left));
            cooldownCurrent = cooldown;
        }
    }

    public void TakeDamage(float amount)
    {
        // if (!isAlive) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.UpdateHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // isAlive = false;
        Destroy(gameObject);
    }

    public void OnHit()
    {
        TakeDamage(damagePerHit);
    }
}