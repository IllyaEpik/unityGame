using UnityEngine;

public class EnemyPlant : MonoBehaviour
{
    public bool isAlive = true; // флаг жизни

    private bool isLeft = false;
    private bool isAttack = false;

    private bool isDead = false;


    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] Transform detectionZone;
    [SerializeField] Transform detectionZoneRight; 
    [SerializeField] float radius = 10f;
    [SerializeField] LayerMask hero ;

    [SerializeField] Hero heroObject;


    [SerializeField] private GameObject healthBarPrefab; // префаб полоски HP
    private EnemyHealthBar healthBar; // ссылка на созданный бар

    
    private bool attacking = false;
    



    public float maxHealth = 100;
    public float currentHealth;

void Start()
{
    heroObject = FindFirstObjectByType<Hero>();
    hero = 1 << heroObject.gameObject.layer;
    animator = GetComponent<Animator>();
    currentHealth = maxHealth;
    // создаём личный бар для этого врага
    if (healthBarPrefab != null)
    {
        GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        healthBar = bar.GetComponent<EnemyHealthBar>();
        healthBar.target = transform;
    }
}

    void Update()
    {
        Flip();
    }
    public void getDamageForPlant()
    {

        // isDead = true;
        TakeDamage(50);
        
    }
    private void Flip()
    {
        Collider2D player = Physics2D.OverlapCircle(detectionZone.position, radius, hero);
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
            if (!attacking)
            {
                attacking = true;
                Invoke("attackHero", 1f);
            }
            isAttack = true;
            animator.SetBool("isAttack", isAttack);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }

    private void attackHero()
    {
        Collider2D player = Physics2D.OverlapCircle(detectionZoneRight.position, radius, hero);
        if (player)
        {
            heroObject.getDamage();
        }
        attacking = false;
    }





    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.UpdateHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
            Die();
        // currentHealth -= amount;
        // if (currentHealth < 0) currentHealth = 0;

        // healthBar.UpdateHealth(currentHealth, maxHealth);

        // if (currentHealth <= 0)

        //     Die();
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        if (healthBar != null)
            Destroy(healthBar.gameObject);
        Destroy(gameObject, 1f);
    }
}
