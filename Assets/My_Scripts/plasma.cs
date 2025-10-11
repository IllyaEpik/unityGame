using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
#nullable enable
public class plasma : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer spriteRenderer;
    private int moveSpeed = 9;
    // private GameObject currentGameObject = ;
    // left = -1; right = 1
    private int direction = -1;
    [SerializeField] private Transform detectionZone;
    [SerializeField] LayerMask layers;
    // Hero heroObject;
    // [SerializeField] 
    void Start()
    {

        
        spriteRenderer = GetComponent<SpriteRenderer>();
        // detectionZone = GetComponent<BoxCollider2D>();
        // heroObject = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
    }

    // Update is called once per frame
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
            // var checkBot = someone.GetComponentInChildren<BoxCollider2D>();
            // 
            enemyBot? targetBot = someone.GetComponent<enemyBot>();
            if (someone.CompareTag("enemy") && targetBot != null)
            {
                targetBot.getDamageForBot();
                RemoveAttack();
                return;
            }

            EnemyPlant? target = someone.GetComponent<EnemyPlant>();
            if (target != null)
            {
                target.getDamageForPlant();
                RemoveAttack();
                // return;
            }

            Hero? targetHero = someone.GetComponent<Hero>();
            if (targetHero != null)
            {
                targetHero.getDamage(); 
                // RemoveAttack();
                // return;
                RemoveAttack();
                
            }
            plasma? enemyAttack = someone.GetComponent<plasma>();
            if (enemyAttack != null)
            {
                enemyAttack.RemoveAttack();
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
}
