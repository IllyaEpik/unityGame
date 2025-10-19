using UnityEngine;

public class Battery : MonoBehaviour
{
    private ManagerUi managerUi;
    [SerializeField] private Hero hero;
    private SpriteRenderer sr;
    private float alpha = 0f;
    private float appearSpeed = 1f;

    void Start()
    {
        // managerUi = GameObject.FindGameObjectWithTag("managerUi").GetComponent<ManagerUi>();
        hero = FindFirstObjectByType<Hero>();
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
        transform.localScale = Vector3.zero;
    }


    void Update()
    {
        if (alpha < 1f)
        {
            alpha += Time.deltaTime * appearSpeed;
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hero"))
        {
            // managerUi.AddBattery();
            if (hero.battery < 5)
            {
                hero.battery += 1;
            }

            // managerUi.batteryUi.sprite = managerUi.BattaryElems[hero.battery];
            managerUi.updateBattery();
            Invoke("DeleteBattery", 0.3f);
        }
    }

    private void DeleteBattery()
    {
        Destroy(gameObject);
    }
}
