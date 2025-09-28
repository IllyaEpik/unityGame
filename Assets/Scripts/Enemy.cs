using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    private bool isLeft = false;
    private bool isAttack = false;
    private Animator animator;

    [SerializeField] Transform detectionZone; // точка проверки (перед врагом)
    [SerializeField] Transform detectionZoneRight; 
    [SerializeField] float radius = 5f;
    [SerializeField] LayerMask hero;
    [SerializeField] Hero heroObject;
    private bool attacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();
    }

    private void Flip()
    {
        Collider2D player = Physics2D.OverlapCircle(detectionZone.position, radius, hero);
        Debug.Log(player);
        if (player != null)
        {
            // если игрок слева
            if (player.transform.position.x < transform.position.x && !isLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isLeft = true;
                // heroObject.getDamage();
            }
            // если игрок справа
            else if (player.transform.position.x > transform.position.x && isLeft)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isLeft = false;
                // Invoke("attackHero", 11100);
            }
            if (!attacking)
            {
                attacking = true;
                Invoke("attackHero", 1f);
            }
            isAttack = true;
            animator.SetBool("isAttack", isAttack);
            // heroObject.getDamage(); 
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }
    private void attackHero()
    {
        Collider2D player = Physics2D.OverlapCircle(detectionZoneRight.position, radius, hero);
        // if (player.transform.position.x < transform.position.x && !isLeft)
        // {
        //     heroObject.getDamage();
        // }
        // else if (player.transform.position.x > transform.position.x && isLeft)
        // {
        //     heroObject.getDamage();
        // }
        if (player)
        {
            heroObject.getDamage();
        }
        attacking = false;
    }


}