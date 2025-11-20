using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private Hero hero;
    public float jetpackForce = 10f;          // сила джетпака
    public float horizontalBoost = 1f;        // горизонтальная тяга
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
        if (isUsingJetpack && hero.battery > 0)
        {
            // Горизонтальная тяга
            rb.AddForce((hero.isLeft ? Vector2.left : Vector2.right) * horizontalBoost, ForceMode2D.Force);

            // Вертикальная тяга
            rb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);

            // Анимация (если по какой-то причине не включилась в StartJetpack)
            if (animator != null)
                animator.SetBool("isFlying", true);

            // Расход батареи
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
            // Выключаем анимацию, когда джетпак не используется
            if (animator != null)
                animator.SetBool("isFlying", false);
            justStarted = false; // сброс флага старта
        }
    }

    // Запуск джетпака через UI
    public void StartJetpack()
    {
        if (hero.battery > 0 && !justStarted)
        {
            isUsingJetpack = true;
            justStarted = true;

            // Обнуляем вертикальную скорость для мгновенного отрыва
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            // Импульс вверх
            rb.AddForce(Vector2.up * jetpackForce * 1.5f, ForceMode2D.Impulse);

            // Сразу запускаем анимацию
            if (animator != null)
                animator.SetBool("isFlying", true);
        }
    }

    // Остановка джетпака
    public void StopJetpack()
    {
        isUsingJetpack = false;

        if (animator != null)
            animator.SetBool("isFlying", false);
    }
}