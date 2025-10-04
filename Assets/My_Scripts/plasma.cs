using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class plasma : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer spriteRenderer;
    private int moveSpeed = 9;
    // left = -1; right = 1
    public int direction = 1;

    private Hero hero;


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
        transform.Translate(Vector3.right * moveSpeed * direction * Time.deltaTime, Space.World);
        var someone = Physics2D.OverlapCircle(detectionZone.position, 2, layers);
        if (someone!=null)
        {
            var targetBot = someone.GetComponent<enemyBot>();
            if (targetBot!=null)
            {
                targetBot.getDamageForBot();
            }
            var target = someone.GetComponent<EnemyPlant>();
            if (target!=null)
            {
                target.getDamageForPlant();
            }
            
            var targetHero = someone.GetComponent<Hero>();
            if (targetHero!=null)
            {
                targetHero.getDamage();
            }
            Destroy(gameObject);
        }
        
    }
}
