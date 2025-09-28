using UnityEngine;

public class Battery : MonoBehaviour
{
    // ManagerUi managerUi;
    [SerializeField] private Hero hero;
    void Start()
    {
        // managerUi = GameObject.FindGameObjectWithTag("managerUi").GetComponent<ManagerUi>();
    }


    void Update()
    {
        
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

            hero.batteryUi.sprite = hero.BattaryElems[hero.battery];
            Invoke("DeleteBattery", 0.3f);
        }
    }

    private void DeleteBattery()
    {
        Destroy(gameObject);
    }
}
