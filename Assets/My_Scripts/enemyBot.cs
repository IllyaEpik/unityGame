using Unity.VisualScripting;
using UnityEngine;

public class enemyBot : MonoBehaviour
{
    private bool isLeft = false;

    Vector2 size = new Vector2(25, 3);
    private bool attacking = false;

    private int count = 200;
    private float cooldown = 2;
    private float cooldownCurrent = 0;
    private Animator animator;
    [SerializeField] Transform FirePoint;
    [SerializeField] Transform detectionZone;
    [SerializeField] Transform detectionZoneRight;
    [SerializeField] LayerMask hero;
    [SerializeField] Hero heroObject;
    [SerializeField] private GameObject plasmaPrefab;
        private bool isAttack = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        // Flip();
        // count--;
        cooldownCurrent -= Time.fixedDeltaTime;
        Flip();
        // Collider2D[] left = Physics2D.OverlapBoxAll(detectionZone.position, size, 0f, hero);
        // Debug.Log(count);
    }
    public void getDamageForBot()
    {
        Destroy(gameObject);
    }

    private void Flip()
    {
        Collider2D[] right = Physics2D.OverlapBoxAll(detectionZoneRight.position, size, 0f, hero);
        Collider2D[] left = Physics2D.OverlapBoxAll(detectionZone.position, size, 0f, hero);
        // Debug.Log(left);
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
                    // transform.localScale *= new Vector2(-1, 1);
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
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("hero") && count <= 0)
    //     {
    //         count = 200;
    //         attackHero();
    //     }
    // }

    
}   