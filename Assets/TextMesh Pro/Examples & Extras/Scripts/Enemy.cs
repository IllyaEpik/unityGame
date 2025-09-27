using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isLeft = false;
    private bool isAttack = false;
    private Animator animator;

    [SerializeField] Transform detectionZone; // точка проверки (перед врагом)
    [SerializeField] float radius = 5f;
    [SerializeField] LayerMask hero;

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

        if (player != null)
        {
            // если игрок слева
            if (player.transform.position.x < transform.position.x && !isLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isLeft = true;
            }
            // если игрок справа
            else if (player.transform.position.x > transform.position.x && isLeft)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isLeft = false;
            }
            isAttack = true;
            animator.SetBool("isAttack", isAttack);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionZone != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectionZone.position, radius);
        }
    }
}