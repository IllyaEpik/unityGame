using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private bool isLeft = false;
    private bool isAttack = false;
    private Animator animator;

    [SerializeField] private UnityEngine.UI.Image heart;
    [SerializeField] private Sprite newSprite;
    private Hero health;
    [SerializeField] Transform Circle_trigger_left;
    [SerializeField] Transform Circle_trigger_right;
    [SerializeField] LayerMask hero;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();
    }

    void FixedUpdate()
    {

    }

    private void Flip()
    {
        Collider2D[] check_left = Physics2D.OverlapCircleAll(Circle_trigger_left.position, 5f, hero);
        Collider2D[] check_right = Physics2D.OverlapCircleAll(Circle_trigger_right.position, 5f, hero);
        if (check_left.Length > 0)
        {
            if (isLeft)
            {
                Attack();
                isAttack = true;
                animator.SetBool("isAttack", isAttack);
            }
            else if (!isLeft)
            {
                transform.localScale *= new Vector2(-1, 1);
                isLeft = true;
                Attack();
                isAttack = true;
                animator.SetBool("isAttack", isAttack);
            }
        }
        if (check_right.Length > 0)
        {
            if (!isLeft)
            {
                Attack();
                isAttack = true;
                animator.SetBool("isAttack", isAttack);
            }
            else if (isLeft)
            {
                transform.localScale *= new Vector2(-1, 1);
                isLeft = false;
                Attack();
                isAttack = true;
                animator.SetBool("isAttack", isAttack);
            }
        }
        if (check_left.Length == 0 && check_right.Length == 0)
        {
            isAttack = false;
            animator.SetBool("isAttack", false);
        }
    }

    private void Attack()
    {
        heart.sprite = newSprite;
    }
}
