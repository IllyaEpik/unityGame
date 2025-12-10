using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
#nullable enable
public class plasma : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int moveSpeed = 9;

    private int direction = -1;
    [SerializeField] private Transform detectionZone;
    [SerializeField] LayerMask layers;

    [SerializeField] public float damage = 50f; // урон пули

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.Translate(transform.rotation.z != 0 ? Vector3.left * moveSpeed * direction * Time.deltaTime : Vector3.right * moveSpeed * direction * Time.deltaTime, Space.World);

        Collider2D? collider2D = Physics2D.OverlapCircle(detectionZone.position, 2, layers);
        if (collider2D != null)
        {
            GameObject someone = collider2D.gameObject;
            Debug.Log("Пуля попала в: " + someone.name);

            enemyBot? targetBot = someone.GetComponent<enemyBot>();
            if (someone.CompareTag("enemy") && targetBot != null)
            {
                targetBot.OnHit();
                RemoveAttack();
                return;
            }

            EnemyPlant? target = someone.GetComponent<EnemyPlant>();
            if (target != null)
            {
                target.getDamageForPlant();
                RemoveAttack();
            }

            Hero? targetHero = someone.GetComponent<Hero>();
            if (targetHero != null)
            {
                targetHero.getDamage();
                RemoveAttack();
            }

            plasma? enemyAttack = someone.GetComponent<plasma>();
            if (enemyAttack != null)
            {
                enemyAttack.RemoveAttack();
                RemoveAttack();
                return;
            }

            if (someone.CompareTag("ground"))
            {
                RemoveAttack();
                return;
            }

            return;
        }
    }

    void RemoveAttack()
    {
        Debug.Log(32232323);
        Destroy(gameObject);
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Alien alien = collision.GetComponent<Alien>();
        if (alien != null)
        {
            alien.TakeDamage(damage);
            Destroy(gameObject); 
            return;
        }

        if (collision.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }
}