using TMPro;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private Hero hero;
    public float jetpackForce = 10f;
    public float horizontalBoost = 1f;
    // public Sprite normalSprite;
    // public Sprite jetpackSprite;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Animator animator;
    public bool isUsingJetpack = false;
    private float batteryTimer = 0f;
    private ManagerUi manager;
    private bool jetpackMode = false;
    public TMP_Text buttonText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // sr.sprite = normalSprite;
        sr.flipX = false;
    }

    void Update()
    {
        // Debug.Log(hero.battery);
        // if (hero.battery > 0 && Input.GetKey(KeyCode.Space))
        // {
        //     isUsingJetpack = true;
        //     animator.SetBool("isFlying", true);
        //     // sr.sprite = jetpackSprite;
        // }
        // else
        // {
        //     isUsingJetpack = false;
        //     animator.SetBool("isFlying", false);
        //     // sr.sprite = normalSprite;

        // }

        // float horizontalInput = Input.GetAxis("Horizontal");
        // if (isUsingJetpack && horizontalInput != 0)
        //     rb.AddForce(Vector2.right * horizontalInput * horizontalBoost);

        // if (horizontalInput < 0)
        //     sr.flipX = true;
        // else
        //     sr.flipX = false;

        // if (hero.isGround)
        // {
        //     jetpackMode = false;
        //     if (buttonText != null)
        //     {
        //         buttonText.text = "Jump";
        //     }
        // }
    }

    void FixedUpdate()
    {
        if (isUsingJetpack)
        {
            rb.AddForce(Vector2.up * jetpackForce);
            rb.AddForce(hero.isLeft ? Vector2.left * jetpackForce * 10 : Vector2.right * jetpackForce * 10);
            batteryTimer += Time.fixedDeltaTime;
            animator.SetBool("isFlying", true);
            Debug.Log("Jetpack animation triggered!");

            if (batteryTimer >= 2f)
            {
                hero.battery -= 1;
                batteryTimer = 0f;
                manager.updateBattery();
                if (hero.battery < 0) hero.battery = 0;
            }
        }
        if (!isUsingJetpack)
        {
            animator.SetBool("isFlying", false);
        }
    }

public void UseJetpackFromUI()
{
    if (hero.battery > 0)
    {
        isUsingJetpack = true;
        animator.SetBool("isFlying", true);
        Debug.Log("Jetpack animation triggered!");

        rb.AddForce(Vector2.up * jetpackForce);
        rb.AddForce(hero.isLeft ? Vector2.left * jetpackForce * 10 : Vector2.right * jetpackForce * 10);

        
        if (batteryTimer >= 4f)
        {
            hero.battery -= 1;
            batteryTimer = 0f;
            manager.updateBattery();
            if (hero.battery < 0) hero.battery = 0;
        }
    }
    else
    {
        isUsingJetpack = false;
        animator.SetBool("isFlying", false);
        Debug.Log("Jetpack animation triggered!");
    }
}
}