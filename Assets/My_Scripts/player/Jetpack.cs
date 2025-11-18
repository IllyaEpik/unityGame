using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private Hero hero;
    public float jetpackForce = 10f;        
    public float horizontalBoost = 1f;      
    public bool isUsingJetpack = false;

    private Rigidbody2D rb;
    public Animator animator;
    private ManagerUi manager;
    private float batteryTimer = 0f;
    private bool justStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        manager = FindFirstObjectByType<ManagerUi>();
    }

    void FixedUpdate()
    {
        if (isUsingJetpack && hero.battery > 0 && !hero.isGround)
        {
            rb.AddForce((hero.isLeft ? Vector2.left : Vector2.right) * horizontalBoost, ForceMode2D.Force);
            rb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);

            if (animator != null)
                animator.SetBool("isFlying", true);

            batteryTimer += Time.fixedDeltaTime;
            if (batteryTimer >= 2f)
            {
                hero.battery -= 1;
                batteryTimer = 0f;
                manager.updateBattery();
                if (hero.battery < 0) hero.battery = 0;
            }
        }
        else
        {
            if (animator != null)
                animator.SetBool("isFlying", false);
            justStarted = false;
        }
    }

    public void StartJetpack()
    {
        if (hero.battery > 0 && !justStarted && !hero.isGround)
        {
            isUsingJetpack = true;
            justStarted = true;

            rb.AddForce(Vector2.up * jetpackForce * 1.5f, ForceMode2D.Impulse);

            if (animator != null)
                animator.SetBool("isFlying", true);
        }
    }

    public void StopJetpack()
    {
        isUsingJetpack = false;

        if (animator != null)
            animator.SetBool("isFlying", false);
    }
}