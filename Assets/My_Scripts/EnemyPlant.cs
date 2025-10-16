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

    

    
    
    private bool attacking = false;

    void Start()
    {
        heroObject = FindFirstObjectByType<Hero>();// находим объект героя в сцене
        hero = 1 << heroObject.gameObject.layer;// получаем слой героя
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();
    }
    public void getDamageForPlant()
    {
        animator.SetTrigger("isDead");
        isDead = true;
        Destroy(gameObject, 0.5f);
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
}
