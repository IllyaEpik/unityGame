using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveVector;
    public static float health = 6f; // hero
    public int battery = 1; // hero
    [SerializeField] private ManagerUi managerUi;
    [SerializeField] private GameObject plasmaPrefab;
    [SerializeField] private UnityEngine.UI.Image shieldIcon; // managerUi or hero
    [SerializeField] private FixedJoystick joystick2;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 1000;
    private bool isJump;
    public bool isLeft = false;
    private bool isGround = true;
    private bool hasShield = false;

    [SerializeField] LayerMask ground;
    [SerializeField] Transform zoneGround;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shieldIcon.enabled = false;
        managerUi.updateHp();
    }
    public void getDamage()
    {
        if (hasShield)
        {
            hasShield = false;
            shieldIcon.enabled = false;
            return;
        }
        health -= 1;
        managerUi.updateHp();
    }

    public void attack()
    {
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        // (this.isLeft) ? Quaternion.Euler(0, 0, 90) :

        Instantiate(plasmaPrefab, transform.position, Quaternion.Euler(0, 0, this.isLeft ? 0 : 180));
        // }
    }
    void Update()
    {
        // Jump();
        CheckGround();
        // attack();
    }
    void FixedUpdate()
    {
        Walk();
        Flip();
    }

    private void Walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        // moveVector.x = 
        moveVector.x = joystick2.Horizontal;
        rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
        animator.SetFloat("moveVector", Mathf.Abs(moveVector.x));
        // Debug.Log(Mathf.Abs(moveVector.x));
    }

    private void Flip()
    {
        if ((!isLeft && moveVector.x < 0) || (isLeft && moveVector.x > 0))
        {
            transform.localScale *= new Vector2(-1, 1);
            isLeft = !isLeft;
            // Sprite newSprite = Resources.Load<Sprite>($"images/Текстури/батарейка/заряд/{battery}");
            // batteryUi.  = newSprite;
            // BattaryElems
            // battery -= 1;
            // batteryUi.sprite = BattaryElems[battery];
        }
    }

    public void Jump()
    {

        // if (Input.GetKeyDown(KeyCode.Space) && isGround)
        // {

            Debug.Log(battery);
            isJump = true;
            animator.SetBool("isJump", isJump);
            rb.AddForce(Vector2.up * jumpForce);
        // }
    }
    private void CheckGround()
    {
        Collider2D[] gr = Physics2D.OverlapCircleAll(zoneGround.position, 1.3f, ground);
        if (gr.Length > 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
            isJump = false;
            animator.SetBool("isJump", isJump);
        }
        animator.SetBool("isGround", isGround);
    }

    public void AddShield()
    {
        hasShield = true;
        shieldIcon.enabled = true;
    }
}