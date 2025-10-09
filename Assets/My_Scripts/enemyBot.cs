using Unity.VisualScripting;
using UnityEngine;

public class enemyBot : MonoBehaviour
{
    private bool isLeft = false;
    Vector2 size = new Vector2(25, 3);

    private float cooldown = 2;
    private float cooldownCurrent = 0;
    private Animator animator;

    [SerializeField] Transform FirePoint;
    [SerializeField] Transform detectionZone;
    [SerializeField] Transform detectionZoneRight;
    [SerializeField] LayerMask hero;
    [SerializeField] Hero heroObject;
    [SerializeField] private GameObject plasmaPrefab;

    public float safeDistance = 3f;
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        cooldownCurrent -= Time.fixedDeltaTime;
        Flip();
        CheckPlayerDistance();
    }
    public void OnPlayerShoot()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
    private void CheckPlayerDistance()
    {
        if (heroObject == null) return;

        float distance = Vector2.Distance(transform.position, heroObject.transform.position);
        if (distance < safeDistance)
        {
            Vector2 dir = (transform.position - heroObject.transform.position).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    public void getDamageForBot()
    {
        Destroy(gameObject);
    }

    private void Flip()
    {
        Collider2D[] right = Physics2D.OverlapBoxAll(detectionZoneRight.position, size, 0f, hero);
        Collider2D[] left = Physics2D.OverlapBoxAll(detectionZone.position, size, 0f, hero);

        if (right.Length != 0 || left.Length != 0)
        {
            foreach (Collider2D col in right)
            {
                if (col.CompareTag("hero"))
                {
                    animator.SetBool("isAttack", true);
                    attackHero(180);
                }
            }
            foreach (Collider2D col in left)
            {
                if (col.CompareTag("hero"))
                {
                    animator.SetBool("isAttack", true);
                    attackHero(0);
                }
            }
        }
        else
        {
            animator.SetBool("isAttack", false);
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
}