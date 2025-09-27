using UnityEngine;

public class Battery : MonoBehaviour
{
    ManagerUi managerUi;
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
            managerUi.AddBattery();
            Invoke("DeleteBattery", 0.3f);
        }
    }

    private void DeleteBattery()
    {
        Destroy(gameObject);
    }
}
