using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private Hero hero;
    public float jetpackForce = 10f;
    public float horizontalBoost = 5f;
    // public Sprite normalSprite;
    // public Sprite jetpackSprite;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private bool isUsingJetpack = false;
    private float batteryTimer = 0f;

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
        Debug.Log(hero.battery);
        if (hero.battery > 0 && Input.GetKey(KeyCode.Space))
        {
            isUsingJetpack = true;
            animator.SetBool("isFlying", true);
            // sr.sprite = jetpackSprite;
        }
        else
        {
            isUsingJetpack = false;
            animator.SetBool("isFlying", false);
            // sr.sprite = normalSprite;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (isUsingJetpack && horizontalInput != 0)
            rb.AddForce(Vector2.right * horizontalInput * horizontalBoost);

        if (horizontalInput < 0)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    void FixedUpdate()
    {
        if (isUsingJetpack)
        {
            rb.AddForce(Vector2.up * jetpackForce);
            rb.AddForce(hero.isLeft ? Vector2.left * jetpackForce*10 : Vector2.right * jetpackForce*10);
            batteryTimer += Time.fixedDeltaTime;
            if (batteryTimer >= 1f)
            {
                hero.battery -= 1;
                batteryTimer = 0f;
                hero.updateBattery();
                if (hero.battery < 0) hero.battery = 0;
            }
        }
    }
}