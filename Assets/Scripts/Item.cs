using UnityEngine;

public class Item : MonoBehaviour
{
    // ManagerUi managerUi;
    [SerializeField] private Hero hero;
    [SerializeField] private string type = "heart";
    void Start()
    {
        // managerUi = GameObject.FindGameObjectWithTag("managerUi").GetComponent<ManagerUi>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("NO");
        if (collision.CompareTag("hero"))
        {
            // managerUi.AddBattery();
            Debug.Log("collision");
            if (type == "battery")
            {
                if (hero.battery < 5)
                {
                    hero.battery += 1;
                }
                hero.batteryUi.sprite = hero.BattaryElems[hero.battery];
                Invoke("DeleteObject", 0.3f);
            }
            else if (type == "heart")
            {
                if (hero.health < 6)
                {
                    hero.health += 1;
                }
                hero.updateHp();
                Invoke("DeleteObject", 0.3f);
            }

            
        }
    }

    private void DeleteObject()
    {
        Destroy(gameObject);
    }
}
