using UnityEngine;

public class Shield : MonoBehaviour
{

    private ManagerUi managerUi;
    void Start()
    {
        managerUi = GameObject.FindGameObjectWithTag("managerUi").GetComponent<ManagerUi>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hero"))
        {
            Hero hero = collision.GetComponent<Hero>();
            if (hero != null)
            {
                hero.AddShield();
                Invoke("DeleteShield", 0.3f);
            }
        }
    }

    private void DeleteShield()
    {
        Destroy(gameObject);
    }
}
